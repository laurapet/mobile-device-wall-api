using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using device_wall_backend.Data;
using device_wall_backend.Modules.Dashboard.Control;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using device_wall_backend.Modules.Dashboard.Gateway;
using device_wall_backend.Modules.Lendings.Control;
using device_wall_backend.Modules.Lendings.Gateway;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace device_wall_backend
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
            services.AddEntityFrameworkNpgsql().AddDbContext<DeviceWallContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "device_wall_backend", Version = "v1" });
            });
            
            //to avoid cyclic referencing in Serialization
            services.AddMvc()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            services.AddAuthentication().AddGitLab(options =>
            {
                options.ClientId = "fd2dcaf8dbff0e54d71d6d26cb7a2610f686528bb3b24cf40bd5b232645a5688";
                options.ClientSecret = "7844a873747524022ed2c966b011c0404c3207aa1948a4e0b3d74afed8e99dec";
                options.CallbackPath = "http://localhost:4000/";
            });
            
            services.AddRazorPages();
            services.AddCoreAdmin();
            services.AddControllersWithViews();
            services.AddScoped<ILendingManagement,LendingManagement>();
            services.AddScoped<ILendingRepository,LendingRepository>();
            services.AddScoped<IDashboardManagement,DashboardManagement>();
            services.AddScoped<IDashboardRepository,DashboardRepository>();
            services.AddScoped<ILogger,Logger<DashboardRepository>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()){
                var context = serviceScope.ServiceProvider.GetRequiredService<DeviceWallContext>();
                //context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "device_wall_backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
