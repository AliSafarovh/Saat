using Persistence;
using Application;
using Application.Services;
using Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Servisl?r
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

// CORS siyas?ti
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAdminPanel", policy =>
    {
        policy.WithOrigins("https://localhost:7016") // AdminPanel (MVC) portu
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

// CORS t?tbiqi burda olmalýdýr
app.UseCors("AllowAdminPanel");

app.UseAuthorization();

app.MapControllers();

app.Run();
