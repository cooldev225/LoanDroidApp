#pragma checksum "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Home\Loancalculator.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ced0052b92f24735603c3a31303fc31d0b981ab9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Loancalculator), @"mvc.1.0.view", @"/Views/Home/Loancalculator.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 2 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\_ViewImports.cshtml"
using App;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\_ViewImports.cshtml"
using Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Home\Loancalculator.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Home\Loancalculator.cshtml"
using Microsoft.AspNetCore.Mvc;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Home\Loancalculator.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ced0052b92f24735603c3a31303fc31d0b981ab9", @"/Views/Home/Loancalculator.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9d02f0770a77ba05df8c68ce97c90f3af1f4b323", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Loancalculator : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 6 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Home\Loancalculator.cshtml"
  
    ViewData["Title"] = "I want to lend | LoanDroidApp";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<div class=""page-header"">
    <div class=""container"">
        <div class=""row"">
            <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                <div class=""page-breadcrumb"">
                    <ol class=""breadcrumb"">
                        <li><a href=""index.html"">Home</a></li>
                        <li class=""active"">Loan Calculator</li>
                    </ol>
                </div>
            </div>
            <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                <div class=""bg-white pinside30"">
                    <div class=""row align-items-center"">
                        <div class=""col-xl-8 col-lg-8 col-md-8 col-sm-12 col-12"">
                            <h1 class=""page-title"">Loan Calculator</h1>
                        </div>
                        <div class=""col-xl-4 col-lg-4 col-md-4 col-sm-12 col-12"">
                            <div class=""btn-action""> <a href=""#!"" class=""btn btn-secondary"">How To Apply</a> </div>
     ");
            WriteLiteral(@"                   </div>
                    </div>
                </div>
                <div class=""sub-nav"" id=""sub-nav"">
                    <ul class=""nav nav-justified"">
                        <li class=""nav-item"">
                            <a href=""contact-us.html"" class=""nav-link"">Give me call back</a>
                        </li>
                        <li class=""nav-item"">
                            <a href=""#!"" class=""nav-link"">Emi Caculator</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
<!-- content start -->
<div class=""container"">
    <div class=""row"">
        <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
            <div class=""wrapper-content bg-white p-3 p-lg-5"">
                <div class=""row"">
                    <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                        <div class=""row"">
                            <div class=""co");
            WriteLiteral(@"l-xl-6 col-lg-6 col-md-6 col-sm-12 col-12"">
                                <div class=""bg-light bg-white p-3 p-lg-5 outline"">
                                    <span>Loan Amount is </span>
                                    <strong>
                                        <span");
            BeginWriteAttribute("class", " class=\"", 2652, "\"", 2660, 0);
            EndWriteAttribute();
            WriteLiteral(@" id=""la_value"">30000</span>
                                    </strong>
                                    <input type=""text"" data-slider=""true"" value=""30000"" data-slider-range=""100000,5000000"" data-slider-step=""10000"" data-slider-snap=""true"" id=""la"">
                                    <hr>
                                    <span>
                                        No. of Month is <strong>
                                            <span");
            BeginWriteAttribute("class", " class=\"", 3120, "\"", 3128, 0);
            EndWriteAttribute();
            WriteLiteral(@" id=""nm_value"">30</span>
                                        </strong>
                                    </span>
                                    <input type=""text"" data-slider=""true"" value=""30"" data-slider-range=""120,360"" data-slider-step=""1"" data-slider-snap=""true"" id=""nm"">
                                    <hr>
                                    <span>
                                        Rate of Interest [ROI] is <strong>
                                            <span");
            BeginWriteAttribute("class", " class=\"", 3630, "\"", 3638, 0);
            EndWriteAttribute();
            WriteLiteral(@" id=""roi_value"">10</span>
                                        </strong>
                                    </span>
                                    <input type=""text"" data-slider=""true"" value=""10.2"" data-slider-range=""8,16"" data-slider-step="".05"" data-slider-snap=""true"" id=""roi"">
                                </div>
                            </div>
                            <div class=""col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12"">
                                <div class=""row"">
                                    <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                                        <div class=""bg-light p-3 outline"">
                                            Monthly EMI
                                            <h2 id='emi' class=""mb-0""></h2>
                                        </div>
                                    </div>
                                    <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                   ");
            WriteLiteral(@"                     <div class=""bg-light p-3 outline"">
                                            Total Interest
                                            <h2 id='tbl_int' class=""mb-0""></h2>
                                        </div>
                                    </div>
                                    <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                                        <div class=""bg-light p-3 outline"">
                                            Payable Amount
                                            <h2 id='tbl_full' class=""mb-0""></h2>
                                        </div>
                                    </div>
                                    <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                                        <div class=""bg-light p-3 outline"">
                                            Interest Percentage
                                            <h2 id='tbl_int_pge' class=""mb-0""></h2>
  ");
            WriteLiteral(@"                                      </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12"">
                        <div id=""loantable"" class='table table-striped table-bordered loantable table-responsive'></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHtmlLocalizer<LoanDroidApp.CommonResources> SharedLocalizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<Models.data.ApplicationUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
