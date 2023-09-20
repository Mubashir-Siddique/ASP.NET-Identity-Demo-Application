using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = "Cookie";

    config.DefaultChallengeScheme = "Oidc";

})
    .AddCookie("Cookie")
    .AddOpenIdConnect("Oidc", config =>
    {
        config.Authority = "https://localhost:44307/";
        config.ClientId = "client_id_MVC";
        config.ClientSecret = "client_secret_MVC";
        config.SaveTokens = true;

        config.ResponseType = "code";

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


app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MapRazorPages();

app.Run();
