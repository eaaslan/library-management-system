using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstracts;
using WebApplication1.Data;

using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IBookService, BookService>();


builder.Services.AddDbContext<ApplicationDbContext>(options=>
// we have to tell that which class has the implementation of db context
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // to reach apsettings.json for connectionString use builder.Configuration

builder.Services
    .AddIdentity<AppUser,AppRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
   var services = scope.ServiceProvider;
   try
   {
       // call SeedData.Initialize method to create admin user and roles
       await SeedData.Initialize(services);
   }
   catch (Exception ex)
   {
       var logger = services.GetRequiredService<ILogger<Program>>();
       logger.LogError(ex, "An error occured while adding seed data.");
   }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

// Configure port based on environment
if (app.Environment.IsDevelopment())
{
    // Use port 5000 for local development
    app.Urls.Add("http://localhost:5000");
}
else
{
    // Use Heroku's dynamic port for production
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://+:{port}");
}

app.Run();
