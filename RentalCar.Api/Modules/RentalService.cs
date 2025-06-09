using RentalCar.ApiS.Data.Database;
using RentalCar.ApiS.Data.Models;

namespace RentalCar.ApiS.Modules;

public class RentalService(RentalDatabase database)
{
    public async Task<Rental> GetRentalAsync(string bookingNumber) => await database.GetRental(bookingNumber);

    public async Task RegisterPickUp(PickUpRequest pickUp)
    {
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            BookingNumber = await GenerateBookingNumber(),
            RegistrationNumber = pickUp.RegistartionNumber,
            RentalCarCategory = pickUp.CarCategory,
            CustomerSocialSecurityNumber = pickUp.SocialSecurityNumber,
            TimeOfPickup = pickUp.PickupTime,
            KmAtPickup = pickUp.PickupMeterReading,
            CreatedAt = DateTime.Now
        };
        
        
    }

    private async Task<string> GenerateBookingNumber()
    {
        // Simplified logic: get last booking number
        var lastRental = await database.GetLastRentalAsync();
        var lastNumber = lastRental != null 
            ? int.Parse(lastRental.BookingNumber.Replace("B", ""))
            : 0;
        var newNumber = lastNumber + 1;
        return $"B{newNumber}";
    }
    
}