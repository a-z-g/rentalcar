using RentalCar.ApiS.Data.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

app.Run();

public record PickUpRequest(
    string RegistartionNumber,
    RentalCarCategory CarCategory,
    string SocialSecurityNumber,
    DateTime PickupTime,
    int PickupMeterReading);

public record ReturnRequest(
    string BookingNumber,
    DateTime ReturnTime,
    int ReturnMeterReading);