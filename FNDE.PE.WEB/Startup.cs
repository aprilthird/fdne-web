using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FDNE.PE.WEB.PORTAL.Startup))]
namespace FDNE.PE.WEB.PORTAL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
