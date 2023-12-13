using Microsoft.AspNetCore.Identity;
using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


string path = "C:\\Users\\angel\\Source\\Repos\\WebAppCondominio\\WebAppCondominio\\firebase-config.json";
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(3000);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();