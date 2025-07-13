using Api_comerce.Data;
using Microsoft.EntityFrameworkCore;
using Api_comerce.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using Api_comerce.Services.Products;
using Microsoft.Extensions.FileProviders;
using Api_comerce.Services.Lineas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api_comerce.Services.JWTService;
using Api_comerce.Services.Cart;
using Microsoft.OpenApi.Models;
using Api_comerce.Services.Checkout;
using Api_comerce.Services.Wishlists;
using Api_comerce.Services.AccountsDatosFacturacion;
using Api_comerce.Services.AccountsDirecciones;
using Api_comerce.Services.correos;
using Api_comerce.Services.ProductosComentarios;



var builder = WebApplication.CreateBuilder(args);

// Cadena de conexi�n desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ILineasService, LineasService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IDatosFacturacionServicie, DatosFacturacionService>();
builder.Services.AddScoped<IAccountsDireccionesServicie, AccountsDireccionesServicie>();
builder.Services.AddScoped<ICorreoSevice, CorreoSevice>();
builder.Services.AddScoped<IProductosComentariosService, ProductosComentariosService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyOrigin() // Permitir solicitudes desde cualquier origen
            .AllowAnyMethod() // Permitir cualquier método HTTP
            .AllowAnyHeader() // Permitir cualquier encabezado HTTP
    );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),

            // Aceptar múltiples emisores
            IssuerValidator = (issuer, token, parameters) =>
            {
                var validIssuers = new[] {
                config["Jwt:Issuer"], // Emisor local
                "https://accounts.google.com", // Emisor de Google
                "accounts.google.com"
            };

                if (!validIssuers.Contains(issuer))
                    throw new SecurityTokenInvalidIssuerException("Issuer inválido");

                return issuer;
            },

            // Aceptar múltiples audiencias
            AudienceValidator = (audiences, token, parameters) =>
            {
                var validAudiences = new[]
                    {
                        config["Jwt:Audience"],      
                        config["Google:ClientId"]     // ClientId de tu app en Google
                     };

                return audiences.Any(aud => validAudiences.Contains(aud));
            }
        };
    });


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Comerce", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT con el esquema Bearer. Ejemplo: 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Authorization"
            },
            Array.Empty<string>()
        }
    });
});

//builder.Services.AddDbContext<AppContext>(options =>
//    options.UseSqlServer(config["ConnectionString("DefaultConnection")")
//           .EnableSensitiveDataLogging()
//           .LogTo(Console.WriteLine, LogLevel.Information));




builder.Services.AddAuthorization();
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//        options.JsonSerializerOptions.WriteIndented = true;
//    });


// Swagger/OpenAPI

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Middleware en entorno desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Assets")),
    RequestPath = "/Assets"
});

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
