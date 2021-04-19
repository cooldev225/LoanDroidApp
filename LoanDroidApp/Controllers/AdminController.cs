using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DBSetup;
using DBSetup.Interfaces;
using IdentityServer4.AccessTokenValidation;
using LoanDroidApp;
using LoanDroidApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.data;
using RestSharp;

namespace App.Controllers
{
    [Authorize(Roles="administrator")]
    //[Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountManager _accountManager;
        private readonly ApplicationDbContext _context;
        public AdminController(UserManager<ApplicationUser> userManager, IAccountManager accountManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;            
            _context = context;
            _accountManager = accountManager;
        }
        public void GlobalVariables() {
            ViewData["Title"] = "";
            var representantes = _userManager.GetUsersInRoleAsync("representante").Result.Count();
            var contactors = _userManager.GetUsersInRoleAsync("contactor").Result.Count();
            var servicemanagers = _userManager.GetUsersInRoleAsync("servicio").Result.Count();
            var debuggerdepartments = _userManager.GetUsersInRoleAsync("depuracion").Result.Count();
            var collectiondepartments = _userManager.GetUsersInRoleAsync("coleccion").Result.Count();
            ViewBag.client_count = _userManager.GetUsersInRoleAsync("cliente").Result.Count();
            ViewBag.investor_count = _userManager.GetUsersInRoleAsync("inversora").Result.Count();
            ViewBag.inneraluser_count = representantes + contactors + servicemanagers + debuggerdepartments + collectiondepartments;
            ViewBag.UserRoles = "administrator,representante,contactor,servicio,depuracion,coleccion";
        }
        public IActionResult Index()
        {
            GlobalVariables();
            //var userid = HttpContext.Session.GetString("loan.droid.app.loggedin.userid");
            //if(userid==null) return RedirectToAction(nameof(AccountController.Login), "Account");
            //var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //ViewBag.user = await _userManager.FindByIdAsync(userid);
            return View();
        }
        public IActionResult Roles()
        {
            GlobalVariables(); 
            return View();
        }
        [HttpPost]
        public DatatableRole getRolesDataTable()
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null?0:int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault()==null?0:int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search= Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault(); 
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationRole> query = _context.Set<ApplicationRole>();
            int total = query.Count();
            DatatableRole res = new DatatableRole
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            if (!search.Equals(""))
                query = query.Where(u => u.Name.IndexOf(search) > -1 || u.Description.IndexOf(search) > -1);
            if (
                total>0 && sort.field!=null && sort.sort!=null && !sort.field.Equals("")
            ){
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("name")) query = query.OrderBy(u => u.Name);
                    else if (sort.field.Equals("description")) query = query.OrderBy(u => u.Description);
                    else if (sort.field.Equals("createdDate")) query = query.OrderBy(u => u.CreatedDate);
                    else if (sort.field.Equals("updatedDate")) query = query.OrderBy(u => u.UpdatedDate);
                }
                else
                {
                    if (sort.field.Equals("name")) query = query.OrderByDescending(u => u.Name);
                    else if (sort.field.Equals("description")) query = query.OrderByDescending(u => u.Description);
                    else if (sort.field.Equals("createdDate")) query = query.OrderByDescending(u => u.CreatedDate);
                    else if (sort.field.Equals("updatedDate")) query = query.OrderByDescending(u => u.UpdatedDate);
                }
            }
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public DatatablePermission getPermissionDataTable(string roleId) {
            IQueryable<ApplicationPage> query = _context.Set<ApplicationPage>();
            int total = query.Count();
            List<ApplicationRolePermission> res = new List<ApplicationRolePermission>();
            var data = query.ToList();
            foreach (ApplicationPage page in data) {
                var action = "";
                foreach (string claim in page.Claims.Split(",")) {
                    foreach (ApplicationPermission c in ApplicationPermissions.AllPermissions) {
                        if (claim.Equals(c.Value)) {
                            action +=
                                (action.Equals("")?"":"") +
                                "<input type=\"checkbox\" id=\""+roleId+"_"+c.Value+"\" "+
                                (_context.RoleClaims.Where(u => u.RoleId.Equals(roleId) && u.ClaimValue.Equals(c.Value)).Count() > 0?" checked=\"true\"":"") +
                                " style=\"margin-left:10px;\" "+
                                " onchange=\"AssignAction('"+ roleId + "','"+c.Value+"',$(this).prop('checked'));\""+
                                "> " + 
                                c.Name;
                            break;
                        }
                    }
                }
                res.Add(new ApplicationRolePermission { 
                    Id=page.Id,
                    Name=page.Name,
                    Claims=page.Claims,
                    PageGroup=page.PageGroup,
                    Order=page.Order,
                    Action= action,
                });
            }
            return new DatatablePermission { 
                data=res
            };
        }
        [HttpPost]
        public async Task<JsonResult> assignRolePermission(string roleId, string claim, Boolean chk) {
            ApplicationRole role=await _roleManager.FindByIdAsync(roleId);
            if (chk)
                await _roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
            else
                await _roleManager.RemoveClaimAsync(role, new Claim(ClaimConstants.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
            return Json(new
            {
                msg="ok"
            });
        }
        public IActionResult Users()
        {
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public DatatableUser getUsersDataTable(string role)
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            var query= _userManager.GetUsersInRoleAsync("administrator").Result; 
            if(role==null||role.Equals("")||role.Equals("administrator"))query = _userManager.GetUsersInRoleAsync("administrator").Result;
            else if (role.Equals("representante")) query = _userManager.GetUsersInRoleAsync("representante").Result;
            else if (role.Equals("contactor")) query = _userManager.GetUsersInRoleAsync("contactor").Result;
            else if (role.Equals("servicio")) query = _userManager.GetUsersInRoleAsync("servicio").Result;
            else if (role.Equals("depuracion")) query = _userManager.GetUsersInRoleAsync("depuracion").Result;
            else if (role.Equals("coleccion")) query = _userManager.GetUsersInRoleAsync("coleccion").Result;
            int total = query.Count();
            DatatableUser res = new DatatableUser
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            if (!search.Equals(""))
                query = (IList<ApplicationUser>)query.Where(u => 
                    u.UserName.IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1 ||
                    u.Email.IndexOf(search) > -1 ||
                    u.PhoneNumber.IndexOf(search) > -1 ||
                    u.OtherPhone.IndexOf(search) > -1 ||
                    u.OfficeNumber.IndexOf(search) > -1 ||
                    u.JobTitle.IndexOf(search) > -1
                );
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("userName")) query = query.OrderBy(u => u.UserName).ToList();
                    else if (sort.field.Equals("friendlyName")) query = query.OrderBy(u => u.FriendlyName).ToList();
                    else if (sort.field.Equals("email")) query = query.OrderBy(u => u.Email).ToList();
                    else if (sort.field.Equals("phoneNumber")) query = query.OrderBy(u => u.PhoneNumber).ToList();
                    else if (sort.field.Equals("otherPhone")) query = query.OrderBy(u => u.OtherPhone).ToList();
                    else if (sort.field.Equals("officeNumber")) query = query.OrderBy(u => u.OfficeNumber).ToList();
                    else if (sort.field.Equals("jobTitle")) query = query.OrderBy(u => u.JobTitle).ToList();
                    else if (sort.field.Equals("createdDate")) query = query.OrderBy(u => u.UpdatedDate).ToList();
                    else if (sort.field.Equals("updatedDate")) query = query.OrderBy(u => u.UpdatedDate).ToList();
                }
                else {
                    if (sort.field.Equals("userName")) query = query.OrderByDescending(u => u.UserName).ToList();
                    else if (sort.field.Equals("friendlyName")) query = query.OrderByDescending(u => u.FriendlyName).ToList();
                    else if (sort.field.Equals("email")) query = query.OrderByDescending(u => u.Email).ToList();
                    else if (sort.field.Equals("phoneNumber")) query = query.OrderByDescending(u => u.PhoneNumber).ToList();
                    else if (sort.field.Equals("otherPhone")) query = query.OrderByDescending(u => u.OtherPhone).ToList();
                    else if (sort.field.Equals("officeNumber")) query = query.OrderByDescending(u => u.OfficeNumber).ToList();
                    else if (sort.field.Equals("jobTitle")) query = query.OrderByDescending(u => u.JobTitle).ToList();
                    else if (sort.field.Equals("createdDate")) query = query.OrderByDescending(u => u.UpdatedDate).ToList();
                    else if (sort.field.Equals("updatedDate")) query = query.OrderByDescending(u => u.UpdatedDate).ToList();
                }
            }
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveUser(
            string id,
            string username,
            string firstname,
            string lastname,
            string email,
            string phonenumber,
            string otherphone,
            string officenumber,
            string department,
            string address,
            string avatarimage,
            string role
        ) {
            id = id == null ? "" : id;
            ApplicationUser user = new ApplicationUser
            {
                UserName = username,
                FirstName = firstname,
                LastName = lastname == null ? "" : lastname,
                Email = email,
                PhoneNumber = phonenumber,
                OtherPhone = otherphone == null ? "" : otherphone,
                OfficeNumber = officenumber,
                JobTitle = department == null ? "" : department,
                Address = address == null ? "" : address,
                AvatarImage = CommonUtil.DecodeUrlBase64(avatarimage)//System.Text.Encoding.Unicode.GetBytes(avatarimage)
            };
            if (id.Equals(""))
            {
                string password = "tempP@ss123";
                var result = await _accountManager.CreateUserAsync(user, new string[] {role }, password);
            }
            else {
                ApplicationUser cuser = await _userManager.FindByIdAsync(id);
                cuser.UserName = user.UserName;
                cuser.FirstName = user.FirstName;
                cuser.LastName = user.LastName;
                cuser.PhoneNumber = user.PhoneNumber;
                cuser.OtherPhone = user.OtherPhone;
                cuser.OfficeNumber = user.OfficeNumber;
                cuser.JobTitle = user.JobTitle;
                cuser.Address = user.Address;
                cuser.AvatarImage = user.AvatarImage;
                _context.Entry(cuser).CurrentValues.SetValues(cuser);
                _context.Entry(cuser).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                ApplicationUser cuser = await _userManager.FindByIdAsync(id);
                _context.Remove(cuser);
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        [HttpPost]
        public async Task<ActionResult> resetUserPassword(string id)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, "tempP@ss123");
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        
        public IActionResult Clients()
        {
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public DatatableClient getClientsDataTable()
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            var query = _userManager.GetUsersInRoleAsync("cliente").Result;
            int total = query.Count();
            DatatableClient res = new DatatableClient
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            if (!search.Equals(""))
                query = (IList<ApplicationUser>)query.Where(u =>
                    u.UserName.IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1 ||
                    u.Email.IndexOf(search) > -1 ||
                    u.PhoneNumber.IndexOf(search) > -1 ||
                    u.OtherPhone.IndexOf(search) > -1 ||
                    u.OfficeNumber.IndexOf(search) > -1 ||
                    u.JobTitle.IndexOf(search) > -1
                );
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("userName")) query = query.OrderBy(u => u.UserName).ToList();
                    else if (sort.field.Equals("friendlyName")) query = query.OrderBy(u => u.FriendlyName).ToList();
                    else if (sort.field.Equals("email")) query = query.OrderBy(u => u.Email).ToList();
                    else if (sort.field.Equals("phoneNumber")) query = query.OrderBy(u => u.PhoneNumber).ToList();
                    else if (sort.field.Equals("otherPhone")) query = query.OrderBy(u => u.OtherPhone).ToList();
                    else if (sort.field.Equals("officeNumber")) query = query.OrderBy(u => u.OfficeNumber).ToList();
                    else if (sort.field.Equals("jobTitle")) query = query.OrderBy(u => u.JobTitle).ToList();
                    else if (sort.field.Equals("createdDate")) query = query.OrderBy(u => u.UpdatedDate).ToList();
                    else if (sort.field.Equals("updatedDate")) query = query.OrderBy(u => u.UpdatedDate).ToList();
                }
                else
                {
                    if (sort.field.Equals("userName")) query = query.OrderByDescending(u => u.UserName).ToList();
                    else if (sort.field.Equals("friendlyName")) query = query.OrderByDescending(u => u.FriendlyName).ToList();
                    else if (sort.field.Equals("email")) query = query.OrderByDescending(u => u.Email).ToList();
                    else if (sort.field.Equals("phoneNumber")) query = query.OrderByDescending(u => u.PhoneNumber).ToList();
                    else if (sort.field.Equals("otherPhone")) query = query.OrderByDescending(u => u.OtherPhone).ToList();
                    else if (sort.field.Equals("officeNumber")) query = query.OrderByDescending(u => u.OfficeNumber).ToList();
                    else if (sort.field.Equals("jobTitle")) query = query.OrderByDescending(u => u.JobTitle).ToList();
                    else if (sort.field.Equals("createdDate")) query = query.OrderByDescending(u => u.UpdatedDate).ToList();
                    else if (sort.field.Equals("updatedDate")) query = query.OrderByDescending(u => u.UpdatedDate).ToList();
                }
            }
            var data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            ApplicationClient client = null;
            res.data = new List<ApplicationClient>();
            foreach (ApplicationUser user in data) {
                client = new ApplicationClient(user);
                client.AccountPayment = _context.AccountPayment.Where(u => u.UserId.Equals(user.Id)).ToList();
                res.data.Add(client);
            }
            return res;
        }
        public IActionResult Investors()
        {
            GlobalVariables();
            return View();
        }
        public DatatableInvestor getInvestorsDataTable()
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            var query = _userManager.GetUsersInRoleAsync("inversora").Result;
            int total = query.Count();
            DatatableInvestor res = new DatatableInvestor
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            if (!search.Equals(""))
                query = (IList<ApplicationUser>)query.Where(u =>
                    u.UserName.IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1 ||
                    u.Email.IndexOf(search) > -1 ||
                    u.PhoneNumber.IndexOf(search) > -1 ||
                    u.OtherPhone.IndexOf(search) > -1 ||
                    u.OfficeNumber.IndexOf(search) > -1 ||
                    u.JobTitle.IndexOf(search) > -1
                );
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("userName")) query = query.OrderBy(u => u.UserName).ToList();
                    else if (sort.field.Equals("friendlyName")) query = query.OrderBy(u => u.FriendlyName).ToList();
                    else if (sort.field.Equals("email")) query = query.OrderBy(u => u.Email).ToList();
                    else if (sort.field.Equals("phoneNumber")) query = query.OrderBy(u => u.PhoneNumber).ToList();
                    else if (sort.field.Equals("otherPhone")) query = query.OrderBy(u => u.OtherPhone).ToList();
                    else if (sort.field.Equals("officeNumber")) query = query.OrderBy(u => u.OfficeNumber).ToList();
                    else if (sort.field.Equals("jobTitle")) query = query.OrderBy(u => u.JobTitle).ToList();
                    else if (sort.field.Equals("createdDate")) query = query.OrderBy(u => u.UpdatedDate).ToList();
                    else if (sort.field.Equals("updatedDate")) query = query.OrderBy(u => u.UpdatedDate).ToList();
                }
                else
                {
                    if (sort.field.Equals("userName")) query = query.OrderByDescending(u => u.UserName).ToList();
                    else if (sort.field.Equals("friendlyName")) query = query.OrderByDescending(u => u.FriendlyName).ToList();
                    else if (sort.field.Equals("email")) query = query.OrderByDescending(u => u.Email).ToList();
                    else if (sort.field.Equals("phoneNumber")) query = query.OrderByDescending(u => u.PhoneNumber).ToList();
                    else if (sort.field.Equals("otherPhone")) query = query.OrderByDescending(u => u.OtherPhone).ToList();
                    else if (sort.field.Equals("officeNumber")) query = query.OrderByDescending(u => u.OfficeNumber).ToList();
                    else if (sort.field.Equals("jobTitle")) query = query.OrderByDescending(u => u.JobTitle).ToList();
                    else if (sort.field.Equals("createdDate")) query = query.OrderByDescending(u => u.UpdatedDate).ToList();
                    else if (sort.field.Equals("updatedDate")) query = query.OrderByDescending(u => u.UpdatedDate).ToList();
                }
            }
            var data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            ApplicationInvestor client = null;
            res.data = new List<ApplicationInvestor>();
            foreach (ApplicationUser user in data)
            {
                client = new ApplicationInvestor(user);
                client.AccountPayment = _context.AccountPayment.Where(u => u.UserId.Equals(user.Id)).ToList();
                res.data.Add(client);
            }
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveAccountPayment(
            string id,
            string userid,
            string gatewayname,
            string gatewayurl,
            string gatewayemail,
            string gatewayusername,
            string gatewaypassword,
            string cardfirstname,
            string cardlastname,
            string cardnumber,
            string cardexpirationdate,
            string cardaddress1,
            string cardaddress2,
            string bankcountry,
            string bankcurrency,
            string bankroutingnumber,
            string bankaccontholder,
            string bankname,
            string bankstreet,
            string bankcity,
            string bankregion,
            string bankswiftbicnumber,
            string bankibannumber,
            int type
        )
        {
            id = id == null ? "0" : id;
            AccountPayment acc = new AccountPayment
            {
                UserId = userid,
                GatewayName = gatewayname,
                GatewayUrl = gatewayurl,
                GatewayEmail = gatewayemail,
                GatewayUserName = gatewayusername,
                GatewayPassword = gatewaypassword,
                CardFirstName = cardfirstname,
                CardLastName = cardlastname,
                CardNumber = cardnumber,
                CardExpirationDate = cardexpirationdate,
                CardAddress1 = cardaddress1,
                CardAddress2 = cardaddress2,
                BankCountry = bankcountry,
                BankAccountHolder = bankaccontholder,
                BankName = bankname,
                BankStreet = bankstreet,
                BankCity = bankcity,
                BankRegion = bankregion,
                BankCurrency = bankcurrency,
                BankRoutingNumber = bankroutingnumber,
                BankSwiftBicNumber=bankswiftbicnumber,
                BankIBANNumber=bankibannumber,
                Type=type
            };
            if (id.Equals("0"))
            {
                await _context.AccountPayment.AddAsync(acc);
                await _context.SaveChangesAsync();
            }
            else
            {
                acc.Id = long.Parse(id);
                _context.Entry(acc).CurrentValues.SetValues(acc);
                _context.Entry(acc).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Json(new
            {
                msg = "ok"
            });
        }
        
        [HttpPost]
        public async Task<ActionResult> deleteAccountPayment(long id)
        {
            try
            {
                AccountPayment acc = await _context.AccountPayment.FindAsync(id);
                _context.Remove(acc);
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        [HttpGet]
        public IActionResult Loanreq() {
            GlobalVariables();
            return View();
        }
    }
}