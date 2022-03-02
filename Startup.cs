using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using device_wall_backend.Data;
using device_wall_backend.Modules.Dashboard.Control;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using device_wall_backend.Modules.Dashboard.Gateway;
using device_wall_backend.Modules.Lendings.Control;
using device_wall_backend.Modules.Lendings.Gateway;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AspNet.Security.OAuth.GitLab;
using device_wall_backend.Authentication;
using device_wall_backend.Models;
using device_wall_backend.Modules.Users.Control;
using device_wall_backend.Modules.Users.Gateway;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "device_wall_backend", Version = "v1"});
            });
            services.AddIdentity<DeviceWallUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<DeviceWallContext>();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme=JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://git.slashwhy.de";
                    options.Audience = "fd2dcaf8dbff0e54d71d6d26cb7a2610f686528bb3b24cf40bd5b232645a5688";
                    options.SaveToken = true;
                }
            )
            .AddCookie("Cookies",options =>
            {
                options.LoginPath = "/Account/login-callback";
                options.Cookie.Name = "Cookies";
            })/*.AddOAuth<GitLabAuthenticationOptions,MDWGitLabAuthHandler>*/
            .AddGitLab("GitLab",options =>
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.ClientId = "fd2dcaf8dbff0e54d71d6d26cb7a2610f686528bb3b24cf40bd5b232645a5688";
                options.ClientSecret = "7844a873747524022ed2c966b011c0404c3207aa1948a4e0b3d74afed8e99dec";
                //CallbackPath: where the OAuth application redirects the user with state and code to the OAuth middleware internal route,
                //which by default is /signin-gitlab
                options.CallbackPath = "/";
                options.AuthorizationEndpoint = "https://git.slashwhy.de/oauth/authorize";
                options.TokenEndpoint = "https://git.slashwhy.de/oauth/token";
                options.UserInformationEndpoint = "https://git.slashwhy.de/api/v4/user";
                options.SaveTokens = true;

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
                options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "name");
                options.ClaimActions.MapJsonKey("urn:gitlab:avatar", "avatar_url","url");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        context.RunClaimActions(user.RootElement);
                    }
                };
            });
            services.AddHttpContextAccessor();

            services.AddCors();
            //to avoid cyclic referencing in Serialization
            services.AddMvc()
                .AddNewtonsoftJson(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });
            
            services.AddRazorPages()
                .AddNewtonsoftJson();
            services.AddCoreAdmin();
            services.AddControllersWithViews();
            services.AddControllers()
                .AddNewtonsoftJson();
            
            services.AddScoped<ILendingManagement,LendingManagement>();
            services.AddScoped<ILendingRepository,LendingRepository>();
            services.AddScoped<IDashboardManagement,DashboardManagement>();
            services.AddScoped<IDashboardRepository,DashboardRepository>();
            services.AddScoped<ISearchManagement, SearchManagement>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogger,Logger<DashboardRepository>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()){
                var context = serviceScope.ServiceProvider.GetRequiredService<DeviceWallContext>();
                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "device_wall_backend v1");
                    c.RoutePrefix = string.Empty;
                    c.OAuthClientId("fd2dcaf8dbff0e54d71d6d26cb7a2610f686528bb3b24cf40bd5b232645a5688");
                    c.OAuthClientSecret("7844a873747524022ed2c966b011c0404c3207aa1948a4e0b3d74afed8e99dec");
                    c.OAuth2RedirectUrl("/");
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                });
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            
            app.UseCoreAdminCustomUrl("admin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
