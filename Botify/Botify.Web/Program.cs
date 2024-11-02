using Botify.Data.EF;
using Botify.Entidades;
using Botify.Logica;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
 // Registra HttpClient como servicio

builder.Services.AddScoped<IBotLogica, BotLogica>();
builder.Services.AddScoped<ITokenLogica, TokenLogica>();

builder.Services.Configure<SpotifyConfig>(builder.Configuration.GetSection("Spotify"));
builder.Services.AddScoped<BotifyContext>();
builder.Services.AddScoped<IUsuariosLogica, UsuariosLogica>();
builder.Services.AddScoped<AuthService>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Usuarios/IngresarSesion";
});

// Configuraci�n de autenticaci�n (JWT y Cookies)
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
})
.AddCookie("CookieAuth", options =>
{
    options.Cookie.Name = "AuthToken";
    options.LoginPath = "/Usuarios/IniciarSesion";  // Ruta de login
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/Usuarios/IniciarSesion");
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Chat}/{id?}");

app.Run();
