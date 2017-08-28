using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Levi9Library.Startup))]
namespace Levi9Library
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
