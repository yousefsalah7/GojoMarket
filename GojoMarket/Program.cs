using GojoMarket.Data;
using GojoMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
        builder.Services.AddIdentity<IdentityUser,IdentityRole>()
            .AddDefaultTokenProviders().AddDefaultUI()
             .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSession(Options =>
        {
            Options.IdleTimeout = TimeSpan.FromMinutes(10);
            Options.Cookie.HttpOnly = true;
            Options.Cookie.IsEssential = true;
        });
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.MapRazorPages();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
