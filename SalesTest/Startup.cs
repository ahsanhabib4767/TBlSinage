using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesTest.Startup))]
namespace SalesTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
