using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Scheduling.Infrastructure.MongoDb;

public static class MongoDbExtensions
{
    public static IMongoCollection<T> For<T>(this IMongoDatabase database) => database.GetCollection<T>(typeof(T).Name);

    // public static Task<T> LoadDocument<T>(this IMongoCollection<T> collection, string id, CancellationToken cancellationToken = default)
    //     where T : IDocument
    //     => collection.Find(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
    //
    // public static Task<T> LoadDocument<T>(this IMongoDatabase database, string id, CancellationToken cancellationToken = default)
    //     where T : IDocument
    //     =>  database.For<T>().Find(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
    //
    // public static Task ReplaceDocument<T>(this IMongoCollection<T> collection, T document, CancellationToken cancellationToken = default)
    //     where T : IDocument
    //     => collection.ReplaceOneAsync(Builders<T>.Filter.Eq(x => x.Id, document.Id), document, cancellationToken: cancellationToken);
    //
    // public static Task<UpdateResult> UpdateDocument<T>(
    //     this IMongoDatabase database,
    //     Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> filter,
    //     Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> update,
    //     CancellationToken cancellationToken = default
    // ) where T : IDocument
    //     => database.For<T>().UpdateDocument(filter, update, cancellationToken);
    //
    // public static Task UpdateManyDocuments<T>(
    //     this IMongoDatabase database,
    //     Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> filter,
    //     Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> update,
    //     CancellationToken cancellationToken = default
    // ) where T : IDocument
    //     => database.For<T>().UpdateManyDocuments(filter, update, cancellationToken);
    //
    // public static Task<UpdateResult> UpdateDocument<T>(
    //     this IMongoCollection<T> collection,
    //     Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> filter,
    //     Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> update,
    //     CancellationToken cancellationToken = default
    // ) where T : IDocument
    // {
    //     var options = new UpdateOptions {IsUpsert = true};
    //
    //     return collection.UpdateOneAsync(filter(Builders<T>.Filter), update(Builders<T>.Update), options, cancellationToken);
    // }
    //
    // public static Task UpdateManyDocuments<T>(
    //     this IMongoCollection<T> collection,
    //     Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> filter,
    //     Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> update,
    //     CancellationToken cancellationToken = default
    // ) where T : IDocument
    // {
    //     var options = new UpdateOptions {IsUpsert = true};
    //
    //     return collection.UpdateManyAsync(filter(Builders<T>.Filter), update(Builders<T>.Update), options, cancellationToken);
    // }
    //
    // public static Task DeleteDocument<T>(this IMongoDatabase database, string id) where T : IDocument
    //     => database.For<T>().DeleteOneAsync(Builders<T>.Filter.Eq(x => x.Id, id));
}

public interface IDocument
{
    public string Id { get; init; }
}