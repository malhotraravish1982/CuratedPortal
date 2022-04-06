using MasterGenerator.Data.Context;
using MasterGenerator.Data.Repository;
using System.IO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MasterGenerator.Data.Helper;
using Microsoft.AspNetCore.Identity;
using MasterGenerator.Data.Entity;
using MasterGenerator.UI.Extensions;
using MasterGenerator.UI.Helper;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
}); 
builder.Services.AddIdentityServices(builder.Configuration);

//add refernce to the GoogleSheetHelper
builder.Services.AddSingleton(typeof(GoogleSheetsHelper));

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion")))
{
    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2")))
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"scripts", @"ej2.min.js"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2", @"ej2.min.js"));
    }

    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2")))
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap4.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap4.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"material.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"material.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"highcontrast.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"highcontrast.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"fabric.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"fabric.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"tailwind.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"tailwind.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap5.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap5.css"));
    }
}
var app = builder.Build();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjAwMDIxQDMxMzkyZTM0MmUzMEt4Z0ZFMEE5R0pibmN0ZnFCbmZiNUJKUHpmYUwvZlRqMW9DeXJIVFFyN0U9");

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();

