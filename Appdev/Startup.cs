using Microsoft.Owin;
using Owin;
using static Appdev.Controllers.AccountController;

[assembly: OwinStartupAttribute(typeof(Appdev.Startup))]
namespace Appdev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
        }
    }
}
