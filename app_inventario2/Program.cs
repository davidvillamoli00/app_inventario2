using app_inventario2.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var conString = builder.Configuration.GetConnectionString("conexion") ??
     throw new InvalidOperationException("Connection string 'BloggingContext'" +
    " not found.");
builder.Services.AddDbContext<AppInventarioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));
var app = builder.Build();

// --- Bloque para probar la conexión a la base de datos ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppInventarioContext>();
    if (context.Database.CanConnect())
    {
        Console.WriteLine(" Conexión con la base de datos OK");
    }
    else
    {
        Console.WriteLine(" No se pudo conectar a la base de datos");
    }
}

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
