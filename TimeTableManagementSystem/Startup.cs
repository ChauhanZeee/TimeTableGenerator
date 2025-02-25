using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimeTableManagementSystem.Startup))]
namespace TimeTableManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
