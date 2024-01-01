using Microsoft.EntityFrameworkCore;
using MyBank.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyBankDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")!)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.EnableAnnotations());
builder.Services.AddControllers();

var app = builder.Build();


app.Lifetime.ApplicationStarted.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<MyBankDbContext>();

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();