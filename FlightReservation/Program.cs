using FlightReservation.Areas.Admin.Services;
using FlightReservation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlightReservation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Veritabaný baðlantý dizesini al ve baðlamý kaydet.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // DbContext'i SQL Server ile kullanacak þekilde yapýlandýr.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Geliþtirici hata sayfalarýný etkinleþtir.
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Varsayýlan kimlik sistemi ayarlarýný yapýlandýr.
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // MVC Controller ve View'larý için hizmetleri kaydet.
            builder.Services.AddControllersWithViews();

            // Yönetim alaný için hizmetleri kaydet.
            builder.Services.AddScoped<IAirlineService, AirlineService>();
            builder.Services.AddScoped<IFlightService, FlightService>();
            builder.Services.AddScoped<IAirportService, AirportService>();
            builder.Services.AddSession();

            // HTTP istek pipeline'ýný yapýlandýr.
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    // Ensure the database is created.
                    context.Database.EnsureCreated();

                    // Call the method to initialize roles and admin user.
                    InitializeRolesAndAdminUser(userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                }
            }

            // Uygulama geliþtirme modunda ise, migration endpoint'ini kullan.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                // Üretim için hata yöneticisi ve HSTS kullan.
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // HTTPS yönlendirmesi ve statik dosyalarý kullan.
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Yönlendirme.
            app.UseRouting();

            app.UseSession();
            // Kimlik doðrulama ve yetkilendirme.
            app.UseAuthentication();
            app.UseAuthorization();

            // Yönetim alaný için rota haritalarý oluþtur.
            app.MapAreaControllerRoute(
                name: "Admin",
                areaName: "Admin",
                pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

            // Yolcu alaný için rota haritalarý oluþtur.
            app.MapAreaControllerRoute(
                name: "Passenger",
                areaName: "Passenger",
                pattern: "Passenger/{controller=Home}/{action=Index}/{id?}");

            // Varsayýlan rota haritasý.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Razor Pages için rota haritalarý.
            app.MapRazorPages();

            // Uygulamayý çalýþtýr.
            app.Run();
        }

        private static async Task InitializeRolesAndAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = await userManager.FindByEmailAsync("admin@mail.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    EmailConfirmed = true 
                };

                string adminUserPassword = "Admin123.";
                var createAdminUser = await userManager.CreateAsync(adminUser, adminUserPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}