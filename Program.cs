
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LocalMessenger.Data;
using Microsoft.EntityFrameworkCore;
using LocalMessenger.Models;
using LocalMessenger.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();  
builder.Services.AddSignalR(); //new
//new
builder.Services.AddDbContext<SettingsBD>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();
builder.Services.AddScoped<LocalMessenger.Services.OllamaServices>();   

builder.Services.Configure<AiLimits>( 
    builder.Configuration.GetSection("AiLimits"));

builder.Services.AddHttpClient<OllamaServices>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.MapHub<LocalMessenger.Hubs.ChatHub>("/chatHub"); //new


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SettingsBD>();
    db.Database.EnsureCreated();
}

app.Run();