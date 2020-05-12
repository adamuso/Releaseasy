using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Releaseasy.Services;
using Releaseasy.Model;



namespace Releaseasy
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddEnvironmentVariables()
                .Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddDbContext<ReleaseasyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ReleaseasyDatabase")));

            services.AddAuthorization(options =>
            {
                //options.AddPolicy()
            });
            
            

            services.AddMvc();
            services.AddControllers();

            services.AddScoped<RoleManager<Role>>();
            services.AddIdentity<User, Role>(options =>
             {
                 options.Password.RequiredLength = 8;
                 options.Password.RequiredUniqueChars = 3;

                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireDigit = false;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireUppercase = false;

                 options.User.RequireUniqueEmail = true;

                 options.SignIn.RequireConfirmedEmail = true;


             })
               .AddEntityFrameworkStores<ReleaseasyContext>()
            .AddDefaultTokenProviders();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(env.ContentRootPath, "wwwroot")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/res")
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action}");
                endpoints.MapControllerRoute("others", "{*url}", defaults: new { controller = "Home", action = "Index" });
            });

        }
    }
}
