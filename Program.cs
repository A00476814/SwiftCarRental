using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwiftCarRental.Areas.Identity.Data;
using SwiftCarRental.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
var connectionString= builder.Configuration.GetConnectionString("defaultConnectionString") ?? throw new InvalidOperationException("Connection string 'userDBContext' not found.");
builder.Services.AddDbContext<userDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString")));

 //Configure SwiftCarRentalDBContext for application data
builder.Services.AddDbContext<SwiftCarRentalDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString") ?? throw new InvalidOperationException("Connection string 'SwiftCarRentalDBContext' not found.")));

// Configure userDBContext for identity
builder.Services.AddDbContext<userDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnectionString")));

// Configure identity with userDBContext
builder.Services.AddDefaultIdentity<SwiftUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<userDBContext>();


// Add other services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
