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

            ViewBag.client_count = 0;
            ViewBag.investor_count = 0;
            ViewBag.inneraluser_count = 0;
            ViewBag.UserRoles = "";
            foreach (ApplicationRole role in _roleManager.Roles.OrderBy(u=>u.CreatedDate).ToList()) {
                if (role.Name.Equals("cliente")) ViewBag.client_count = _userManager.GetUsersInRoleAsync("cliente").Result.Count();
                else if (role.Name.Equals("inversora")) ViewBag.investor_count = _userManager.GetUsersInRoleAsync("inversora").Result.Count();
                else
                {
                    ViewBag.inneraluser_count += _userManager.GetUsersInRoleAsync(role.Name).Result.Count();
                    ViewBag.UserRoles += (ViewBag.UserRoles.Equals("")?"":",") + role.Name;
                }
            }
            
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

            ViewBag.Company = _context.Company.OrderBy(u => u.Name).ToList();
            ViewBag.Province = _context.Province.OrderBy(u => u.Id).ToList();
            ViewBag.Nationality = _context.Nationality.OrderBy(u => u.Id).ToList();
        }
        [HttpGet]
        public IActionResult Index()
        {
            GlobalVariables();
            //var userid = HttpContext.Session.GetString("loan.droid.app.loggedin.userid");
            //if(userid==null) return RedirectToAction(nameof(AccountController.Login), "Account");
            //var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //ViewBag.user = await _userManager.FindByIdAsync(userid);
            return View();
        }
        [HttpGet]
        public IActionResult Roles()
        {
            if (!HasPermmision("roles.view")) return Redirect("index");
            GlobalVariables(); 
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> getUserRowAsync(string id) {
            ApplicationUser user= await _accountManager.GetUserByIdAsync(id);
            var q1 = _context.AccountPayment.Where(u => u.UserId.Equals(id) && u.Type == 0).OrderByDescending(u => u.CreatedDate);
            var q2 = _context.AccountPayment.Where(u => u.UserId.Equals(id) && u.Type == 1).OrderByDescending(u => u.CreatedDate);
            var q3 = _context.AccountPayment.Where(u => u.UserId.Equals(id) && u.Type == 2).OrderByDescending(u => u.CreatedDate);
            AccountPayment acc = new AccountPayment
            {
                Id = 0,
                UserId = id,
                GatewayName = "",
                GatewayUrl = "",
                GatewayEmail = user.Email,
                GatewayUserName = user.UserName,
                GatewayPassword = "",
                CardFirstName = user.FirstName,
                CardLastName = user.LastName,
                CardNumber = "",
                CardExpirationDate = "",
                CardAddress1 = "",
                CardAddress2 = "",
                BankCountry = "",
                BankAccountHolder = "",
                BankName = "",
                BankStreet = "",
                BankCity = "",
                BankRegion = "",
                BankCurrency = "",
                BankRoutingNumber = "",
                BankSwiftBicNumber = "",
                BankIBANNumber = "",
                Type = 0
            };
            var Payment01 = q1.Count() > 0 ? q1.First() : acc;
            acc.Type = 1;
            var Payment02 = q2.Count() > 0 ? q2.First() : acc;
            acc.Type = 2;
            var Payment03 = q3.Count() > 0 ? q3.First() : acc;
            return Json(new
            {
                user = user,
                payment01 = Payment01,
                payment02 = Payment02,
                payment03 = Payment03,
            });
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
            query = query.Where(u => !u.Name.Equals("administrator") && !u.Name.Equals("cliente") && !u.Name.Equals("inversora"));
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


        [HttpGet]
        public IActionResult Company()
        {
            //if (!HasPermmision("roles.view")) return Redirect("index");
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public DatatableCompany getCompanyDataTable()
        {
            if (!HasPermmision("users.cmanage")) return null;
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<Company> query = _context.Set<Company>();
            int total = query.Count();
            DatatableCompany res = new DatatableCompany
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
                    u.Name.IndexOf(search) > -1 ||
                    u.Purpose.IndexOf(search) > -1 ||
                    u.Direction.IndexOf(search) > -1 ||
                    u.CPhone.IndexOf(search) > -1 ||
                    u.Url.IndexOf(search) > -1
                );
            if (
                total > 0 && sort.field != null && sort.sort != null && !sort.Equals("")
            )
            {
                if (sort.sort.Equals("asc"))
                {
                    if (sort.field.Equals("name")) query = query.OrderBy(u => u.Name);
                    else if (sort.field.Equals("purpose")) query = query.OrderBy(u => u.Purpose);
                    else if (sort.field.Equals("direction")) query = query.OrderBy(u => u.Direction);
                    else if (sort.field.Equals("cPhone")) query = query.OrderBy(u => u.CPhone);
                    else if (sort.field.Equals("url")) query = query.OrderBy(u => u.Url);
                }
                else
                {
                    if (sort.field.Equals("name")) query = query.OrderByDescending(u => u.Name);
                    else if (sort.field.Equals("purpose")) query = query.OrderByDescending(u => u.Purpose);
                    else if (sort.field.Equals("direction")) query = query.OrderByDescending(u => u.Direction);
                    else if (sort.field.Equals("cPhone")) query = query.OrderByDescending(u => u.CPhone);
                    else if (sort.field.Equals("url")) query = query.OrderByDescending(u => u.Url);
                }
            }
            res.data = query.Skip((page - 1) * perpage)
                          .Take(perpage)
                          .ToList();
            return res;
        }
        [HttpPost]
        public async Task<JsonResult> saveCompany(
            int id,
            string name,
            string purpose,
            string direction,
            string phone,
            string url
        )
        {
            Company req = new Company
            {
                Name = name,
                Purpose = purpose,
                Direction = direction,
                Phone = phone,
                Url = url
            };
            if (!HasPermmision("users.cmanage")) return null;
            if (id == 0)
            {
                await _context.Company.AddAsync(req);
                await _context.SaveChangesAsync();

                await SaveNotification("creó un '"+name+"' de la empresa", "users.cmanage");
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
        public async Task<ActionResult> DeleteCompany(int id)
        {
            if (!HasPermmision("users.cmanage")) return null;
            try
            {
                Company req = await _context.Company.FindAsync(id);
                _context.Remove(req);
                _context.SaveChanges();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return Json(new
            {
                error = ""
            });
        }

        [HttpGet]
        public IActionResult Loanrate()
        {
            //if (!HasPermmision("roles.view")) return Redirect("index");
            ViewBag.Rates = new List<LoanCycleModel>();
            foreach (int i in Enum.GetValues(typeof(LoanCycle))) {
                var name = Enum.GetName(typeof(LoanCycle), i);
                double val = 0;
                if (_context.Option.Where(u => u.Key.Equals("LOAN_RATE_" + name)).Count() > 0)
                    val = double.Parse(_context.Option.Where(u => u.Key.Equals("LOAN_RATE_" + name)).First().Value, new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." });
                ViewBag.Rates.Add(new LoanCycleModel {
                    LoanCycle = (LoanCycle)Enum.GetValues(typeof(LoanCycle)).GetValue(i),
                    Rate=val
                });
            }
            ViewBag.SRates = new List<LoanCycleModel>();
            foreach (int i in Enum.GetValues(typeof(LoanCycle)))
            {
                var name = Enum.GetName(typeof(LoanCycle), i);
                double val = 0;
                if (_context.Option.Where(u => u.Key.Equals("SAVING_RATE_" + name)).Count() > 0)
                    val = double.Parse(_context.Option.Where(u => u.Key.Equals("SAVING_RATE_" + name)).First().Value, new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." });
                ViewBag.SRates.Add(new LoanCycleModel
                {
                    LoanCycle = (LoanCycle)Enum.GetValues(typeof(LoanCycle)).GetValue(i),
                    Rate = val
                });
            }
            GlobalVariables();
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> saveloanRate() {
            //if (!HasPermmision("users.cmanage")) return null;
            foreach (int i in Enum.GetValues(typeof(LoanCycle))) {
                var name = Enum.GetName(typeof(LoanCycle), i);
                var sss = Request.Form[name].FirstOrDefault();
                double val = double.Parse(sss, new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." });
                Option req = new Option
                {
                    Key = "LOAN_RATE_"+name,
                    Value = val.ToString(),
                    Kind = "",
                    Description=""
                };
                if (_context.Option.Where(u=>u.Key.Equals("LOAN_RATE_" + name)).Count()==0)
                {
                    await _context.Option.AddAsync(req);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Entry(req).CurrentValues.SetValues(req);
                    _context.Entry(req).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                sss = Request.Form["saving_"+name].FirstOrDefault();
                val = double.Parse(sss, new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." });
                req = new Option
                {
                    Key = "SAVING_RATE_" + name,
                    Value = val.ToString(),
                    Kind = "",
                    Description = ""
                };
                if (_context.Option.Where(u => u.Key.Equals("SAVING_RATE_" + name)).Count() == 0)
                {
                    await _context.Option.AddAsync(req);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Entry(req).CurrentValues.SetValues(req);
                    _context.Entry(req).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                await SaveNotification("Establezca la tasa de préstamo "+name+" en "+val, "users.cmanage");
            }
            return Json(new
            {
                error = ""
            });
        }

        [HttpGet]
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
            var query= _userManager.GetUsersInRoleAsync(role).Result; 
            /*
            if(role==null||role.Equals("")||role.Equals("administrator"))query = _userManager.GetUsersInRoleAsync("administrator").Result;
            else if (role.Equals("representante")) query = _userManager.GetUsersInRoleAsync("representante").Result;
            else if (role.Equals("contactor")) query = _userManager.GetUsersInRoleAsync("contactor").Result;
            else if (role.Equals("servicio")) query = _userManager.GetUsersInRoleAsync("servicio").Result;
            else if (role.Equals("depuracion")) query = _userManager.GetUsersInRoleAsync("depuracion").Result;
            else if (role.Equals("coleccion")) query = _userManager.GetUsersInRoleAsync("coleccion").Result;
            */
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
            string username,string email,string passport,string firstname,string lastname,Gender sex,
            Marital marital,string phonenumber,string otherphone,DateTime birth,string numdependant,
            Residence residence,string residenceperiod,int company,string officenumber,string address,
            int nationality,int province,string mothername,string motherphone,
            string fathername,string fatherphone,string avatarimage, string role, string department
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
                Email = email,
                Passport=passport,
                FirstName = firstname,
                LastName = lastname == null ? "" : lastname,
                Sex=sex,
                Marital=marital,
                PhoneNumber = phonenumber,
                OtherPhone = otherphone == null ? "" : otherphone,
                Birth=birth,
                NumDependant=numdependant,
                Residence=residence,
                ResidencePeriod=residenceperiod == null ? "" : residenceperiod,
                CompanyId=company,
                OfficeNumber = officenumber,
                JobTitle = department == null ? "" : department,
                Address = address == null ? "" : address,
                NationalityId=nationality,
                ProvinceId=province,
                MotherName=mothername,
                MotherPhone=motherphone==null?"": motherphone,
                FatherName=fathername,
                FatherPhone=fatherphone==null?"": fatherphone,
                AvatarImage = CommonUtil.DecodeUrlBase64(avatarimage)//System.Text.Encoding.Unicode.GetBytes(avatarimage)
            };
            if (id.Equals(""))
            {
                string password = "tempP@ss123";
                var result = await _accountManager.CreateUserAsync(user, new string[] {role }, password);
                await SaveNotification("creado el usuario \"" + user.UserName+"\"", "users.manage");
            }
            else {
                ApplicationUser cuser = await _accountManager.GetUserByIdAsync(id);
                cuser.UserName = user.UserName;
                cuser.Email = user.Email;
                cuser.Passport = user.Passport;
                cuser.FirstName = user.FirstName;
                cuser.LastName = user.LastName;
                cuser.Sex = user.Sex;
                cuser.Marital = user.Marital;
                cuser.PhoneNumber = user.PhoneNumber;
                cuser.OtherPhone = user.OtherPhone;
                cuser.Birth = user.Birth;
                cuser.NumDependant = user.NumDependant;
                cuser.Residence = user.Residence;
                cuser.ResidencePeriod = user.ResidencePeriod;
                cuser.CompanyId = user.CompanyId;
                cuser.OfficeNumber = user.OfficeNumber;
                cuser.JobTitle = user.JobTitle;
                cuser.Address = user.Address;
                cuser.NationalityId = user.NationalityId;
                cuser.ProvinceId = user.ProvinceId;
                cuser.MotherName = user.MotherName;
                cuser.MotherPhone = user.MotherPhone;
                cuser.FatherName = user.FatherName;
                cuser.FatherPhone = user.FatherPhone;
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
            if (User.IsInRole("cliente"))
            {
                query = query.Where(u =>
                    u.ClientId.Equals(_userManager.GetUserId(User)));
            }
            else if (!User.IsInRole("administrator"))
            {
                if (!HasPermmision("loan.representante")&&!HasPermmision("loan.request")) query = query.Where(u =>
                     u.Status != LoanStatus.New);
                if (!HasPermmision("loan.representante")) query = query.Where(u =>
                    u.Status != LoanStatus.Representante_Processing &&
                    u.Status != LoanStatus.Representante_Rejected);
                if (!HasPermmision("loan.request")) query = query.Where(u =>
                    u.Status != LoanStatus.Contactor_Checking &&
                    u.Status != LoanStatus.Contactor_Rejected);
                if (!HasPermmision("loan.service")) query = query.Where(u =>
                    u.Status != LoanStatus.Service_Processing &&
                    u.Status != LoanStatus.Service_Rejected
                    );
                if (!HasPermmision("loan.debug")) query = query.Where(u =>
                    u.Status != LoanStatus.Debug_Processing &&
                    u.Status != LoanStatus.Debug_Rejected
                    );
                if (!HasPermmision("loan.collection")) query = query.Where(u =>
                    u.Status != LoanStatus.Collection_Processing &&
                    u.Status != LoanStatus.Investor_Piad &&
                    u.Status != LoanStatus.Interesting_Process&&
                    u.Status != LoanStatus.Interesting_Completed &&
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
            int times,
            string description
        )
        {
            LoanRequest req = new LoanRequest
            {
                ClientId = clientid,
                RequestedDate=requesteddate,
                Amount=amount,
                InterestingRate= interestingrate,
                Cycle=cycle,
                Times=times,
                Description=description
            };            
            if (!(User.IsInRole("cliente")||HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
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
        public DatatableMessage getMessageDataTable(string fromUserId="", string toUserId="", int type=0) {
            int page = Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };

            IQueryable<ApplicationMessage> query;
            IQueryable<Message> messages = _context.Set<Message>();
            if(fromUserId!=null&&!fromUserId.Equals(""))
                messages= messages.Where(u => u.FromUserId.Equals(fromUserId));
            if (toUserId != null && !toUserId.Equals(""))
                messages = messages.Where(u => u.ToUserId.Equals(toUserId));
            query = (
            from a in messages.OrderBy(u=>u.CreatedDate)
            join b in _context.Set<ApplicationUser>()
            on a.FromUserId equals b.Id into g
            from b in g.DefaultIfEmpty()
            join c in _context.Set<ApplicationUser>()
            on a.ToUserId equals c.Id into h
            from c in h.DefaultIfEmpty()
            select new ApplicationMessage
            {
                Id = a.Id,
                FromUserId=a.FromUserId,
                ToUserId=a.ToUserId,
                Question=a.Question,
                QuestionAttach=a.QuestionAttach,
                Answer=a.Answer,
                AnswerAttach=a.AnswerAttach,
                FromUserName=b.UserName,
                FromFriendlyName=b.FriendlyName,
                FromAvatarImage=b.AvatarImage,
                ToUserName=c.UserName,
                ToFriendlyName=c.FriendlyName,
                ToAvatarImage=c.AvatarImage,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                CreatedDevice = a.CreatedDevice,
                UpdatedBy = a.UpdatedBy,
                UpdatedDate = a.UpdatedDate,
                UpdatedDevice = a.UpdatedDevice
            });            
            int total = query.Count();
            DatatableMessage res = new DatatableMessage
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
        public async Task<JsonResult> saveMessage(
            long id=0,
            string fromUserId="",
            string toUserId="",
            string question="",
            string answer=""
        ) {
            if (id == 0)
            {
                if (fromUserId.Equals("")) fromUserId = _context.CurrentUserId;
                if (toUserId.Equals("")) toUserId = _context.CurrentUserId;
                Message req = new Message { 
                    FromUserId=fromUserId,
                    ToUserId=toUserId,
                    Question=question,
                    Answer=answer
                };
                await _context.Message.AddAsync(req);
                await _context.SaveChangesAsync();

                string fromName = _accountManager.GetUserByIdAsync(req.FromUserId).Result.FriendlyName;
                string toName = _accountManager.GetUserByIdAsync(req.ToUserId).Result.FriendlyName;
                await SaveNotification("La usuario "+fromName+" envió un mensaje a la usuario "+toName, "users.cview");
            }
            else
            {
                Message req = _context.Message.Find(id);
                if (!fromUserId.Equals("")) req.FromUserId = fromUserId;
                if (!toUserId.Equals("")) req.ToUserId = toUserId;
                if (!question.Equals("")) req.Question = question;
                if (!answer.Equals("")) req.Answer = answer;
                _context.Entry(req).CurrentValues.SetValues(req);
                _context.Entry(req).State = EntityState.Modified;
                _context.SaveChanges();

                string fromName = _accountManager.GetUserByIdAsync(req.FromUserId).Result.FriendlyName;
                string toName = _accountManager.GetUserByIdAsync(req.ToUserId).Result.FriendlyName;
                await SaveNotification("La usuario " + fromName + " respondió un mensaje a la usuario " + toName, "users.cview");
            }
            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public DatatableLoanCalculator getLoanCalculatorDataTable(
            double amount, double interestingrate, LoanCycle cycle, DateTime requesteddate, int times,int loan_id=0
        )
        {
            if (requesteddate == null|| requesteddate.Year<1000) requesteddate = DateTime.Now;
            if (!(User.IsInRole("inversora") || User.IsInRole("cliente") || HasPermmision("loan.request") || HasPermmision("loan.service") || HasPermmision("loan.debug") || HasPermmision("loan.collection"))) return null;
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
            DateTime d = LoanCycleCalculator.NextDate(cycle, requesteddate);//requesteddate;
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
            if (loan_id > 0) {
                for (int i = 0,j=0; i < res.data.Count(); i++) {
                    if (j == 0)
                    {
                        var q = _context.LoanInterestPayment.Where(u => u.LoanRequestId == loan_id && u.TimesNum == i);
                        if (q.Count() > 0)
                        {
                            res.data[i].Status = 2;
                            res.data[i].PaidDate = q.First().CreatedDate;
                        }
                        else
                        {
                            res.data[i].Status = 0;
                            j = 1;
                        }
                    }
                    else {
                        res.data[i].Status = 1;
                    }
                }
            }
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
                if (!HasPermmision("loan.representante"))
                    if (
                        status == LoanStatus.Representante_Rejected) return null;
                if (!HasPermmision("loan.request"))
                    if (
                        status == LoanStatus.Contactor_Rejected) return null;
                if (!HasPermmision("loan.service"))
                    if (
                        status == LoanStatus.Service_Rejected) return null;
                if (!HasPermmision("loan.debug"))
                    if (status == LoanStatus.Debug_Rejected) return null;
                if (!HasPermmision("loan.collection"))
                    if (
                    status == LoanStatus.Investor_Piad ||
                    status == LoanStatus.Interesting_Process ||
                    status == LoanStatus.Interesting_Completed ||
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
            if (User.IsInRole("inversora"))
            {
                query = query.Where(u =>
                         u.InvestorId.Equals(_context.CurrentUserId));
            }
            else
            {
                if (!User.IsInRole("administrator"))
                {
                    if (!HasPermmision("investment.representante") && !HasPermmision("investment.request")) query = query.Where(u =>
                         u.Status != InvestStatus.New);
                    if (!HasPermmision("investment.representante")) query = query.Where(u =>
                        u.Status != InvestStatus.Representante_Processing &&
                        u.Status != InvestStatus.Representante_Rejected);
                    if (!HasPermmision("investment.request")) query = query.Where(u =>
                        u.Status != InvestStatus.Contactor_Checking &&
                        u.Status != InvestStatus.Contactor_Rejected);
                    if (!HasPermmision("investment.service")) query = query.Where(u =>
                        u.Status != InvestStatus.Service_Processing &&
                        u.Status != InvestStatus.Service_Rejected);
                    if (!HasPermmision("investment.debug")) query = query.Where(u =>
                        u.Status != InvestStatus.Debug_Processing &&
                        u.Status != InvestStatus.Debug_Rejected);
                    if (!HasPermmision("investment.collection")) query = query.Where(u =>
                        u.Status != InvestStatus.Collection_Processing &&
                        u.Status != InvestStatus.Created_Milestone &&
                        u.Status != InvestStatus.Completed_Payment &&
                        u.Status != InvestStatus.Saving_Process &&
                        u.Status != InvestStatus.Completed_Investment &&
                        u.Status != InvestStatus.Incompleted_Investment);
                }
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
        public DatatableMilestone getMilestoneDataTable(long investmentId=0,string investorId="")
        {
            if (investorId.Equals("")) investorId = _context.CurrentUserId;
            int page = Request.Form.Keys.Where(u => u.Equals("pagination[page]")).Count() == 0||Request.Form["pagination[page]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[page]"].FirstOrDefault());
            int perpage = Request.Form.Keys.Where(u=>u.Equals("pagination[perpage]")).Count()==0|| Request.Form["pagination[perpage]"].FirstOrDefault() == null ? 0 : int.Parse(Request.Form["pagination[perpage]"].FirstOrDefault());
            string search = Request.Form.Keys.Where(u => u.Equals("pagination[generalSearch]")).Count() == 0 || Request.Form["query[generalSearch]"].FirstOrDefault() == null ? "" : Request.Form["query[generalSearch]"].FirstOrDefault();
            DatatableSort sort = new DatatableSort
            {
                field = Request.Form["sort[field]"].FirstOrDefault(),
                sort = Request.Form["sort[sort]"].FirstOrDefault()
            };
            IQueryable<ApplicationMilestone> query = (from a in _context.Set<InvestmentStatus>()
                                                      join b in _context.Set<TransactionHistory>()
                                                      on a.TransactionId equals b.Id into g
                                                      from b in g.DefaultIfEmpty()
                                                      join c in _context.Set<Investment>()
                                                       on a.InvestmentId equals c.Id into h
                                                      from c in h.DefaultIfEmpty()
                                                      select new ApplicationMilestone
                                                       {
                                                           InvestmentId = a.InvestmentId,
                                                           InvestorId = c.InvestorId,
                                                           Amount = b.Amount,
                                                           Description = a.StatusReason,
                                                           CreatedDate=b.CreatedDate
                                                       });
            int total = query.Count();
            DatatableMilestone res = new DatatableMilestone
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
        public async Task<JsonResult> saveMilestone(
            DateTime requestedDate,
            long investmentId = 0,
            string investorId = "",
            double amount = 0,
            string description = "")
        {
            if (requestedDate.Year < 1000) requestedDate = DateTime.Now;
            if (investorId.Equals("")) investorId = _context.CurrentUserId;

            TransactionHistory trans = new TransactionHistory
            {
                FromPaymentId = 0,
                ToPaymentId = 0,
                Amount = amount,
                Fee = 0
            };
            await _context.TransactionHistory.AddAsync(trans);
            await _context.SaveChangesAsync();

            InvestmentStatus req = new InvestmentStatus
            {
                Status = InvestStatus.Created_Milestone,
                StatusReason = description,
                InvestmentId = investmentId,
                Paid=amount,
                TransactionId=trans.Id
            };

            await _context.InvestmentStatus.AddAsync(req);
            await _context.SaveChangesAsync();

            Investment invest = _context.Investment.Find(investmentId);
            invest.Status = InvestStatus.Created_Milestone;
            invest.StatusReason = description;
            _context.Entry(invest).CurrentValues.SetValues(invest);
            _context.Entry(invest).State = EntityState.Modified;
            _context.SaveChanges();

            return Json(new
            {
                msg = "ok"
            });
        }
        [HttpPost]
        public async Task<JsonResult> saveInvestment(
            long id,
            DateTime requesteddate,
            string investorid = "",
            double amount=0,
            double savingrate=0,
            LoanCycle cycle=0,
            int times=1,
            long loanId=0
        )
        {
            if (requesteddate.Year < 1000) requesteddate = DateTime.Now;
            if (investorid.Equals("")) investorid = _context.CurrentUserId;
            if (!(User.IsInRole("inversora")|| HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
            Investment req = new Investment
            {
                InvestorId = investorid,
                RequestedDate = requesteddate,
                Amount = amount,
                SavingRate = savingrate,
                Cycle = cycle,
                Times = times,
                LoanId=loanId
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
            if (!(User.IsInRole("inversora") || HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
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
                from a in _context.Set<InvestmentStatus>().Where(u => u.InvestmentId == investmentId)
                join b in _context.Set<ApplicationUser>()
                on a.CreatedBy equals b.Id into g
                from b in g.DefaultIfEmpty()
                join c in _context.Set<LoanRequest>()
                on a.InvestmentId equals c.Id into h
                from c in h.DefaultIfEmpty()
                select new ApplicationInvestmentStatus
                {
                    Id = a.Id,
                    InvestmentId = a.InvestmentId,
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
                if (!HasPermmision("investment.representante"))
                    if (status == InvestStatus.Representante_Rejected) return null;
                if (!HasPermmision("investment.request"))
                    if (status == InvestStatus.Contactor_Rejected) return null;
                if (!HasPermmision("investment.service"))
                    if (status == InvestStatus.Service_Rejected) return null;
                if (!HasPermmision("investment.debug"))
                    if (status == InvestStatus.Debug_Rejected) return null;
                if (!HasPermmision("investment.collection"))
                    if (status == InvestStatus.Incompleted_Investment ||
                        status == InvestStatus.Completed_Payment ||
                        status == InvestStatus.Created_Milestone ||
                        status == InvestStatus.Saving_Process ||
                    status == InvestStatus.Completed_Investment) return null;
            }
            InvestmentStatus req = new InvestmentStatus
            {
                Status = status,
                StatusReason = statusreason,
                InvestmentId = investmentid
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
            if (!(User.IsInRole("inversora") || HasPermmision("investment.service") || HasPermmision("investment.debug") || HasPermmision("investment.collection"))) return null;
            try
            {
                InvestmentStatus req = await _context.InvestmentStatus.FindAsync(id);
                long reqid = req.InvestmentId;
                _context.Remove(req);
                _context.SaveChanges();

                Investment lreq = await _context.Investment.FindAsync(reqid);
                var query = _context.InvestmentStatus.Where(u => u.InvestmentId == reqid).OrderByDescending(u => u.CreatedDate);
                InvestStatus status = InvestStatus.Debug_Processing;
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
        [HttpPost]
        public async Task<JsonResult> saveLoanInterestPayment(
            long LoanRequestId,
            double Capital,
            double Interest,
            double Balabnce,
            int TimesNum
        )
        {
            var userId = _context.CurrentUserId;
            var username = _userManager.GetUserName(User);
            var q1 = _context.AccountPayment.Where(u => u.UserId.Equals(userId)).OrderByDescending(u => u.CreatedDate);
            if (q1.Count() == 0) return null;
            LoanInterestPayment req = new LoanInterestPayment
            {
                LoanRequestId = LoanRequestId,
                Capital = Capital,
                Interest = Interest,
                Balabnce = Balabnce,
                TimesNum = TimesNum,
                AccountPaymentId = q1.First().Id
            };
            if (!User.IsInRole("cliente")) return null;
            await _context.LoanInterestPayment.AddAsync(req);
            await _context.SaveChangesAsync();
            await SaveNotification("Saldo pagado por " + username + " del cliente " + Balabnce + "$", "loan.collection");
            await SaveNotification("Saldo pagado por " + username + " del cliente " + Balabnce + "$", "loan.debug");
            await SaveNotification("Saldo pagado por " + username + " del cliente " + Balabnce + "$", "users.cmanage");
            return Json(new
            {
                msg = "ok"
            });
        }
    }
}