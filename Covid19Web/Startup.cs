using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Covid19Web.Startup))]
namespace Covid19Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
