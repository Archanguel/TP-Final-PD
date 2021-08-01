﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trabajo_Final___Grupo_4.Data;
using Trabajo_Final___Grupo_4.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Trabajo_Final___Grupo_4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<UsuarioContext>(options =>
            //options.UseMySql(Credenciales.GetConnectionString(), new MySqlServerVersion(new Version(8, 0, 11))));
            options.UseMySql(Credenciales.GetConnectionString()));
            //options.UseSqlServer(Configuration.GetConnectionString("UsuarioContext")));
            services.AddScoped<AgenciaManager, AgenciaManager>();
            services.AddScoped<Agencia, Agencia>();
            /*services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();*/

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Login";
                        //options.ReturnUrlParameter = "/Home";
                        //options.ExpireTimeSpan.TotalHours.Equals(2);
                        //options.LogoutPath = "/Login";
                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                //endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}");
            });
        }
    }
}
