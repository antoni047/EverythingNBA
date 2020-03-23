using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using EverythingNBA.Data;
using EverythingNBA.Services;
using EverythingNBA.Services.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;

namespace EverythingNBA.Web
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
            services.AddDbContext<EverythingNBADbContext>(
                options => options.UseSqlServer(@"Server=DESKTOP-E6O5I68\SQLEXPRESS01;Database=EverythingNbaDb;Integrated Security=True;"));

            services.AddAutoMapper(typeof(Web.Mapping.MappingProfile));
            services.AddMvc();

            services.AddTransient<ISeasonService, SeasonService>();
            services.AddTransient<IAwardService, AwardService>();
            services.AddTransient<IAllStarTeamService, AllStarTeamService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IGameStatisticService, GameStatisticService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IPlayoffService, PlayoffService>();
            services.AddTransient<ISeasonStatisticService, SeasonStatisticService>();
            services.AddTransient<ISeriesService, SeriesService>();
            services.AddTransient<ITeamService, TeamService>();
            services.AddTransient<IImageInfoWriterService, ImageInfoWriterService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            Account account = new Account(
                this.Configuration["Cloudinary:AppName"],
                this.Configuration["Cloudinary:ApiKey"],
                this.Configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "TeamsArchiveArea",
                    areaName: "Archive",
                    pattern: "{area}/{controller=Teams}/{action=TeamDetails}/{teamId:int}/{year:int}"
                    );

                endpoints.MapControllerRoute("HeadToHeadGames", "Games/HeadToHead/{team1Name}-vs-{team2Name}", new { controller = "Games", action = "HeadToHead" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
