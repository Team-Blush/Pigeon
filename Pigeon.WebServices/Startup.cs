using Microsoft.Owin;
using Pigeon.WebServices;

[assembly: OwinStartup(typeof (Startup))]

namespace Pigeon.WebServices
{
    using Microsoft.Owin.Cors;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            this.ConfigureAuth(app);
        }
    }
}