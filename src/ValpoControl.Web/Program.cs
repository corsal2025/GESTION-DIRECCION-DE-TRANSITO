using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using ValpoControl.Infrastructure.Extensions;
using ValpoControl.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri)
});

// MudBlazor
builder.Services.AddMudServices();

// Agregar DbContext y servicios municipales
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=valpo-control.db";
var rutaBaseServidor = builder.Configuration.GetValue<string>("RutaBaseServidor")
    ?? @".\Gestion";

builder.Services.AddMunicipalidadModulesSqlite(connectionString, rutaBaseServidor);

// Agregar logging
builder.Logging.AddConsole();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode();

// Crear base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ValpoControlDbContext>();
    dbContext.Database.EnsureCreated();
    Console.WriteLine("✅ Base de datos inicializada");
}

Console.WriteLine("🚀 Iniciando ValpoControl Hub...");
app.Run();
