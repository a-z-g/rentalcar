using System.Runtime.InteropServices.JavaScript;

namespace RentalCar.Api.Data.Models;


    public enum RentalCarCategory
    {
        Small,
        Combi,
        Truck
    }

    public record Rental
    {
        public Guid Id { get; init; }
        public string BookingNumber { get; init; } = string.Empty;
        public RentalCarCategory RentalCarCategory { get; init; }
        public string RegistrationNumber { get; init; } = string.Empty;
        public string CustomerSocialSecurityNumber { get; init; } = string.Empty;
        
        public DateTime TimeOfPickup { get; init; }
        public int KmAtPickup { get; init; }
        
        public DateTime TimeOfReturn { get; set; }
        public int KmAtReturn { get; set; }

        public DateTime CreatedAt{ get; init; }
        public DateTime LastModifiedAt{ get; set; }
    }

