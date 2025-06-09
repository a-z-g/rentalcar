using MongoDB.Driver;
using MongoDB.Driver.Linq;

using RentalCar.Api.Data.Models;

namespace RentalCar.Api.Data.Database;

public class RentalDatabase(IMongoDatabase database)
{
    private readonly IMongoCollection<Rental> _rentalsCollection = database.GetCollection<Rental>("rentals");

    public async Task<Rental> GetRental(string bookingNumber) =>
        await _rentalsCollection
            .AsQueryable()
            .SingleOrDefaultAsync(x => x.BookingNumber == bookingNumber)
            .ConfigureAwait(false);

    public async Task<Rental> GetLastRentalAsync() =>  
        await _rentalsCollection
            .AsQueryable()
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        

public async Task UpsertRental(Rental rentalDocument)
    {
        var filter = Builders<Rental>.Filter.Eq(x => x.Id, rentalDocument.Id);
        var options = new ReplaceOptions() { IsUpsert = true };
        
        rentalDocument.LastModifiedAt = DateTime.Now;
        await _rentalsCollection.ReplaceOneAsync(filter, rentalDocument, options).ConfigureAwait(false);
    } 

    public async Task EnsureIndexesAsync()
    {
        var indexKeys = Builders<Rental>.IndexKeys.Ascending(r => r.BookingNumber);
        var indexModel = new CreateIndexModel<Rental>(indexKeys, new CreateIndexOptions { Unique = true });
        await _rentalsCollection.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false);
    }
}