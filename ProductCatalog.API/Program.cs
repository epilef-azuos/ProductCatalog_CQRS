using Microsoft.EntityFrameworkCore;
using ProductCatalog.API.Filters;
using ProductCatalog.CrossCutting.AppDependencies;

var builder = WebApplication.CreateBuilder(args);

// Registro dos serviços de infraestrutura (Banco, Repositórios, Unit of Work)
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(new CustomExceptionFilter());
});

var app = builder.Build();

// Aplicar migrações automaticamente no arranque
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProductCatalog.Infrastructure.Context.AppDbContext>();

    // Isto irá criar a base de dados e aplicar todas as migrações se não existirem
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();