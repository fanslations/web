using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Paranovels.Mvc.Models;

namespace Paranovels.Mvc
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType =  DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                //Provider = new CookieAuthenticationProvider
                //{
                //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                //        validateInterval: TimeSpan.FromMinutes(30),
                //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                //}
            });


            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            app.UseMicrosoftAccountAuthentication(
                clientId: "000000004415596E",
                clientSecret: "ora1r6nMuGN3WXwmK5WKUCsyMMWSnSrG");

            app.UseTwitterAuthentication(
                consumerKey: "dgXhngxFz7a1mz9dgdRtHLWIb",
                consumerSecret: "uBd5psVe3xHRpGOx1kztFZ1ijYqs5DCiudIAKIQnm1B4I2lg0i");

            app.UseFacebookAuthentication(
                appId: "1597519003870102",
                appSecret: "da29786df187804bcf02dc324009b499");

            app.UseGoogleAuthentication(
                clientId: "455221808943-r9i596dqdebng8a70qedcorb4enbgf9h.apps.googleusercontent.com",
                clientSecret: "wNLTk4_UdfTP3QyDAWJxkgTp");
        }
    }
}