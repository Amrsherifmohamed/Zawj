using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Zwaj.api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Zwaj.api.helper;
using AutoMapper;
using Zwaj.api.Models;
using Stripe;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ZwajApp.api
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
            services.AddDbContext<Datacontext>(x=>x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            IdentityBuilder builder=services.AddIdentityCore<User>(opt=>{
              opt.Password.RequireDigit=false;
              opt.Password.RequiredLength=4;
              opt.Password.RequireNonAlphanumeric=false;
              opt.Password.RequireUppercase=false;
            });
            builder=new IdentityBuilder(builder.UserType,typeof(Role),builder.Services);
            builder.AddEntityFrameworkStores<Datacontext>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(Options=>{
                Options.TokenValidationParameters=new TokenValidationParameters{
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.ASCII.GetBytes
                    (Configuration.GetSection("Appsettings:Token").Value)),
                    ValidateIssuer=false,
                    ValidateAudience=false
                };
            }); 
            services.AddAuthorization(
                    options=>{
                        options.AddPolicy("RequireAdminRole",policy=>policy.RequireRole("Admin"));
                        options.AddPolicy("ModeratorPhotoRole",policy=>policy.RequireRole("Admin","Moderator"));
                        options.AddPolicy("Viponly",policy=>policy.RequireRole("VIP"));
                    }
            );
            services.AddMvc(options=>{
                var policy=new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(option=>{
                option.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                //for preiform json optject in data come
            });
            services.AddCors();
            services.AddSignalR();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
             services.AddAutoMapper();
            // Mapper.Reset();
            services.AddTransient<TrialData>();
            services.AddScoped<IZwajRepositry,ZwajRepository>();
            services.AddScoped<LogUserActivity>();
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,TrialData trialData)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe:SecretKey").Value);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(BuilderExtensions=>
                {
                    BuilderExtensions.Run(async context=>
                    {
                        context.Response.StatusCode=(int)System.Net.HttpStatusCode.InternalServerError;
                        var error=context.Features.Get<IExceptionHandlerFeature>();
                        if(error!=null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
             // trialData.TrialUsers();//=>use this to generate data in triale date in database
            app.UseCors(x=>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseSignalR(routes=>{
                routes.MapHub<ChatHub>("/chat");
            });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
