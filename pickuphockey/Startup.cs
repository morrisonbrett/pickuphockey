using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pickuphockey.Startup))]
namespace pickuphockey
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
