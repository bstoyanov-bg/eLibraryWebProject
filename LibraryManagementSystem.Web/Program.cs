using LibraryManagementSystem.Data;
using LibraryManagementSystem.Data.Models;
using LibraryManagementSystem.Data.Seeding;
using LibraryManagementSystem.Services.Data.Interfaces;
using LibraryManagementSystem.Web.Infrastructure.Extensions;
using LibraryManagementSystem.Web.Infrastructure.ModelBinders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ELibraryDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
                options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
                options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
                options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
                options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric");
                options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
            })
                // Add identity Roles.
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ELibraryDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
                options.AccessDeniedPath = "/Home/Error/401";
            });

            builder.Services.AddApplicationServices(typeof(IBookService));
            // Register UserService for the Admin area.
            builder.Services.AddApplicationServices(typeof(Areas.Admin.Services.Interfaces.IUserService));
            // Register Lazy services.
            builder.Services.AddScoped(provider =>
            {
                return new Lazy<IEditionService>(() => provider.GetRequiredService<IEditionService>());
            });
            builder.Services.AddScoped(provider =>
            {
                return new Lazy<IRatingService>(() => provider.GetRequiredService<IRatingService>());
            });
            builder.Services.AddScoped(provider =>
            {
                return new Lazy<IBookService>(() => provider.GetRequiredService<IBookService>());
            });

            builder.Services.AddRecaptchaService();

            builder.Services.AddMemoryCache();

            builder.Services
                .AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    // Custom Decimal Model Binder.
                    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                    // Prevent Cross-Site Request Forgery (CSRF).
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });

            WebApplication app = builder.Build();

            // Seed data when the application starts.
            using (IServiceScope serviceScope = app.Services.CreateScope())
            {
                ELibraryDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ELibraryDbContext>();

                dbContext.Database.Migrate();

                new ELibraryDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error?statusCode=500");
                // Custom Error pages with status code.
                app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
                app.UseHsts();
            }

            //var cultureInfo = CultureInfo.InvariantCulture;
            //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            //CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Suggest using top level route registrations UseEndpoints.
            #pragma warning disable ASP0014
            // Add Admin area routing.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapRazorPages();

            app.Run();
        }
    }   
}
