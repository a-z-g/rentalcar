using RentalCar.Api.Data.Database;
using RentalCar.Api.Data.Models;
using RentalCar.Api.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMongoDBClient("mongodb");
builder.Services.AddScoped<RentalService>();
builder.Services.AddScoped<RentalDatabase>();

builder.Services.AddOpenApi();


var app = builder.Build();

app.MapDefaultEndpoints();

app.MapPost("/register/pickup", async (PickupRequest pickupRequest, RentalService rentalService, ILogger<Program> logger) =>
{
    try
    {
       var bookingNumber = await rentalService.RegisterPickUp(pickupRequest);
        logger.LogInformation("Rental car with booking number {BookingNumber} registered for pickup at {PickupTime} with registration number {RegistrationNumber}.",
            bookingNumber, pickupRequest.PickupTime, pickupRequest.RegistartionNumber);
        return Results.Ok(bookingNumber);
    }
    catch (Exception e)
    {
        logger.LogError(e, "Error registering rental car pickup for registration number {RegistrationNumber} at {PickupTime}.",
            pickupRequest.RegistartionNumber, pickupRequest.PickupTime);
        return Results.Problem("An error occurred while registering the rental car pickup. Please try again later.");
    }
});

app.MapPost("/register/return", async (ReturnRequest returnRequest, RentalService rentalService, ILogger<Program> logger) =>
{
    try
    {
        // Assuming baseDayRental and baseKmPrice are stored in a configuration or constants
        var baseDayRental = 100; 
        var baseKmPrice = 1; 
        
       var price = await rentalService.RegisterReturn(returnRequest, baseDayRental, baseKmPrice);
        logger.LogInformation("Rental car with booking number {BookingNumber} returned at {ReturnTime}. Total price: {Price}.",
            returnRequest.BookingNumber, returnRequest.ReturnTime, price);
        return Results.Ok("Car with booking number " + returnRequest.BookingNumber + " returned successfully. Total price: " + price);
    }
    catch (Exception e)
    {
        logger.LogError(e, "Error returning rental car with booking number {BookingNumber} at {ReturnTime}.",
            returnRequest.BookingNumber, returnRequest.ReturnTime);
        return Results.Problem("An error occurred while returning the rental car with booking number " + returnRequest.BookingNumber + ". Please try again later.");
    }
});
    

app.Run();

public record PickupRequest(
    string RegistartionNumber,
    RentalCarCategory CarCategory,
    string SocialSecurityNumber,
    DateTime PickupTime,
    int PickupMeterReading);

public record ReturnRequest(
    string BookingNumber,
    DateTime ReturnTime,
    int ReturnMeterReading);