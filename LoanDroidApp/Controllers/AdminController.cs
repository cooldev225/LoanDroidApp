using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;            
        }
        public void GlobalVariables() {
            ViewData["Title"] = "";
            var representantes = _userManager.GetUsersInRoleAsync("representante").Result.Count();
            var contactors = _userManager.GetUsersInRoleAsync("contactor").Result.Count();
            var servicemanagers = _userManager.GetUsersInRoleAsync("servicemanager").Result.Count();
            var debuggerdepartments = _userManager.GetUsersInRoleAsync("debuggerdepartment").Result.Count();
            var collectiondepartments = _userManager.GetUsersInRoleAsync("collectiondepartment").Result.Count();
            ViewBag.client_count = _userManager.GetUsersInRoleAsync("client").Result.Count();
            ViewBag.investor_count = _userManager.GetUsersInRoleAsync("investor").Result.Count();
            ViewBag.inneraluser_count = representantes + contactors + servicemanagers + debuggerdepartments + collectiondepartments;
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
        public IActionResult Users()
        {
            GlobalVariables();
            return View();
        }
        public IActionResult Clients()
        {
            GlobalVariables();
            return View();
        }
        public IActionResult Investors()
        {
            GlobalVariables();
            return View();
        }
    }
}