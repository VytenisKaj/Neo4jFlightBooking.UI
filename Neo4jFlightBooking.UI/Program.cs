using Neo4jFlightBooking.Infrstructure;
using Neo4jFlightBooking.UI.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connection = builder.Configuration.GetValue<string>("Neo4jConnection");
Neo4jClient client = new Neo4jClient(connection);

builder.Services.AddSingleton<Neo4jClient>(client);

builder.Services.AddScoped<Neo4jDatabaseService>(_ => new Neo4jDatabaseService(client));
builder.Services.AddScoped<Neo4jController>(_ => new Neo4jController(client));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
