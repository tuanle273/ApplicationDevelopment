using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Appdev.Startup))]
namespace Appdev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
