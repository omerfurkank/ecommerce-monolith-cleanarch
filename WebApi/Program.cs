using Application;
using Carter;
using Infrastructure;
using System.Diagnostics;
using Scalar.AspNetCore;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWeb();
builder.Services.AddOpenApi();
builder.Services.AddCors();

var app = builder.Build();


app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHttpsRedirection();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var url = "https://localhost:7143/scalar";
    Process.Start(new ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
});
app.Run();

