
using EventStore_Product_Application;
using EventStore_Shared.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EventStoreBackgroundService>();

builder.Services.AddSingleton<IEventStoreService, EventStoreService>();
builder.Services.AddSingleton<IMongoDBService, MongoDBService>();

var host = builder.Build();
host.Run();