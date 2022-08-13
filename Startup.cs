using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Tweet.Context;
using Tweet.Models;
using Tweet.Repository;
using Tweet.Services;
using Tweet.Settings;
using Tweet.Utils;

namespace Tweet
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
            //services.AddSingleton<InterfacesHERE, TheConcreteInstaceOfThatInterfaceHere>
            services.AddSingleton<DapperContext>();
            services.AddSingleton<JwtSettings>();
            services.AddSingleton<IUsersRepository, SqlServerUsersRepository>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthMiddlewareHandler>();




            //this used to be able to return created at action methods
            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;

            });

            services.AddAuthorization(opt =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(

                      JwtBearerDefaults.AuthenticationScheme,
                      CookieAuthenticationDefaults.AuthenticationScheme
                );

                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                opt.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = new TimeSpan(0, 10, 0);
        opt.Cookie.Name = "jwt_super_cookie";
        opt.Cookie.HttpOnly = false;
        opt.Cookie.SameSite = SameSiteMode.None;

    });

            var key = Encoding.UTF8.GetBytes("suppersecret comming right from the dotnet secrets");

            services.AddAuthentication(x =>
            {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false; //set true on production
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IValidator<UserModel>, UserValidator>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweet", Version = "v1" });


            });


            services.AddHealthChecks();


            services.AddCors(policy =>
            {

                policy.AddPolicy("OpenCorsPolicy", opt =>

                    opt.AllowAnyOrigin().
                    AllowAnyHeader().
                    AllowAnyMethod()
                );
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tweet v1"));
            }


            // app.UseHttpsRedirection();
            app.UseCors("OpenCorsPolicy");

            app.UseRouting();

            //using the app.useAuthentication and authorization enables the httpContext.[User] property
            //must be called before map
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
