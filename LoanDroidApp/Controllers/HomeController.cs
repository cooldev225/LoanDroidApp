using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBSetup;
using DBSetup.Interfaces;
using LoanDroidApp;
using LoanDroidApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.data;

namespace App.Controllers
{
    //[Authorize(Roles = "client,investor")]
    //[Route("[controller]/[action]")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountManager _accountManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager, IAccountManager accountManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _accountManager = accountManager;
            ViewBag.Layout = "_Layout";
        }
        public void GlobalVariables()
        {
            ViewData["Title"] = "";
            ViewBag.client_count = _userManager.GetUsersInRoleAsync("cliente").Result.Count();
            ViewBag.investor_count = _userManager.GetUsersInRoleAsync("inversora").Result.Count();
        }
        public async Task<int> GetProfileStep() {
            var userId = _context.CurrentUserId;
            if (userId == null || userId.Equals("")) return 0;
            var q1 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 0).OrderByDescending(u => u.CreatedDate);
            var q2 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 1).OrderByDescending(u => u.CreatedDate);
            var q3 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 2).OrderByDescending(u => u.CreatedDate);
            int step = 0;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (user.FirstName != null && !user.FirstName.Equals(""))
                if (user.LastName != null && !user.LastName.Equals(""))
                    if (user.Passport != null && !user.Passport.Equals(""))
                        if (user.Sex != Gender.None)
                            if (user.PhoneNumber != null && !user.PhoneNumber.Equals(""))
                                if (user.NumDependant != null && !user.NumDependant.Equals(""))
                                    if (user.MotherName != null && !user.MotherName.Equals(""))
                                        if (user.FatherName != null && !user.FatherName.Equals(""))
                                            step = 1;
            if (step > 0 && (q1.Count() > 0 || q2.Count() > 0 || q3.Count() > 0))
            {
                step = 2;
            }
            return step;
        }
        public List<LoanCycleModel> GetLoanRates()
        {
            List<LoanCycleModel> Rates = new List<LoanCycleModel>();
            foreach (int i in Enum.GetValues(typeof(LoanCycle)))
            {
                var name = Enum.GetName(typeof(LoanCycle), i);
                double val = 0;
                if (_context.Option.Where(u => u.Key.Equals("LOAN_RATE_" + name)).Count() > 0)
                    val = double.Parse(_context.Option.Where(u => u.Key.Equals("LOAN_RATE_" + name)).First().Value);
                Rates.Add(new LoanCycleModel
                {
                    LoanCycle = (LoanCycle)Enum.GetValues(typeof(LoanCycle)).GetValue(i),
                    Rate = val
                });
            }
            return Rates;
        }
        public List<LoanCycleModel> GetSavingRates()
        {
            List<LoanCycleModel> Rates = new List<LoanCycleModel>();
            foreach (int i in Enum.GetValues(typeof(LoanCycle)))
            {
                var name = Enum.GetName(typeof(LoanCycle), i);
                double val = 0;
                if (_context.Option.Where(u => u.Key.Equals("SAVING_RATE_" + name)).Count() > 0)
                    val = double.Parse(_context.Option.Where(u => u.Key.Equals("SAVING_RATE_" + name)).First().Value);
                Rates.Add(new LoanCycleModel
                {
                    LoanCycle = (LoanCycle)Enum.GetValues(typeof(LoanCycle)).GetValue(i),
                    Rate = val
                });
            }
            return Rates;
        }
        [HttpGet]
        public IActionResult Index()
        {
            GlobalVariables();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "cliente")]
        public async Task<IActionResult> Cprofile(string returnUrl = "")
        {
            ViewData["returnUrl"] = returnUrl;
            var userId = _context.CurrentUserId;
            ViewBag.AccountPaymentOnline = new AccountPayment();
            var q1 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 0).OrderByDescending(u => u.CreatedDate);
            var q2 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 1).OrderByDescending(u => u.CreatedDate);
            var q3 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 2).OrderByDescending(u => u.CreatedDate);
            ViewBag.step = GetProfileStep().Result;
            if (ViewBag.step == 2)
            {
                if(returnUrl.Equals("/home/wantloan"))return RedirectToAction(nameof(HomeController.Wantloan), "Home");
            }
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            AccountPayment acc = new AccountPayment
            {
                Id = 0,
                UserId = userId,
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
            ViewBag.Payment01 = q1.Count()>0?q1.First():acc;
            acc.Type = 1;
            ViewBag.Payment02 = q2.Count()>0?q2.First():acc;
            acc.Type = 2;
            ViewBag.Payment03 = q3.Count()>0?q3.First():acc;
            if (ViewBag.step > 1) {
                ViewBag.loan = new LoanRequest
                {
                    ClientId = userId,
                    Amount = 0,
                    InterestingRate = 8.9,
                    Cycle = LoanCycle.SEMANAL,
                    Times = 1,
                    Status = LoanStatus.New,
                    StatusReason = "",
                    RequestedDate = DateTime.Now
                };
            }
            ViewBag.Company = _context.Company.OrderBy(u => u.Name).ToList();
            ViewBag.Province = _context.Province.OrderBy(u => u.Id).ToList();
            ViewBag.Nationality = _context.Nationality.OrderBy(u => u.Id).ToList();

            ViewBag.Rates = GetLoanRates();

            return View();
        }
        [HttpGet]
        [Authorize(Roles = "inversora")]
        public async Task<IActionResult> Iprofile(string returnUrl="")
        {
            ViewData["returnUrl"] = returnUrl;
            var userId = _context.CurrentUserId;
            ViewBag.AccountPaymentOnline = new AccountPayment();
            var q1 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 0).OrderByDescending(u => u.CreatedDate);
            var q2 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 1).OrderByDescending(u => u.CreatedDate);
            var q3 = _context.AccountPayment.Where(u => u.UserId.Equals(userId) && u.Type == 2).OrderByDescending(u => u.CreatedDate);
            ViewBag.step = GetProfileStep().Result;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if (ViewBag.step == 2)
            {
                if (returnUrl.Equals("/home/wantlend")) return RedirectToAction(nameof(HomeController.Wantlend), "Home");
            }
            AccountPayment acc = new AccountPayment
            {
                Id = 0,
                UserId = userId,
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
            ViewBag.Payment01 = q1.Count() > 0 ? q1.First() : acc;
            acc.Type = 1;
            ViewBag.Payment02 = q2.Count() > 0 ? q2.First() : acc;
            acc.Type = 2;
            ViewBag.Payment03 = q3.Count() > 0 ? q3.First() : acc;
            if (ViewBag.step > 1)
            {
                ViewBag.loan = new LoanRequest
                {
                    ClientId = userId,
                    Amount = 0,
                    InterestingRate = 8.9,
                    Cycle = LoanCycle.SEMANAL,
                    Times = 1,
                    Status = LoanStatus.New,
                    StatusReason = "",
                    RequestedDate = DateTime.Now
                };
            }
            ViewBag.Company = _context.Company.OrderBy(u => u.Name).ToList();
            ViewBag.Province = _context.Province.OrderBy(u => u.Id).ToList();
            ViewBag.Nationality = _context.Nationality.OrderBy(u => u.Id).ToList();

            ViewBag.Rates = GetSavingRates();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Wantloan()
        {
            var userId = _context.CurrentUserId;
            if (userId == null || userId.Equals(""))
                return RedirectToAction(nameof(HomeController.Login), "Home", new { returnUrl = "/home/wantloan" });
            if(User.IsInRole("inversora")) return RedirectToAction(nameof(HomeController.Wantlend), "Home");
            ViewBag.step = GetProfileStep().Result;
            if (ViewBag.step < 2)
                return RedirectToAction(nameof(HomeController.Cprofile), "Home", new { returnUrl = "/home/wantloan" });

            var q = _context.LoanRequest.Where(u => u.ClientId.Equals(userId) && (
                      u.Status == LoanStatus.New ||
                      u.Status == LoanStatus.Representante_Processing ||
                      u.Status == LoanStatus.Collection_Processing ||
                      u.Status == LoanStatus.Service_Processing ||
                      u.Status == LoanStatus.Contactor_Checking ||
                      u.Status == LoanStatus.Debug_Processing ||
                      u.Status == LoanStatus.Interesting_Process ||
                      u.Status == LoanStatus.Investor_Piad
                  )).OrderByDescending(u => u.CreatedDate);
            if (q.Count() > 0) ViewBag.loan = q.First();
            else ViewBag.loan = new LoanRequest
            {
                ClientId = userId,
                Amount = 0,
                InterestingRate = 8.9,
                Cycle = LoanCycle.SEMANAL,
                Times = 1,
                Status = LoanStatus.New,
                StatusReason = "",
                RequestedDate = DateTime.Now,
                Description=""
            };
            ViewBag.Rates = new List<LoanCycleModel>();
            foreach (int i in Enum.GetValues(typeof(LoanCycle)))
            {
                var name = Enum.GetName(typeof(LoanCycle), i);
                double val = 0;
                if (_context.Option.Where(u => u.Key.Equals("LOAN_RATE_" + name)).Count() > 0)
                    val = double.Parse(_context.Option.Where(u => u.Key.Equals("LOAN_RATE_" + name)).First().Value);
                ViewBag.Rates.Add(new LoanCycleModel
                {
                    LoanCycle = (LoanCycle)Enum.GetValues(typeof(LoanCycle)).GetValue(i),
                    Rate = val
                });
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Wantlend(int tab=1)
        {
            ViewBag.tab = tab;
            var userId = _context.CurrentUserId;
            if (userId == null || userId.Equals("")) 
                return RedirectToAction(nameof(HomeController.Login), "Home",new { returnUrl = "/home/wantlend" });
            if (User.IsInRole("cliente")) return RedirectToAction(nameof(HomeController.Wantloan), "Home");
            ViewBag.step = GetProfileStep().Result;
            if (ViewBag.step < 2)
                return RedirectToAction(nameof(HomeController.Iprofile), "Home", new { returnUrl = "/home/wantlend" });
            IQueryable<ApplicationLoanRequest> query = (from a in _context.Set<LoanRequest>()
                                                        join b in _context.Set<ApplicationUser>()
                                                        on a.ClientId equals b.Id into g
                                                        from b in g.DefaultIfEmpty()
                                                        select new ApplicationLoanRequest
                                                        {
                                                            Id = a.Id,
                                                            ClientId = a.ClientId,
                                                            RequestedDate = a.RequestedDate,
                                                            Amount = a.Amount,
                                                            InterestingRate = a.InterestingRate,
                                                            Cycle = a.Cycle,
                                                            Times = a.Times,
                                                            Status = a.Status,
                                                            StatusReason = a.StatusReason,
                                                            Description=a.Description,
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
            //query = query.Where(u =>u.Status != LoanStatus.New);
            List<ApplicationLoanRequest> loans = query.ToList();
            for(int i=0;i< loans.Count();i++) {
                loans[i].isApplied=_context.Investment.Where(u => u.LoanId == loans[i].Id).Count() > 0;
            }
            ViewBag.loans = loans;
            IQueryable<ApplicationInvestment> iquery = (from a in _context.Set<Investment>()
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
                                                            //Description = a.Description,
                                                            LoanId=a.LoanId,
                                                            CreatedBy = a.CreatedBy,
                                                            CreatedDate = a.CreatedDate,
                                                            CreatedDevice = a.CreatedDevice,
                                                            UpdatedBy = a.UpdatedBy,
                                                            UpdatedDate = a.UpdatedDate,
                                                            UpdatedDevice = a.UpdatedDevice
                                                        });
            List<ApplicationInvestment> investments = iquery.ToList();
            for (int i = 0; i < investments.Count(); i++)
            {
                investments[i].Paid = 0;
                foreach (InvestmentStatus status in _context.InvestmentStatus.Where(u => u.InvestmentId == investments[i].Id&& u.Status==InvestStatus.Created_Milestone).ToList()){
                    investments[i].Paid += status.Paid;
                }
            }
            ViewBag.investments = investments;
            ViewBag.Rates = GetSavingRates();
            return View();
        }
        [HttpGet]
        public IActionResult Blog()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BlogDetail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Howitwork()
        {
            return View();
        }
        [HttpGet]
        public IActionResult FAQ()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Loancalculator()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string page="login", string returnUrl = "")
        {
            ViewData["returnUrl"] = returnUrl;
            ViewBag.page = page;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username,string password, string returnUrl="")
        {
            try
            {
                ViewData["returnUrl"] = returnUrl;
                var user = _userManager.Users.Where(u => u.Email.Equals(username)).SingleOrDefault(); 
                if (user == null)
                {
                    user = _userManager.Users.Where(u => u.UserName.Equals(username)).SingleOrDefault(); 
                }
                if (user == null)
                {
                    ViewBag.error = "Intento de inicio de sesión no válido.";
                    return View();
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //Response.Cookies.Append("loan.droid.app.loggedin.user", user.UserName);
                    HttpContext.Session.SetString("loan.droid.app.loggedin.userid", user.Id);
                    if (_userManager.GetUsersInRoleAsync("cliente").Result.Where(u => u.Id.Equals(user.Id)).Count() > 0)
                    {
                        HttpContext.Session.SetString("loan.droid.app.loggedin.usertype", "client");
                        if (returnUrl==null||returnUrl.Equals("")) returnUrl = "/home/cprofile";
                    }
                    else if (_userManager.GetUsersInRoleAsync("inversora").Result.Where(u => u.Id.Equals(user.Id)).Count() > 0)
                    {
                        HttpContext.Session.SetString("loan.droid.app.loggedin.usertype", "investor");
                        if (returnUrl == null || returnUrl.Equals("")) returnUrl = "/home/iprofile";
                    }
                    else return RedirectToAction(nameof(AdminController.Index), "Admin");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    //_logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ViewBag.error="Invalid login attempt.";
                    return View();
                }
                // If we got this far, something faild, redisplay form 
                return View();
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                return View();
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if(returnUrl==null) return RedirectToAction(nameof(HomeController.Index), "Home");
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            int usertype,string username, string email,string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var isCurrentNameIsExist = _userManager.Users.Where(u => u.UserName.Equals(username)).Any();

            if (isCurrentNameIsExist)
            {
                ViewBag.error= "double_username";
                return RedirectToAction(nameof(HomeController.Login), "Home");
            }

            var isCurrentEmailIsExist = _userManager.Users.Where(u => u.Email.Equals(email)).Any();

            if (isCurrentEmailIsExist)
            {
                ViewBag.error= "double_email";
                return RedirectToAction(nameof(HomeController.Login), "Home");
            }

            var avatarImage = "data:image/png;base64,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";
            ApplicationUser user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = false,
                IsEnabled = true,
                AvatarImage = CommonUtil.DecodeUrlBase64(avatarImage)
            };
            var result = await _accountManager.CreateUserAsync(user, new string[] { usertype==0?"cliente": "inversora" }, password);
            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user.UserName, password, true, lockoutOnFailure: false);
                if ((returnUrl==null||returnUrl.Equals(""))&&usertype==0) returnUrl = "/home/cprofile";
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction(nameof(HomeController.Login), "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Login), "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
        
    }
}
