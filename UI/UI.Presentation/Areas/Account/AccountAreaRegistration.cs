using System.Web.Mvc;

namespace UI.Presentation.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Account_Security",
                "Account/Security/{action}/{id}",
                new { controller = "Security", action = "Index", id = UrlParameter.Optional },
                new[] { "UI.Presentation.Areas.Account.Controllers" }
            );

            context.MapRoute(
                "Account_Profile",
                "Account/Profile/{action}/{id}",
                new { controller = "Profile", action = "Index", id = UrlParameter.Optional },
                new[] { "UI.Presentation.Areas.Account.Controllers" }
            );

            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { controller ="Home", action = "Login", id = UrlParameter.Optional },
                new[] { "UI.Presentation.Areas.Account.Controllers" }
            ); 
        }
    }
}