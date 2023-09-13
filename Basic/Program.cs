using Basic.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "Grandma.Cookie";
        config.LoginPath = "/Home/Authenticate";
    });

builder.Services.AddAuthorization(config =>
{
    //    var defaultAuthBuilder = new AuthorizationPolicyBuilder();
    //    var defaultAuthPolicy = defaultAuthBuilder
    //        .RequireAuthenticatedUser()
    //        .RequireClaim(ClaimTypes.DateOfBirth)
    //        .Build();

    //    config.DefaultPolicy = defaultAuthPolicy;


    //config.AddPolicy("Claim.DoB", policyBuilder =>
    //{
    //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
    //});

    config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role,"Admin"));

    config.AddPolicy("Claim.DoB", policyBuilder =>
    {
        policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
    });
});

builder.Services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(config =>
    {
        config.Conventions.AuthorizePage("/Razor/Secured");
        config.Conventions.AuthorizePage("/Razor/Policy");
        config.Conventions.AuthorizeFolder("/RazorSecured");
        config.Conventions.AllowAnonymousToPage("/RazorSecured/Anom");
    });

builder.Services.AddControllersWithViews();

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


// Who are you?     
app.UseAuthentication();


// are you allowed?
app.UseAuthorization();

// standard way to showcase authentication
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});


app.MapRazorPages();

app.Run();
