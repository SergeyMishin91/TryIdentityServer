using Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

builder.Services.Configure<IdentityServerSettings>(
    builder.Configuration.GetSection("IdentityServerSettings"));
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.Configure<InteractiveServiceSettings>(
    builder.Configuration.GetSection("InteractiveServiceSettings"));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(
    OpenIdConnectDefaults.AuthenticationScheme,
    opt =>
    {
        opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
        //opt.Authority = builder.Configuration["InteractiveServiceSettings.AuthorityUrl"];
        //opt.ClientId = builder.Configuration["InteractiveServiceSettings.ClientId"];
        //opt.ClientSecret = builder.Configuration["InteractiveServiceSettings.ClientSecret"];
        opt.Authority = "https://localhost:5443";
        opt.ClientId = "interactive";
        opt.ClientSecret = "ClientSecret1";

        opt.ResponseType = "code";
        opt.SaveTokens = true;
        opt.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
