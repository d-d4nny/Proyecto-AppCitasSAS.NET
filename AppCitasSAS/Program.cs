using AppCitasSAS.Servicios.Implementaciones;
using AppCitasSAS.Servicios.Interfaces;
using DAL.Entidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppCitasSasContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddScoped<IntfPacienteServicio, ImplPacienteServicio>();
builder.Services.AddScoped<IntfPacienteToDao, ImplPacienteToDao>();
builder.Services.AddScoped<IntfPacienteToDto, ImplPacienteToDto>();
builder.Services.AddScoped<IntfEncriptar,  ImplEncriptar>();
builder.Services.AddScoped<IntfEmailRecuperacion, ImplEmailRecuperacion>();
builder.Services.AddScoped<IntfCitasServicio, ImplCitasServicio>();
builder.Services.AddScoped<IntfCitasToDao, ImplCitasToDao>();
builder.Services.AddScoped<IntfCitasToDto, ImplCitasToDto>();
builder.Services.AddScoped<IntfConsultaTurnoServicio, ImplConsultaTurnoServicio>();
builder.Services.AddScoped<IntfConsultaTurnoToDao, ImplConsultaTurnoToDao>();
builder.Services.AddScoped<IntfConsultaTurnoToDto, ImplConsultaTurnoToDto>();
builder.Services.AddScoped<IntfDoctorServicio, ImplDoctorServicio>();
builder.Services.AddScoped<IntfDoctorToDao, ImplDoctorToDao>();
builder.Services.AddScoped<IntfDoctorToDto, ImplDoctorToDto>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/auth/login";
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
