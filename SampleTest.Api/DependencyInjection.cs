using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SampleTest.Api.Repositories;
using SampleTest.Api.Settings;

namespace SampleTest.Api
{
    public static class DependencyInjection
    {
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // instruct mongodb on how to serialize data types like Guid and DateTimeOffset
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        // Anytime it sees a Guid in any of the entities, it serializes them to a string
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
        var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

        services.AddSingleton<IMongoClient>(sp=>
        {
            // var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            return new MongoClient(settings.ConnectionString);
        });

        

        services.AddHealthChecks()
        .AddMongoDb(settings.ConnectionString, name: "mongodb", timeout:TimeSpan.FromSeconds(5),
        tags: new[] {"ready", "alive"});

        // So we pass our mongodb credentials to the healthcheck so that it can also check for it while giving it a specified name and a preset timeout, so that it doesn't take forever to check


        // services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(configuration.GetConnectionString("MongoDbSettings")));

        // services.AddSingleton<IMongoClient, MongoClient>(sp =>
        //    new MongoDbSettings(configuration.GetSection("MongoDbSettings:ConnectionString").Value,
        //    configuration.GetSection("MongoDbSettings:DatabaseName").Value));
        
        services.AddSingleton<IInMemItemsRepository, MongoDbItemsRepository>();
        // services.AddSingleton<IInMemItemsRepository, inMemItemsRepository>();
        return services;
    }

    }
}