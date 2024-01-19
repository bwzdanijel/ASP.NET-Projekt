using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using ProjektArbeitDanijelMademidda.Controllers;
using ProjektArbeitDanijelMademidda.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); // neue Linie, einfügen; bindet Razor Pages ein
builder.Services.AddControllers();
builder.Services.AddDbContext<ProjektArbeitDanijelMademidda.Models.AppContext>(opt =>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
            .Replace("[Path]", builder.Environment.ContentRootPath)));

builder.Services.AddTransient<DbInitializer>();

builder.Services
 .AddAuthentication(options =>
 {
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
 })
 .AddJwtBearer(o =>
 {
     o.TokenValidationParameters = new TokenValidationParameters
     {
         ValidIssuer = JwtConfiguration.ValidIssuer,
         ValidAudience = JwtConfiguration.ValidAudience,
         IssuerSigningKey = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes(JwtConfiguration.IssuerSigningKey)),
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true
     };
 });


var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = (context) =>
    {
        var headers = context.Context.Response.GetTypedHeaders();

        headers.CacheControl = new CacheControlHeaderValue
        {
            NoCache = true,
            NoStore = true
        };
    }
});

using (var scope = app.Services.CreateScope())
{
    // Execute db initializer
    scope.ServiceProvider.GetRequiredService<DbInitializer>().Run();
}

//app.MapGet("/", () => "Hello World!"); auskommentieren
app.MapControllers();
app.MapRazorPages(); // neue Linie, einfügen; lädt Razor Pages unterhalb /Pages
app.Run();
