using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Repository;
using SassRefactorApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Registra los servicios necesarios para los controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias para servicios
// builder.Services.AddScoped<IYourService, YourService>();
builder.Services.AddScoped<IDBConnection, DbConnection>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IServicesInstitute, InstituteServices>();
// Inyección de dependencias para repositorios
// builder.Services.AddScoped<IYourRepository, YourRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IReposioryInstitute, InstituteRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aquí se mapearán tus controladores
app.MapControllers(); // Esto ahora funcionará correctamente

app.Run();
