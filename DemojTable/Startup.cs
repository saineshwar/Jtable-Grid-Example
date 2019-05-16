using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemojTable.Startup))]
namespace DemojTable
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
