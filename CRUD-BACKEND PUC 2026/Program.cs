using CrudApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Serviços ──────────────────────────────────────────────────────────────────

// Registra controllers com suporte a views
builder.Services.AddControllersWithViews();

// Habilita recompilação de Razor em tempo de execução (útil em desenvolvimento)
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Registra o DbContext usando a connection string do appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilita o Swagger/OpenAPI para documentar a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CrudApp API", Version = "v1" });
});

var app = builder.Build();

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────

if (app.Environment.IsDevelopment())
{
    // Swagger disponível apenas em desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrudApp API v1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Rota padrão para o controller MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rotas de API REST
app.MapControllers();

app.Run();
