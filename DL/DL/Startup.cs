using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DL.Startup))]
namespace DL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
