var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithMongoExpress();

var mongoDb = mongo.AddDatabase("mongodb");

var rentalCarApi = builder.AddProject<Projects.RentalCar_Api>("api")
    .WithHttpHealthCheck("/health")
    .WithReference(mongoDb)
    .WaitFor(mongoDb);

builder.AddProject<Projects.RentalCar_ConsoleApp>("console")
    .WithReference(mongoDb) // This links it to the same MongoDB as the API
    .WaitFor(mongoDb);

builder.Build().Run();