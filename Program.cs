using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SassRefactorApi;
using SassRefactorApi.Contracts.IRepositories;
using SassRefactorApi.Contracts.Iservices;
using SassRefactorApi.Repository;
using SassRefactorApi.Services;
using System.Text;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };
});

//Singleton
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Services
builder.Services.AddScoped<IDBConnection, DbConnection>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IServicesInstitute, InstituteServices>();

//Repository
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IReposioryInstitute, InstituteRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddMvc();
builder.Services.AddControllers();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
}

app.UseHttpsRedirection();
app.UseRouting();
app.Use(async (context, next) =>
{
    // Antes de llamar al siguiente middleware
    try
    {
        await next(); // Llama al siguiente middleware en la cadena

        // Aquí puedes verificar el resultado de la autenticación
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            // Loguear información adicional
            Console.WriteLine("401 Unauthorized - Access denied.");
            Console.WriteLine("Unauthorized access attempt:");
            Console.WriteLine($"Request Path: {context.Request.Path}");
            Console.WriteLine($"Request Headers: {context.Request.Headers}");
            var userClaims = context.User.Claims;
            foreach (var claim in userClaims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
        }
    }
    catch (Exception ex)
    {
        // Manejo de excepciones (ejemplo: loggear los errores de autenticación)
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync($"Error: {ex.Message}");
    }
});
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
