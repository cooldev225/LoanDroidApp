#pragma checksum "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "79011ae95bc566896ef3dac7b406e2eef269c2ca"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Transhistory), @"mvc.1.0.view", @"/Views/Admin/Transhistory.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
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
#line 1 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"79011ae95bc566896ef3dac7b406e2eef269c2ca", @"/Views/Admin/Transhistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9d02f0770a77ba05df8c68ce97c90f3af1f4b323", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Transhistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/admin/js/transhistory.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
  
    ViewData["Title"] = "Transaction Histories";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"d-flex flex-column-fluid\">\r\n    <!--begin::Container-->\r\n    <div class=\" container \">\r\n        <!--begin::Notice-->\r\n");
#nullable restore
#line 10 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
         if (ViewBag.Attention != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <div class=""alert alert-custom alert-white alert-shadow fade show gutter-b"" role=""alert"">
                <div class=""alert-icon"">
                    <span class=""svg-icon svg-icon-primary svg-icon-xl"">
                        <!--begin::Svg Icon | path:assets/media/svg/icons/Tools/Compass.svg--><svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" width=""24px"" height=""24px"" viewBox=""0 0 24 24"" version=""1.1"">
                            <g stroke=""none"" stroke-width=""1"" fill=""none"" fill-rule=""evenodd"">
                                <rect x=""0"" y=""0"" width=""24"" height=""24"" />
                                <path d=""M7.07744993,12.3040451 C7.72444571,13.0716094 8.54044565,13.6920474 9.46808594,14.1079953 L5,23 L4.5,18 L7.07744993,12.3040451 Z M14.5865511,14.2597864 C15.5319561,13.9019016 16.375416,13.3366121 17.0614026,12.6194459 L19.5,18 L19,23 L14.5865511,14.2597864 Z M12,3.55271368e-14 C12.8284271,3.53749572e-14 13.5,0.671572875 13.5,1.5 L13.5,4 L10.5,4 L10.");
            WriteLiteral(@"5,1.5 C10.5,0.671572875 11.1715729,3.56793164e-14 12,3.55271368e-14 Z"" fill=""#000000"" opacity=""0.3"" />
                                <path d=""M12,10 C13.1045695,10 14,9.1045695 14,8 C14,6.8954305 13.1045695,6 12,6 C10.8954305,6 10,6.8954305 10,8 C10,9.1045695 10.8954305,10 12,10 Z M12,13 C9.23857625,13 7,10.7614237 7,8 C7,5.23857625 9.23857625,3 12,3 C14.7614237,3 17,5.23857625 17,8 C17,10.7614237 14.7614237,13 12,13 Z"" fill=""#000000"" fill-rule=""nonzero"" />
                            </g>
                        </svg><!--end::Svg Icon-->
                    </span>
                </div>
                <div class=""alert-text"">
                    ");
#nullable restore
#line 25 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
               Write(ViewBag.Attention);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 28 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <!--end::Notice-->
        <!--begin::Dashboard-->
        <!--begin::Row-->
        <div class=""row"">
            <!--begin:epg widget-->
            <div class=""col-xxl-12 col-lg-12 mb-7"">
                <!--begin::Card-->
                <div class=""card card-custom"">
                    <div class=""card-header flex-wrap border-0 pt-6 pb-0"">
                        <div class=""card-title"">
                            <h3 class=""card-label"">
                                ");
#nullable restore
#line 40 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
                           Write(SharedLocalizer["TransactionHistories"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            </h3>
                        </div>
                        <div class=""card-toolbar"">                            
                            <!--
                            <button type=""button"" class=""btn btn-primary font-weight-bolder ml-2"" onclick=""exportExcel();"">
                                <span class=""svg-icon svg-icon-md"">
                                    <svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" width=""24px"" height=""24px"" viewBox=""0 0 24 24"" version=""1.1"">
                                        <g stroke=""none"" stroke-width=""1"" fill=""none"" fill-rule=""evenodd"">
                                            <polygon points=""0 0 24 0 24 24 0 24"" />
                                            <path d=""M18.5,8 C17.1192881,8 16,6.88071187 16,5.5 C16,4.11928813 17.1192881,3 18.5,3 C19.8807119,3 21,4.11928813 21,5.5 C21,6.88071187 19.8807119,8 18.5,8 Z M18.5,21 C17.1192881,21 16,19.8807119 16,18.5 C16,17.1192881 17.1192");
            WriteLiteral(@"881,16 18.5,16 C19.8807119,16 21,17.1192881 21,18.5 C21,19.8807119 19.8807119,21 18.5,21 Z M5.5,21 C4.11928813,21 3,19.8807119 3,18.5 C3,17.1192881 4.11928813,16 5.5,16 C6.88071187,16 8,17.1192881 8,18.5 C8,19.8807119 6.88071187,21 5.5,21 Z"" fill=""#000000"" opacity=""0.3"" />
                                            <path d=""M5.5,8 C4.11928813,8 3,6.88071187 3,5.5 C3,4.11928813 4.11928813,3 5.5,3 C6.88071187,3 8,4.11928813 8,5.5 C8,6.88071187 6.88071187,8 5.5,8 Z M11,4 L13,4 C13.5522847,4 14,4.44771525 14,5 C14,5.55228475 13.5522847,6 13,6 L11,6 C10.4477153,6 10,5.55228475 10,5 C10,4.44771525 10.4477153,4 11,4 Z M11,18 L13,18 C13.5522847,18 14,18.4477153 14,19 C14,19.5522847 13.5522847,20 13,20 L11,20 C10.4477153,20 10,19.5522847 10,19 C10,18.4477153 10.4477153,18 11,18 Z M5,10 C5.55228475,10 6,10.4477153 6,11 L6,13 C6,13.5522847 5.55228475,14 5,14 C4.44771525,14 4,13.5522847 4,13 L4,11 C4,10.4477153 4.44771525,10 5,10 Z M19,10 C19.5522847,10 20,10.4477153 20,11 L20,13 C20,13.5522847 19.5522847,14 19,14 C18.");
            WriteLiteral(@"4477153,14 18,13.5522847 18,13 L18,11 C18,10.4477153 18.4477153,10 19,10 Z"" fill=""#000000"" />
                                        </g>
                                    </svg>
                                </span>
                                ");
#nullable restore
#line 55 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
                           Write(SharedLocalizer["Export"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            </button>
                            -->
                        </div>
                    </div>
                    <div class=""card-body"">
                        <!--begin: Search Form-->
                        <div class=""mb-7"">
                            <div class=""row align-items-center"">
                                <div class=""col-lg-8 col-xl-3"">
                                    <div class=""row align-items-center"">
                                        <div class=""col-md-12 my-2 my-md-0"">
                                            <div class=""input-icon"">
                                                <input type=""text"" class=""form-control""");
            BeginWriteAttribute("placeholder", " placeholder=\"", 5707, "\"", 5750, 2);
#nullable restore
#line 68 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
WriteAttributeValue("", 5721, SharedLocalizer["Search"], 5721, 26, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5747, "...", 5747, 3, true);
            EndWriteAttribute();
            WriteLiteral(@" name=""kt_datatable_search_query"" id=""kt_datatable_search_query"" />
                                                <span><i class=""flaticon2-search-1 text-muted""></i></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class=""col-lg-2 col-xl-2 mt-2 mt-lg-0"">
                                    <a href=""javascript:searchAction();"" class=""btn btn-light-primary px-6 font-weight-bold"">
                                        ");
#nullable restore
#line 77 "E:\2021-development\21-04-05-bank-asp-donglong-6k\LoanDroidApp\LoanDroidApp\Views\Admin\Transhistory.cshtml"
                                   Write(SharedLocalizer["Search"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                    </a>
                                </div>
                            </div>
                        </div>
                        <!--end: Search Form-->
                        <!--begin: Datatable-->
                        <div class=""datatable datatable-bordered datatable-head-custom kt_datatable_class"" name=""kt_datatable"" id=""kt_datatable""></div>
                        <!--end: Datatable-->
                    </div>
                </div>
                <!--end::Card-->
            </div>
            <!--end:epg widget-->
        </div>
        <!--end::Row-->
        <!--end::Dashboard-->
    </div>
    <!--end::Container-->
</div>
<!--end::Entry-->
");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "79011ae95bc566896ef3dac7b406e2eef269c2ca13687", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHtmlLocalizer<LoanDroidApp.CommonResources> SharedLocalizer { get; private set; }
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
