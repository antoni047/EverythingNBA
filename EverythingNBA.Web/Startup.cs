using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;

using EverythingNBA.Services.Implementations;
using EverythingNBA.Services;
using EverythingNBA.Data;
using EverythingNBA.Web.Data;

namespace EverythingNBA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAutoMapper(typeof(Web.Mapping.MappingProfile));

            services.AddDbContext<EverythingNBADbContext>(
                options => options.UseSqlServer(@"Server=DESKTOP-E6O5I68\SQLEXPRESS01;Database=EverythingNbaDb;Integrated Security=True;"));

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

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("HeadToHeadGames", "Games/HeadToHead/{team1Name}-vs-{team2Name}", new { controller = "Games", action = "HeadToHead" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
