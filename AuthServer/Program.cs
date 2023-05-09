using AuthServer;
using AuthServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] {"/seed"}).ToArray();
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assembley = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration
    .GetConnectionString("DefaultConnection");

if (seed)
{
    SeedData.EnsureSeedData(defaultConnString);
}

builder.Services.AddDbContext<AspNetIdentityDbContext>(opt =>
    opt.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembley)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = b =>
            b.UseSqlServer(defaultConnString,
            opt => opt.MigrationsAssembly(assembley));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = b =>
            b.UseSqlServer(defaultConnString,
            opt => opt.MigrationsAssembly(assembley));
    })
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

//for wwwroots
app.UseStaticFiles();
app.UseRouting();
// Configure the HTTP request pipeline.
app.UseIdentityServer();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
//app.MapControllers();

app.Run();
