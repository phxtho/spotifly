using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spotifly.Models;

[assembly: HostingStartup(typeof(Spotifly.Areas.Identity.IdentityHostingStartup))]
namespace Spotifly.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<SpotiflyContext>(options =>
                    options.UseMySQL(
                        context.Configuration.GetConnectionString("Default")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<SpotiflyContext>();
            });
        }
    }
}