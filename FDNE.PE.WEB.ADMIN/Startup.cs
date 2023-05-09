using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FDNE.PE.WEB.ADMIN.Startup))]
namespace FDNE.PE.WEB.ADMIN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
