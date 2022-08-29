using Elastic.Apm.NetCoreAll;
using PocElasticSearch.API.Infrastructure.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

SerilogExtensions.ConfigureLogging(builder.Configuration);

builder.Host.UseSerilog(Log.Logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAllElasticApm(app.Configuration);

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
