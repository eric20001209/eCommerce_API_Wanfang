using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using eCommerce_API.Data;
using eCommerce_API.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace eCommerce_API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            //Log.Logger = new LoggerConfiguration().CreateLogger();
            Configuration =  configuration ;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http//wanfangapi.gpos.co.nz:81", // "http//localhost:8088",
                    ValidAudience = "http//wanfangapi.gpos.co.nz:81",   //"http//localhost:8088",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Startup.Configuration["TokenSecretKey"]))
                };
            });
            services.AddCors();

            string connectionString = @"Server=192.168.1.204\sql2012;Database=wanfang_cloud14;User Id=eznz;password=9seqxtf7";
            services.AddScoped<rst374_cloud12Context>();
            services.AddTransient<rst374_cloud12Context>();
            services.AddScoped<FreightContext>();
            services.AddScoped<ISettings,SettingsRepository>();
            services.AddTransient<FreightContext>();
            services.AddTransient<iMailService, MailService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,  rst374_cloud12Context context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseDefaultFiles();
            app.UseHttpsRedirection();

            app.UseCors(builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());

            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<Models.CodeRelations, Dto.ItemDto>();
                cfg.CreateMap<Models.OrderItem, Dto.OrderItemDto>();
            });
                

            app.UseMvc();
        }
    }
}
