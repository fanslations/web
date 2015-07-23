using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Paranovels.Mvc.Startup))]
namespace Paranovels.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}
