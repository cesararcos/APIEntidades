using WebEntidadesMVC.Utilities;
using WebEntidadesMVC.Utilities.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // Registrar HttpClient para consumir la API

// Añadir servicios para usar sesiones
builder.Services.AddDistributedMemoryCache(); // Necesario para almacenar la sesión en la memoria
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Asegura que la cookie de sesión solo sea accesible desde el servidor
    options.Cookie.IsEssential = true; // Necesario para el consentimiento de cookies bajo GDPR
});

// Inyección Dependencia
builder.Services.AddTransient<IGetHomeService, GetHomeService>();
builder.Services.AddTransient<IGetService, GetServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Usar el middleware de sesión
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
