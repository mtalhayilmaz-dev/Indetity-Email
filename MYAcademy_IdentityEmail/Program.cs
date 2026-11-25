using Microsoft.AspNetCore.Identity;
using MYAcademy_IdentityEmail.Context;
using MYAcademy_IdentityEmail.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmailContext>();
builder.Services.AddIdentity<AppUser, IdentityRole>(opsiyon =>
{
    //Şifrenin Uzunlugu
    opsiyon.Password.RequiredLength = 8;
    //En az bir küçük harf (a-z) zorunlu
    opsiyon.Password.RequireLowercase = true;
    //En az bir büyük harf (a-z) zorunlu
    opsiyon.Password.RequireUppercase = true;
    //En az bir rakam (0-9) zorunlu olsun mu (true / false)
    opsiyon.Password.RequireDigit = true;
    //En az bir özel karakter (*, ?, ! vb.)
    opsiyon.Password.RequireNonAlphanumeric = true;

    opsiyon.Lockout.MaxFailedAccessAttempts = 5;
    opsiyon.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    opsiyon.Lockout.AllowedForNewUsers = true;

    
}).AddEntityFrameworkStores<EmailContext>().AddDefaultTokenProviders();




// Add services to the container.
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
app.UseStatusCodePagesWithReExecute("/ErrorPages/Page{0}", "?statusCode={0}");
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
