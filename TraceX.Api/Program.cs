using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TraceX.Api.Exceptions;
using TraceX.Application.Validators;
using TraceX.Domain.Interfaces;
using TraceX.Infrastructure.Data;
using TraceX.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TraceXDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TraceXDbConnection")));

builder.Services.AddScoped<IMachineRepository, MachineRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMachineDtoValidator>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
