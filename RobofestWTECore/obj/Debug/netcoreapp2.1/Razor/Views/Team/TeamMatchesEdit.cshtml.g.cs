#pragma checksum "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "71fe4a64966b89d6c10a9d35ebf3d6e1f4a9f658"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(RobofestWTECore.Pages.Team.Views_Team_TeamMatchesEdit), @"mvc.1.0.view", @"/Views/Team/TeamMatchesEdit.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Team/TeamMatchesEdit.cshtml", typeof(RobofestWTECore.Pages.Team.Views_Team_TeamMatchesEdit))]
namespace RobofestWTECore.Pages.Team
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\_ViewImports.cshtml"
using RobofestWTECore;

#line default
#line hidden
#line 3 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\_ViewImports.cshtml"
using RobofestWTECore.Data;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"71fe4a64966b89d6c10a9d35ebf3d6e1f4a9f658", @"/Views/Team/TeamMatchesEdit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ce755af2a7418f0d746543a6139563ea8d84f149", @"/Views/_ViewImports.cshtml")]
    public class Views_Team_TeamMatchesEdit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<RobofestWTE.Models.TeamMatch>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/signalr/dist/browser/signalr.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/jquery/dist/jquery.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
            BeginContext(43, 346, true);
            WriteLiteral(@"
<h1>Team Matches</h1>
<div>
    <input type=""button"" class=""btn btn-danger"" id=""autofill"" value=""Auto Fill Completed"" />
    <input type=""button"" class=""btn btn-danger"" id=""everythingc"" value=""Everything Completed"" />
    <input type=""button"" class=""btn btn-danger"" id=""nothingc"" value=""Nothing Completed"" />
    <a class=""btn btn-warning""");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 389, "\"", 430, 1);
#line 8 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 396, Url.Action("TeamMatches", "Team"), 396, 34, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(431, 40, true);
            WriteLiteral(">Edit Match Layout</a>\r\n</div>\r\n<br />\r\n");
            EndContext();
#line 11 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
   int i = 0;

#line default
#line hidden
#line 12 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
 foreach (var match in Model.OrderBy(m => m.MatchID).ThenBy(m => m.Order))
        {
            i++;
            string idtoinput = i.ToString() + "-" + match.Order;
            string idtoinputbutton = i.ToString() + "-" + match.Order + "-completed";
            string idtoinputteam = i.ToString() + "-" + match.Order + "-team";
            string idtoinputbuttontest = i.ToString() + "-" + match.Order + "-test";
            string idtoinputbuttonrerun = i.ToString() + "-" + match.Order + "-rerun";
            string idtoinputround = i.ToString() + "-" + match.Order + "-round";
            string idtoinputcompleted = i.ToString() + "-" + match.Order + "-completed";

#line default
#line hidden
            BeginContext(1171, 116, true);
            WriteLiteral("<div style=\"background-color:lightgray;border-radius:6px;border:10px solid lightgray;z-index:-1;margin-bottom:10px;\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1287, "\"", 1306, 1);
#line 22 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1292, match.MatchID, 1292, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1307, 32, true);
            WriteLiteral(" class=\"matchentry\">\r\n    <label");
            EndContext();
            BeginWriteAttribute("for", " for=\"", 1339, "\"", 1359, 1);
#line 23 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1345, idtoinputteam, 1345, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1360, 66, true);
            WriteLiteral(">Team #</label>\r\n    <input type=\"text\" class=\"teamentry input-sm\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1426, "\"", 1451, 1);
#line 24 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1434, match.TeamNumber, 1434, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1452, 40, true);
            WriteLiteral(" maxlength=\"7\" placeholder=\"Team Number\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1492, "\"", 1511, 1);
#line 24 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1497, idtoinputteam, 1497, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1512, 24, true);
            WriteLiteral(" disabled />\r\n    <label");
            EndContext();
            BeginWriteAttribute("for", " for=\"", 1536, "\"", 1557, 1);
#line 25 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1542, idtoinputround, 1542, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1558, 59, true);
            WriteLiteral(">Round #</label>\r\n    <input type=\"number\" class=\"input-sm\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1617, "\"", 1640, 1);
#line 26 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1625, match.RoundNum, 1625, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("id", " id=\"", 1641, "\"", 1661, 1);
#line 26 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1646, idtoinputround, 1646, 15, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1662, 135, true);
            WriteLiteral(" placeholder=\"Round #\" max=\"2\" min=\"1\" disabled />\r\n    <input type=\"button\" class=\"btn btn-success completed\" value=\"Toggle Completed\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1797, "\"", 1816, 1);
#line 27 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1802, match.MatchID, 1802, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1817, 89, true);
            WriteLiteral(" />\r\n    <input type=\"button\" class=\"btn btn-primary testmatch\" value=\"Toggle Test Match\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 1906, "\"", 1925, 1);
#line 28 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 1911, match.MatchID, 1911, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1926, 80, true);
            WriteLiteral(" />\r\n    <input type=\"button\" class=\"btn btn-warning rerun\" value=\"Toggle Rerun\"");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 2006, "\"", 2025, 1);
#line 29 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
WriteAttributeValue("", 2011, match.MatchID, 2011, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2026, 27, true);
            WriteLiteral(" />\r\n    <span> | </span>\r\n");
            EndContext();
#line 31 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
     if (match.Completed == true)
    {

#line default
#line hidden
            BeginContext(2095, 57, true);
            WriteLiteral("        <span class=\"badge badge-pill\">Completed</span>\r\n");
            EndContext();
#line 34 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
    }
    else if (match.Completed == false)
    {

#line default
#line hidden
            BeginContext(2206, 97, true);
            WriteLiteral("        <span class=\"badge badge-pill\" style=\"background-color:darkorange\">Not Completed</span>\r\n");
            EndContext();
#line 38 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
    }

#line default
#line hidden
            BeginContext(2310, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 39 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
     if (match.Test == true)
    {

#line default
#line hidden
            BeginContext(2347, 135, true);
            WriteLiteral("        <span class=\"badge badge-pill\" style=\"border: 2px solid deeppink;background-color:lightgray;color:deeppink\">Test Match</span>\r\n");
            EndContext();
#line 42 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
    }

#line default
#line hidden
            BeginContext(2489, 4, true);
            WriteLiteral("    ");
            EndContext();
#line 43 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
     if (match.Rerun == true)
    {

#line default
#line hidden
            BeginContext(2527, 90, true);
            WriteLiteral("        <span class=\"badge badge-pill\" style=\"background-color:forestgreen\">Rerun</span>\r\n");
            EndContext();
#line 46 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
    }

#line default
#line hidden
            BeginContext(2624, 12, true);
            WriteLiteral("\r\n\r\n</div>\r\n");
            EndContext();
#line 50 "C:\Users\djree\source\repos\RobofestOSS\RobofestOSSWeb\RobofestWTECore\Views\Team\TeamMatchesEdit.cshtml"
}

#line default
#line hidden
            BeginContext(2639, 61, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3e70e0aabefd4eaab987fe3fc7f28577", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2700, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(2702, 51, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4373d1c8827c4bf58800e72adff75f6d", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2753, 1221, true);
            WriteLiteral(@"
<script>
    var chat = new signalR.HubConnectionBuilder().withUrl(""/scoreHub"").build();
    chat.on(""reloadRequired"", function () {
        location.reload(true);
    });
    chat.start().then(function () {
        var scheduler = {
            ""schedules"" : []
        }
        $("".completed"").on(""click"", function () {
            var matchid = parseInt($(this).attr(""id""))
            chat.invoke(""editSpecificSchedule"", matchid, ""completed"");
        });
        $("".testmatch"").on(""click"", function () {
            var matchid = parseInt($(this).attr(""id""))
            chat.invoke(""editSpecificSchedule"", matchid, ""testmatch"");
        });
        $("".rerun"").on(""click"", function () {
            var matchid = parseInt($(this).attr(""id""))
            chat.invoke(""editSpecificSchedule"", matchid, ""rerun"");
        });
        $(""#autofill"").on(""click"", function () {
            chat.invoke(""autoComplete"");
        });
        $(""#everythingc"").on(""click"", function () {
            ch");
            WriteLiteral("at.invoke(\"completeAll\", true);\r\n        });\r\n        $(\"#nothingc\").on(\"click\", function () {\r\n            chat.invoke(\"completeAll\", false);\r\n        });\r\n    \r\n    \r\n        \r\n    });\r\n</script>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<RobofestWTE.Models.TeamMatch>> Html { get; private set; }
    }
}
#pragma warning restore 1591
