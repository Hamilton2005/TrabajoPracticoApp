using Azure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using TrabajoPracticoApp.Data;
using TrabajoPracticoApp.MappingProfiles;
using TrabajoPracticoApp.Middleware;
using TrabajoPracticoApp.Models.Settings;
using TrabajoPracticoApp.Repository;
using TrabajoPracticoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// =================== SERVICIOS ======================

// Controladores con vistas
builder.Services.AddControllersWithViews();

// Configuración de JWT y SMTP
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(UserProfile).Assembly));

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LoginDatabase")));

// 8Inyección de dependencias
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Sesiones y cookies
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// Autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("access_token"))
                context.Token = context.Request.Cookies["access_token"];
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Redirect("/Auth/Login");
                context.HandleResponse();
            }
            return Task.CompletedTask;
        },
        OnForbidden = context =>
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Redirect("/Auth/AccessDenied");
            }
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!))
    };
});

var app = builder.Build();

// =================== PIPELINE ======================

// Manejo de errores y HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirecciones HTTPS y archivos estáticos
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing
app.UseRouting();

// Sesión,2 autenticación y autorización
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado para JWT refresh
app.UseMiddleware<JwtRefreshMiddleware>();

// Middleware para mejorar redirecciones de autenticación
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
    {
        Console.WriteLine("Redireccionando a login ... ");
        context.Response.Redirect($"/Auth/Login?returnUrl={context.Request.Path}");
    }
    else if (context.Response.StatusCode == 403 && !context.Response.HasStarted)
    {
        context.Response.Redirect("/Auth/AccessDenied");
    }
});

// Archivos estáticos adicionales si los necesitas
app.MapStaticAssets();

// Rutas por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
