using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBank.API.Data;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder);

ConfigureDatabase(builder);

ConfigureApi(builder);

var app = builder.Build();

UseApplication(app);

app.Run();

void ConfigureLogging(WebApplicationBuilder builder)
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog();
}

void ConfigureDatabase(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<MyBankDbContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("Default")!);
    });
}

void ConfigureApi(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(x => x.EnableAnnotations());
    builder.Services.AddControllers();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Secret"]!)),
            ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha256 },
        };
    });
}

void UseApplication(WebApplication app)
{
    // app.Lifetime.ApplicationStarted.Register(() =>
    // {
    //     using var scope = app.Services.CreateScope();
    //     var context = scope.ServiceProvider.GetRequiredService<MyBankDbContext>();

    //     context.Database.EnsureDeleted();
    //     context.Database.EnsureCreated();
    // });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultControllerRoute();
}

public partial class Program { }