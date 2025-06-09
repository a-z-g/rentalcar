using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RentalCar.Api.Data.Database;
using RentalCar.Api.Data.Models;
using RentalCar.Api.Modules;

var services = new ServiceCollection();

var mongoConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__mongodb");
services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("rentalcar");
});

services.AddScoped<RentalDatabase>();
services.AddScoped<RentalService>();

var serviceProvider = services.BuildServiceProvider();
var rentalService = serviceProvider.GetRequiredService<RentalService>();

var categories = new[]
{
    RentalCarCategory.Small,
    RentalCarCategory.Combi,
    RentalCarCategory.Truck
};

foreach (var category in categories)
{
    Console.WriteLine($"--- Testing category: {category} ---");

    var baseDayRental = 100;
    var baseKmPrice = 1;
    
    var pickupRequest = new PickupRequest(
        RegistartionNumber: $"REG-{category}",
        CarCategory: category,
        SocialSecurityNumber: "12345678901",
        PickupTime: DateTime.UtcNow,
        PickupMeterReading: 10000
    );

    var bookingNumber = await rentalService.RegisterPickUp(pickupRequest);
    Console.WriteLine($"Booking number: {bookingNumber}");

    var returnRequest = new ReturnRequest(
        BookingNumber: bookingNumber,
        ReturnTime: DateTime.UtcNow.AddDays(2),
        ReturnMeterReading: 10500
    );

    var price = await rentalService.RegisterReturn(returnRequest, baseDayRental, baseKmPrice);
    Console.WriteLine($"Calculated price: {price}");
    
    var days = 2;
    var km = 500;

    var expectedPrice = category switch
    {
        RentalCarCategory.Small => baseDayRental * days, 
        RentalCarCategory.Combi => baseDayRental * days * 1.3m + baseKmPrice * km,
        RentalCarCategory.Truck => baseDayRental * days * 1.5m + baseKmPrice * km * 1.5m,
        _ => throw new InvalidOperationException("Unknown category")
    };

    Console.WriteLine($"Expected price: {expectedPrice}");

    Console.WriteLine(price != expectedPrice
        ? $"❌ Price for {category} mismatch! Expected: {expectedPrice}, Got: {price}"
        : $"✅ Price for {category} matches expected value.");
}

Console.WriteLine("All categories tested");