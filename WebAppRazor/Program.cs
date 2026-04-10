using DBO;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Razor Pages and tell it to LOCK EVERYTHING by default
builder.Services.AddRazorPages(options =>
{
    // This locks EVERYTHING by default
    options.Conventions.AuthorizeFolder("/");

    // This unlocks ONLY the landing page and Forgot Password page
    options.Conventions.AllowAnonymousToPage("/Index"); // Unlock Login page
    options.Conventions.AllowAnonymousToPage("/ForgotPassword"); // Unlock Forgot Password
});

// 2. Setup the Cookie System
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Index"; // If not logged in, go here
    options.AccessDeniedPath = "/Index"; // If wrong role, go here
});

// 3. Add Authorization (Roles)
builder.Services.AddAuthorization();

// Add services to the container.
string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DFSA002Project;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.InjectServices(connectionString);

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
