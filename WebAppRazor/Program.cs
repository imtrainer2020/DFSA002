using DBO;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add services to the container.
string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DFSA002Project;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.InjectServices(connectionString);

// 1. Add Authentication Services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index"; // Where to go if not logged in
        options.AccessDeniedPath = "/Index"; // Where to go if role is wrong
    });

// 2. Add Authorization (Roles)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // Who are you?
app.UseAuthorization();  // Are you allowed?

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
