using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Hubs;
using Asp.NetStudentManagementSystem.Models;
using Asp.NetStudentManagementSystem.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddIdentity<AppUser, IdentityRole>(con =>
{
    con.Password.RequiredLength = 8;
    con.Password.RequireNonAlphanumeric = false;
    con.User.RequireUniqueEmail = true;
    con.SignIn.RequireConfirmedEmail = true;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<LayoutService>();
builder.Services.AddSignalR();
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



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Notification}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapHub<ChatHub>("/chathub");

app.Run();
