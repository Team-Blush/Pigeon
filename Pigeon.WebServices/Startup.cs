using Microsoft.Owin;
using Pigeon.WebServices;

[assembly: OwinStartup(typeof (Startup))]

namespace Pigeon.WebServices
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}