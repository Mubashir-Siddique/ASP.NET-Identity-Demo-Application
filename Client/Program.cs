using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = "ClientCookie";

    config.DefaultSignInScheme = "ClientCookie";

    config.DefaultChallengeScheme = "OurServer";

})
    .AddCookie("ClientCookie")
    .AddOAuth("OurServer", config =>
    {
        config.ClientId = "client_id";
        config.ClientSecret = "client_secret";
        config.CallbackPath = "/oauth/callback";
        config.AuthorizationEndpoint = "https://localhost:7165/oauth/authorize";
        config.TokenEndpoint = "https://localhost:7165/oauth/token";

        config.SaveTokens = true;

        config.Events = new OAuthEvents()
        {
            OnCreatingTicket= context =>
            {
                var accessToken = context.AccessToken;
                var base64payload = accessToken.Split('.')[1];
                var bytes = Convert.FromBase64String(base64payload);
                var JsonPayload = Encoding.UTF8.GetString(bytes);

                var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonPayload);

                foreach (var claim in claims)
                {
                    context.Identity.AddClaim(new Claim(claim.Key,claim.Value));
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

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
});

app.MapRazorPages();

app.Run();
