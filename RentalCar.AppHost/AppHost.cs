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

builder.Build().Run();