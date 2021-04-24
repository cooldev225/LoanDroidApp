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
    [Authorize]
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
        public bool HasPermmision(string Cliam)
        {
            return User.HasClaim(c => c.Type.Equals("permission") && c.Value.Equals(Cliam));
        }
        public async Task<JsonResult> SaveNotification(string msg, string claim)
        {
            var userId =  _context.CurrentUserId;
            var user = await _userManager.FindByIdAsync(userId);
            Notification noti = new Notification
            {
                Text = msg + " por " + user.UserName,
                Claim = claim
            };
            await _context.Notification.AddAsync(noti);
            await _context.SaveChangesAsync();
            return Json(new
            {
                msg = "ok"
            });
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
            
            IQueryable<ApplicationNotification> query = (
                from a in _context.Set<Notification>().OrderByDescending(u=>u.CreatedDate)
                select new ApplicationNotification
                {
                    Id=a.Id,
                    Text=a.Text,
                    Claim=a.Claim,
                    IsReaded=false,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    CreatedDevice = a.CreatedDevice,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                    UpdatedDevice = a.UpdatedDevice
                });
            int total = query.Count();
            int page = 1;
            int perpage = 10;
            DatatableNotification notis = new DatatableNotification
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            notis.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            for (int i = 0; i < perpage; i++) {
                if (notis.data.Count() <= i) break;
                IQueryable<NotificationReading> reading = _context.Set<NotificationReading>().Where(u => u.CreatedBy.Equals(_context.CurrentUserId));
                if (reading.Count() > 0) notis.data[i].IsReaded = reading.First().IsReaded;
            }
            ViewBag.Notifications = notis;
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
            if (!HasPermmision("roles.view")) return Redirect("index");
            GlobalVariables(); 
            return View();
        }
        [HttpPost]
        public DatatableRole getRolesDataTable()
        {
            if (!HasPermmision("roles.view")) return null;
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
            if (!HasPermmision("roles.view")) return null;
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
                                "<input type=\"checkbox\" "+(HasPermmision("roles.assign")?"":"disabled")+" id=\""+roleId+"_"+c.Value+"\" "+
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
        public async Task<JsonResult> saveRole(
            string id,
            string name,
            string description
        )
        {
            if (!HasPermmision("roles.manage")) return null;            
            if (id == null || id.Equals(""))
            {
                if ((await _accountManager.GetRoleByNameAsync(name)) == null)
                {
                    ApplicationRole applicationRole = new ApplicationRole(name, description);
                    var result = await _accountManager.CreateRoleAsync(applicationRole, new string[] { });
                    if (!result.Succeeded)
                        throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
                    await SaveNotification("creado role " + name, "roles.manage");
                }
            }
            else {
                ApplicationRole applicationRole = await _accountManager.GetRoleByIdAsync(id);
                applicationRole.Name = name;
                applicationRole.Description = description;
                _context.Entry(applicationRole).CurrentValues.SetValues(applicationRole);
                _context.Entry(applicationRole).State = EntityState.Modified;
                _context.SaveChanges();
                await SaveNotification("actualizar role " + name, "roles.manage");
            }
            return Json(new
            {
                error = ""
            });
        }
        [HttpPost]
        public async Task<JsonResult> deleteRole(
            string id
        )
        {
            ApplicationRole applicationRole = await _accountManager.GetRoleByIdAsync(id);
            await _roleManager.DeleteAsync(applicationRole);
            await SaveNotification("eliminado a role " , "roles.manage");
            return Json(new
            {
                error = ""
            });
        }
            public async Task<JsonResult> assignRolePermission(string roleId, string claim, Boolean chk) {
            if (!HasPermmision("roles.assign")) return null;
            ApplicationRole role=await _roleManager.FindByIdAsync(roleId);
            if (chk)
            {
                await _roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
                await SaveNotification("creado permiso "+claim+" a" + role.Name,"roles.assign");
            }
            else
            {
                await _roleManager.RemoveClaimAsync(role, new Claim(ClaimConstants.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
                await SaveNotification("eliminado el permiso " + claim + " a" + role.Name, "roles.assign");
            }
            return Json(new
            {
                msg="ok"
            });
        }
        public IActionResult Users()
        {
            if (!HasPermmision("users.view")) return Redirect("index");
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public DatatableUser getUsersDataTable(string role)
        {
            if (role.Equals("cliente"))
            {
                if (!HasPermmision("users.cview")) return null;
            }
            else if (role.Equals("inversora"))
            {
                if (!HasPermmision("users.iview")) return null;
            }
            else{
                if (!(HasPermmision("users.view")|| HasPermmision("users.gview"))) return null;
            }
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
            if (role.Equals("cliente"))
            {
                if (!HasPermmision("users.cmanage")) return null;
            }
            else if (role.Equals("inversora"))
            {
                if (!HasPermmision("users.imanage")) return null;
            }
            else
            {
                if (!(HasPermmision("users.manage"))) return null;
            }
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
                await SaveNotification("creado el usuario \"" + user.UserName+"\"", "users.manage");
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

                await SaveNotification("actualizó el usuario \"" + user.UserName + "\"", "users.manage");
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
                if (cuser.Roles.First().Equals("cliente"))
                {
                    if (!HasPermmision("users.cmanage")) return null;
                }
                else if (cuser.Roles.First().Equals("inversora"))
                {
                    if (!HasPermmision("users.imanage")) return null;
                }
                else
                {
                    if (!HasPermmision("users.manage")) return null;
                }
                _context.Remove(cuser);
                _context.SaveChanges();
                await SaveNotification("eliminó el usuario \"" + cuser.UserName + "\"", "users.manage");
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
                if (user.Roles.First().Equals("cliente"))
                {
                    if (!HasPermmision("users.cmanage")) return null;
                }
                else if (user.Roles.First().Equals("inversora"))
                {
                    if (!HasPermmision("users.imanage")) return null;
                }
                else
                {
                    if (!HasPermmision("users.manage")) return null;
                }
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, "tempP@ss123");

                await SaveNotification("restablecer la contraseña de usuario \"" + user.UserName + "\"", "users.manage");
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        
        public IActionResult Clients()
        {
            if (!HasPermmision("users.cview")) return Redirect("index");
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public DatatableClient getClientsDataTable()
        {
            if (!HasPermmision("users.cview")) return null;
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
            if (!HasPermmision("users.iview")) return Redirect("index");
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public DatatableInvestor getInvestorsDataTable()
        {
            if (!HasPermmision("users.iview")) return null;
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
            if (!(HasPermmision("users.cmanage")|| HasPermmision("users.imanage"))) return null;
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
            if (!(HasPermmision("users.cmanage") || HasPermmision("users.imanage"))) return null;
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
        public IActionResult Loanreq(){
            if (!(HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return Redirect("index");
            GlobalVariables();
            var query = _userManager.GetUsersInRoleAsync("cliente").Result;
            ViewBag.Clients = query.Select(u => new ApplicationUserShort { Id=u.Id, UserName=u.UserName }).ToList();
            return View();
        }
        [HttpPost]
        public DatatableLoanRequest getLoanRequestsDataTable()
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationLoanRequest> query = (from a in _context.Set<LoanRequest>()
                     join b in _context.Set<ApplicationUser>()
                     on a.ClientId equals b.Id into g
                     from b in g.DefaultIfEmpty()
                     select new ApplicationLoanRequest { 
                        Id = a.Id,
                        ClientId=a.ClientId,
                        RequestedDate=a.RequestedDate,
                        Amount=a.Amount,
                        InterestingRate=a.InterestingRate,
                        Cycle=a.Cycle,
                        Times=a.Times,
                        Status=a.Status,
                        StatusReason=a.StatusReason,
                        UserName =b.UserName,
                        FriendlyName=b.FriendlyName,
                        AvatarImage=b.AvatarImage,
                        CreatedBy=a.CreatedBy,
                        CreatedDate=a.CreatedDate,
                        CreatedDevice=a.CreatedDevice,
                        UpdatedBy=a.UpdatedBy,
                        UpdatedDate=a.UpdatedDate,
                        UpdatedDevice=a.UpdatedDevice
                     });
            if (!HasPermmision("loan.request")) query = query.Where(u =>
                u.Status != LoanStatus.New &&
                u.Status != LoanStatus.Contactor_Checking &&
                u.Status != LoanStatus.Contactor_Rejected);
            if (!HasPermmision("loan.service")) query = query.Where(u =>
                u.Status != LoanStatus.Service_Mapping &&
                u.Status != LoanStatus.Service_rejected);
            if (!HasPermmision("loan.debug")) query = query.Where(u =>
                u.Status != LoanStatus.Debug_Processing &&
                u.Status != LoanStatus.Debug_rejected&&
                u.Status != LoanStatus.Investor_Piad);
            if (!HasPermmision("loan.collection")) query = query.Where(u =>
                u.Status != LoanStatus.Collection_Processing &&
                u.Status != LoanStatus.Investor_Piad);
            if (!User.IsInRole("administrator")) {
                query = query.Where(u =>
                    u.Status != LoanStatus.Interesting_Process &&
                    u.Status != LoanStatus.Interesting_completed &&
                    u.Status != LoanStatus.Interesting_Incompleted);
            }
            int total = query.Count();
            DatatableLoanRequest res = new DatatableLoanRequest
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
                query = query.Where(u =>
                    u.UserName.IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1 ||
                    u.RequestedDate.ToString().IndexOf(search) > -1 ||
                    u.Amount.ToString().IndexOf(search) > -1 ||
                    u.InterestingRate.ToString().IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1 
                );
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("userName")) query = query.OrderBy(u => u.UserName);
                    else if (sort.field.Equals("friendlyName")) query = query.OrderBy(u => u.FriendlyName);
                    else if (sort.field.Equals("amount")) query = query.OrderBy(u => u.Amount);
                    else if (sort.field.Equals("requestDate")) query = query.OrderBy(u => u.RequestedDate);
                    else if (sort.field.Equals("interestingRate")) query = query.OrderBy(u => u.InterestingRate);
                    else if (sort.field.Equals("cycle")) query = query.OrderBy(u => u.Cycle);
                }
                else
                {
                    if (sort.field.Equals("userName")) query = query.OrderByDescending(u => u.UserName);
                    else if (sort.field.Equals("friendlyName")) query = query.OrderByDescending(u => u.FriendlyName);
                    else if (sort.field.Equals("amount")) query = query.OrderByDescending(u => u.Amount);
                    else if (sort.field.Equals("requestDate")) query = query.OrderByDescending(u => u.RequestedDate);
                    else if (sort.field.Equals("interestingRate")) query = query.OrderByDescending(u => u.InterestingRate);
                    else if (sort.field.Equals("cycle")) query = query.OrderByDescending(u => u.Cycle);
                }
            }
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveLoanRequest(
            long id,
            string clientid,
            DateTime requesteddate,
            double amount,
            double interestingrate,
            LoanCycle cycle,
            int times
        )
        {
            LoanRequest req = new LoanRequest
            {
                ClientId = clientid,
                RequestedDate=requesteddate,
                Amount=amount,
                InterestingRate= interestingrate,
                Cycle=cycle,
                Times=times
            };
            if (!(HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
            if (id==0)
            {
                await _context.LoanRequest.AddAsync(req);
                await _context.SaveChangesAsync();

                await SaveNotification("creó una solicitud de préstamo", "loan.request");
            }
            else
            {
                req.Id = id;
                _context.Entry(req).CurrentValues.SetValues(req);
                _context.Entry(req).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public async Task<ActionResult> DeleteLoanRequest(long id)
        {
            if (!(HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
            try
            {
                LoanRequest req = await _context.LoanRequest.FindAsync(id);
                _context.Remove(req);
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        [HttpPost]
        public DatatableLoanCalculator getLoanCalculatorDataTable(
            double amount, double interestingrate, LoanCycle cycle, DateTime requesteddate, int times
        )
        {
            if (!(HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            double c = 1, t = 1, b = amount, I;
            for (int i = 1; i < times; i++)
            {
                t *= 1 + interestingrate / 100;
                c += t;
            }
            c = amount / c;
            DateTime d = requesteddate;
            List<ApplicationLoanCalcuator> data = new List<ApplicationLoanCalcuator>();
            for (int i = 0; i < times; i++)
            {
                data.Add(new ApplicationLoanCalcuator
                {
                    Date = d,
                    Capital = Math.Round(c, 2),
                    Interest = Math.Round(b * interestingrate / 100, 2),
                    Dues = Math.Round(c + b * interestingrate / 100, 2),
                    Balance = Math.Round(b - c, 2)
                });
                b -= c;
                d = LoanCycleCalculator.NextDate(cycle, d);
                c = c * (1 + interestingrate / 100);
            }
            DatatableLoanCalculator res = new DatatableLoanCalculator
            {
                meta = new DatatablePagination
                {
                    total = times,
                    page = page,
                    perpage = perpage,
                    pages = times / perpage + (times % perpage < 5 ? 1 : 0),
                }
            };
            res.data = data.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public DatatableLoanRequestStatus getLoanRequestStatusDataTable(
            long requestId
        )
        {
            if (!(HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationLoanRequestStatus> query = (
                from a in _context.Set<LoanRequestStatus>().Where(u=>u.LoanRequestId==requestId)
                join b in _context.Set<ApplicationUser>()
                on a.CreatedBy equals b.Id into g
                from b in g.DefaultIfEmpty()
                join c in _context.Set<LoanRequest>()
                on a.LoanRequestId equals c.Id into h
                from c in h.DefaultIfEmpty()
                select new ApplicationLoanRequestStatus
                {
                    Id = a.Id,
                    LoanRequestId=a.LoanRequestId,
                    Status=a.Status,
                    StatusReason=a.StatusReason,
                    ByUserName=b.UserName,
                    ByFriendlyName=b.FriendlyName,
                    ByAvatarImage=b.AvatarImage,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    CreatedDevice = a.CreatedDevice,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                    UpdatedDevice = a.UpdatedDevice
                });
            int total = query.Count();
            DatatableLoanRequestStatus res = new DatatableLoanRequestStatus
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveLoanRequestStatus(
            long requestid,
            LoanStatus status,
            string statusreason
        )
        {
            if (!User.IsInRole("administrator")) {
                if (!HasPermmision("loan.request"))
                    if (status == LoanStatus.New ||
                        status == LoanStatus.Contactor_Checking ||
                        status == LoanStatus.Contactor_Rejected ||
                        status == LoanStatus.Service_Mapping) return null;
                if (!HasPermmision("loan.service"))
                    if (status == LoanStatus.Service_rejected ||
                        status == LoanStatus.Debug_Processing) return null;
                if (!HasPermmision("loan.debug"))
                    if (status == LoanStatus.Debug_rejected ||
                    status == LoanStatus.Collection_Processing) return null;
                if (!HasPermmision("loan.collection"))
                    if (status == LoanStatus.Interesting_completed ||
                    status == LoanStatus.Interesting_Incompleted) return null;
            }
            LoanRequestStatus req = new LoanRequestStatus
            {
                Status = status,
                StatusReason=statusreason,
                LoanRequestId=requestid
            };
            
            await _context.LoanRequestStatus.AddAsync(req);
            await _context.SaveChangesAsync();

            LoanRequest lreq = await _context.LoanRequest.FindAsync(requestid);
            lreq.Status = status;
            lreq.StatusReason = statusreason;
            _context.Entry(lreq).CurrentValues.SetValues(lreq);
            _context.Entry(lreq).State = EntityState.Modified;
            _context.SaveChanges();
            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public async Task<ActionResult> deleteLoanRequestStatus(long id)
        {
            if (!(HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
            try
            {
                LoanRequestStatus req = await _context.LoanRequestStatus.FindAsync(id);
                long reqid = req.LoanRequestId;
                _context.Remove(req);
                _context.SaveChanges();

                LoanRequest lreq = await _context.LoanRequest.FindAsync(reqid);
                var query = _context.LoanRequestStatus.Where(u => u.LoanRequestId == reqid).OrderByDescending(u => u.CreatedDate);
                LoanStatus status = LoanStatus.New;
                string statusreason = "";
                if (query.Count() > 0) {
                    status = query.First().Status;
                    statusreason = query.First().StatusReason;
                }
                lreq.Status = status;
                lreq.StatusReason = statusreason;
                _context.Entry(lreq).CurrentValues.SetValues(lreq);
                _context.Entry(lreq).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }

        [HttpGet]
        public IActionResult Investment()
        {
            if (!(HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
            GlobalVariables();
            var query = _userManager.GetUsersInRoleAsync("inversora").Result;
            ViewBag.Investors = query.Select(u => new ApplicationUserShort { Id = u.Id, UserName = u.UserName }).ToList();
            return View();
        }
        [HttpPost]
        public DatatableInvestment getInvestmentDataTable()
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationInvestment> query = (from a in _context.Set<Investment>()
                                                        join b in _context.Set<ApplicationUser>()
                                                        on a.InvestorId equals b.Id into g
                                                        from b in g.DefaultIfEmpty()
                                                        select new ApplicationInvestment
                                                        {
                                                            Id = a.Id,
                                                            InvestorId = a.InvestorId,
                                                            RequestedDate = a.RequestedDate,
                                                            Amount = a.Amount,
                                                            SavingRate = a.SavingRate,
                                                            Cycle = a.Cycle,
                                                            Times = a.Times,
                                                            Status = a.Status,
                                                            StatusReason = a.StatusReason,
                                                            UserName = b.UserName,
                                                            FriendlyName = b.FriendlyName,
                                                            AvatarImage = b.AvatarImage,
                                                            CreatedBy = a.CreatedBy,
                                                            CreatedDate = a.CreatedDate,
                                                            CreatedDevice = a.CreatedDevice,
                                                            UpdatedBy = a.UpdatedBy,
                                                            UpdatedDate = a.UpdatedDate,
                                                            UpdatedDevice = a.UpdatedDevice
                                                        });
            if (!HasPermmision("investment.service")) query = query.Where(u =>
                u.Status != InvestStatus.Service_Processing &&
                u.Status != InvestStatus.Service_rejected&&
                u.Status != InvestStatus.New);
            if (!HasPermmision("investment.debug")) query = query.Where(u =>
                u.Status != InvestStatus.Debug_Processing &&
                u.Status != InvestStatus.Debug_rejected);
            if (!HasPermmision("investment.collection")) query = query.Where(u =>
                u.Status != InvestStatus.Collection_Processing &&
                u.Status != InvestStatus.Completed_Payment &&
                u.Status != InvestStatus.Created_Milestone);
            if (!User.IsInRole("administrator"))
            {
                query = query.Where(u =>
                    u.Status != InvestStatus.Completed_Investment &&
                    u.Status != InvestStatus.Incompleted_Investment);
            }
            int total = query.Count();
            DatatableInvestment res = new DatatableInvestment
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
                query = query.Where(u =>
                    u.UserName.IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1 ||
                    u.RequestedDate.ToString().IndexOf(search) > -1 ||
                    u.Amount.ToString().IndexOf(search) > -1 ||
                    u.SavingRate.ToString().IndexOf(search) > -1 ||
                    u.FriendlyName.IndexOf(search) > -1
                );
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("userName")) query = query.OrderBy(u => u.UserName);
                    else if (sort.field.Equals("friendlyName")) query = query.OrderBy(u => u.FriendlyName);
                    else if (sort.field.Equals("amount")) query = query.OrderBy(u => u.Amount);
                    else if (sort.field.Equals("requestDate")) query = query.OrderBy(u => u.RequestedDate);
                    else if (sort.field.Equals("savingRate")) query = query.OrderBy(u => u.SavingRate);
                    else if (sort.field.Equals("cycle")) query = query.OrderBy(u => u.Cycle);
                }
                else
                {
                    if (sort.field.Equals("userName")) query = query.OrderByDescending(u => u.UserName);
                    else if (sort.field.Equals("friendlyName")) query = query.OrderByDescending(u => u.FriendlyName);
                    else if (sort.field.Equals("amount")) query = query.OrderByDescending(u => u.Amount);
                    else if (sort.field.Equals("requestDate")) query = query.OrderByDescending(u => u.RequestedDate);
                    else if (sort.field.Equals("savingRate")) query = query.OrderByDescending(u => u.SavingRate);
                    else if (sort.field.Equals("cycle")) query = query.OrderByDescending(u => u.Cycle);
                }
            }
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveInvestment(
            long id,
            string investorid,
            DateTime requesteddate,
            double amount,
            double savingrate,
            LoanCycle cycle,
            int times
        )
        {
            if (!(HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
            Investment req = new Investment
            {
                InvestorId = investorid,
                RequestedDate = requesteddate,
                Amount = amount,
                SavingRate = savingrate,
                Cycle = cycle,
                Times = times
            };
            if (id == 0)
            {
                await _context.Investment.AddAsync(req);
                await _context.SaveChangesAsync();
                await SaveNotification("creado una inversión", "loan.request");
            }
            else
            {
                req.Id = id;
                _context.Entry(req).CurrentValues.SetValues(req);
                _context.Entry(req).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public async Task<ActionResult> DeleteInvestment(long id)
        {
            if (!(HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
            try
            {
                Investment req = await _context.Investment.FindAsync(id);
                _context.Remove(req);
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        [HttpPost]
        public DatatableInvestmentStatus getInvestmentStatusDataTable(
            long investmentId
        )
        {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationInvestmentStatus> query = (
                from a in _context.Set<InvestmentStatus>().Where(u => u.InvestmenttId == investmentId)
                join b in _context.Set<ApplicationUser>()
                on a.CreatedBy equals b.Id into g
                from b in g.DefaultIfEmpty()
                join c in _context.Set<LoanRequest>()
                on a.InvestmenttId equals c.Id into h
                from c in h.DefaultIfEmpty()
                select new ApplicationInvestmentStatus
                {
                    Id = a.Id,
                    InvestmenttId = a.InvestmenttId,
                    Status = a.Status,
                    StatusReason = a.StatusReason,
                    ByUserName = b.UserName,
                    ByFriendlyName = b.FriendlyName,
                    ByAvatarImage = b.AvatarImage,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    CreatedDevice = a.CreatedDevice,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                    UpdatedDevice = a.UpdatedDevice
                });
            int total = query.Count();
            DatatableInvestmentStatus res = new DatatableInvestmentStatus
            {
                meta = new DatatablePagination
                {
                    total = total,
                    page = page,
                    perpage = perpage,
                    pages = total / perpage + (total % perpage < 5 ? 1 : 0),
                }
            };
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveInvestmentStatus(
            long investmentid,
            InvestStatus status,
            string statusreason
        )
        {
            if (!User.IsInRole("administrator"))
            {
                if (!HasPermmision("investment.service"))
                    if (status == InvestStatus.Service_Processing ||
                        status == InvestStatus.Service_rejected ||
                        status == InvestStatus.Debug_Processing) return null;
                if (!HasPermmision("investment.debug"))
                    if (status == InvestStatus.Debug_rejected ||
                    status == InvestStatus.Collection_Processing) return null;
                if (!HasPermmision("investment.collection"))
                    if (status == InvestStatus.Incompleted_Investment ||
                        status == InvestStatus.Collection_Error ||
                    status == InvestStatus.Completed_Investment) return null;
            }
            InvestmentStatus req = new InvestmentStatus
            {
                Status = status,
                StatusReason = statusreason,
                InvestmenttId = investmentid
            };

            await _context.InvestmentStatus.AddAsync(req);
            await _context.SaveChangesAsync();

            Investment lreq = await _context.Investment.FindAsync(investmentid);
            lreq.Status = status;
            lreq.StatusReason = statusreason;
            _context.Entry(lreq).CurrentValues.SetValues(lreq);
            _context.Entry(lreq).State = EntityState.Modified;
            _context.SaveChanges();
            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public async Task<ActionResult> deleteInvestmentStatus(long id)
        {
            if (!(HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
            try
            {
                InvestmentStatus req = await _context.InvestmentStatus.FindAsync(id);
                long reqid = req.InvestmenttId;
                _context.Remove(req);
                _context.SaveChanges();

                Investment lreq = await _context.Investment.FindAsync(reqid);
                var query = _context.InvestmentStatus.Where(u => u.InvestmenttId == reqid).OrderByDescending(u => u.CreatedDate);
                InvestStatus status = InvestStatus.New;
                string statusreason = "";
                if (query.Count() > 0)
                {
                    status = query.First().Status;
                    statusreason = query.First().StatusReason;
                }
                lreq.Status = status;
                lreq.StatusReason = statusreason;
                _context.Entry(lreq).CurrentValues.SetValues(lreq);
                _context.Entry(lreq).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }
        [HttpGet]
        public IActionResult Transhistory()
        {
            GlobalVariables();
            return View();
        }
        public DatatableTransactionHistory getTransactionHistoryDataTable() {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationTransactionHistory> query = (
                from a in _context.Set<TransactionHistory>()
                join b in _context.Set<AccountPayment>()
                on a.FromPaymentId equals b.Id into g
                from b in g.DefaultIfEmpty()
                join c in _context.Set<ApplicationUser>()
                on b.UserId equals c.Id into h
                from c in h.DefaultIfEmpty()
                join d in _context.Set<AccountPayment>()
                on a.ToPaymentId equals d.Id into i
                from d in i.DefaultIfEmpty()
                join e in _context.Set<ApplicationUser>()
                on d.UserId equals e.Id into j
                from e in j.DefaultIfEmpty()
                select new ApplicationTransactionHistory
                {
                    Id = a.Id,
                    Amount=a.Amount,
                    Fee=a.Fee,
                    FromPaymentId=a.FromPaymentId,
                    ToPaymentId=a.ToPaymentId,
                    FromUserName=c.UserName,
                    FromFriendlyName=c.FriendlyName,
                    FromAvatarImage=c.AvatarImage,
                    ToUserName=e.UserName,
                    ToFriendlyName=e.FriendlyName,
                    ToAvatarImage=e.AvatarImage,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    CreatedDevice = a.CreatedDevice,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedDate = a.UpdatedDate,
                    UpdatedDevice = a.UpdatedDevice
                });
            int total = query.Count();
            DatatableTransactionHistory res = new DatatableTransactionHistory
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
                //query = (IList<ApplicationUser>)query.Where(u =>
                //);
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    
                }
                else
                {
         
                }
            }
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
    }
}