using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using EduConectaPeru.Data;
using EduConectaPeru.Repositories.Implementations;
using EduConectaPeru.Repositories.Interfaces;
using System;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios MVC (Controllers con Views)
builder.Services.AddControllersWithViews();

// Configurar Entity Framework con SQL Server LocalDB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registrar Repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IQuotaRepository, QuotaRepository>();

// Configurar Autenticación con Cookies (para los 3 roles)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

// Configurar Autorización
builder.Services.AddAuthorization(options =>
{
    // Políticas por Rol
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("SecretariaOnly", policy => policy.RequireRole("Secretaria"));
    options.AddPolicy("ApoderadoOnly", policy => policy.RequireRole("Apoderado"));
    options.AddPolicy("AdminOrSecretaria", policy => policy.RequireRole("Administrador", "Secretaria"));
});

// Configurar Sesiones (para el Carrito de Cuotas)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar distribución de memoria para caché
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session debe ir ANTES de Authentication y Authorization
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Configurar rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();