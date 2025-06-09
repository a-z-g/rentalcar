using RentalCar.Api.Data.Database;
using RentalCar.Api.Data.Models;

namespace RentalCar.Api.Modules;

public class RentalService(RentalDatabase database)
{
    private const decimal CombiMultiplier = 1.3m;
    private const decimal TruckMultiplier = 1.5m;
    
    public async Task<string> RegisterPickUp(PickupRequest pickUp)
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

       await database.UpsertRental(rental);
       
       return rental.BookingNumber;
    }

    public async Task<decimal> RegisterReturn(ReturnRequest returnRequest, decimal baseDayRental, decimal baseKmPrice)
    {
        var rental = await database.GetRental(returnRequest.BookingNumber);

        if (rental == null)
        {
            throw new Exception($"Rental with booking number {returnRequest.BookingNumber} not found");
        }
        
        rental.KmAtReturn = returnRequest.ReturnMeterReading;
        rental.TimeOfReturn = returnRequest.ReturnTime;
        
        await database.UpsertRental(rental);

        var price = CalculatePrice(rental, baseDayRental, baseKmPrice);
        
        return price;
    }

    private static decimal CalculatePrice(Rental rental, decimal baseDayRental, decimal baseKmPrice)
    {
        var days = (rental.TimeOfReturn - rental.TimeOfPickup).Days;
        
        if (days == 0)
        {
            days = 1;
        }
        
        var totalKm = rental.KmAtReturn - rental.KmAtPickup;

        return rental.RentalCarCategory switch
        {
            RentalCarCategory.Small => baseDayRental * days,
            RentalCarCategory.Combi => baseDayRental * days * CombiMultiplier + totalKm * baseKmPrice,
            RentalCarCategory.Truck => baseDayRental * days * TruckMultiplier + totalKm * baseKmPrice * TruckMultiplier,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    

    private async Task<string> GenerateBookingNumber()
    {
        var lastRental = await database.GetLastRentalAsync();

        var lastNumber = 0;

        if (lastRental != null)
        {
            lastNumber = int.Parse(lastRental.BookingNumber.Substring(1)); 
        }
        
        var newNumber = lastNumber + 1;
        
        return $"B{newNumber}";
    }
    
}