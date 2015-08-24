using Microsoft.Owin;
using Pigeon.Service;

[assembly: OwinStartup(typeof (Startup))]

namespace Pigeon.Service
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