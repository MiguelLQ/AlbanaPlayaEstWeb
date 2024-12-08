using albanaPlayaEst;
using albanaPlayaEst.Data;
using albanaPlayaEst.Models;
using albanaPlayaEst.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<VehiculoServices>();
builder.Services.AddScoped<IVehiculoServices, VehiculoServices>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<DA_Usuarios>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<MetodoPagoService>();
builder.Services.AddScoped<IFiltrarPorPlacaService, FiltrarPorPlacaService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AlbanaDBcontext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 1))));
// dotnet ef migrations add Migracion1
// dotnet ef database update
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Acceso/Index";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        option.AccessDeniedPath = "/Home/Privacy";
    });

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Index}/{id?}");

app.Run();