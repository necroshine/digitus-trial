using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Attributes;
using Digitus.Trial.Backend.Api.Interfaces;
using Digitus.Trial.Backend.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Digitus.Trial.Backend.Api.Providers
{
    public class MongoBaseDatabaseProvider<MType> : IDatabaseProvider<MType>, IDisposable where MType : class
    {
        private MongoClient client;
        private IMongoDatabase database;

        protected IMongoDatabase Database { get { return database; } }

        protected IMongoCollection<MType> collection;

        public IMongoCollection<MType> Collection { get { return collection; } }
        public MongoBaseDatabaseProvider(string connectionString)
        {
            MongoUrl mongoUrl = new MongoUrl(connectionString);
            client = new MongoClient(mongoUrl);
            database = client.GetDatabase(mongoUrl.DatabaseName);

            collection = database.GetCollection<MType>(GetCollectionName<MType>());
        }

        public virtual async Task<MType> Add(MType item)
        {
            bool ignoreIdentitySeed = GetIgnoreIdentitySeed();
            if (!ignoreIdentitySeed)
            {
                var prop = (from p in item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            where p.GetCustomAttribute(typeof(IdentityFieldAttribute)) != null
                            select p).FirstOrDefault();

                var identityAttr = prop.GetCustomAttribute(typeof(IdentityFieldAttribute)) as IdentityFieldAttribute;
                if (identityAttr.IdentityFieldType == IdentityFieldTypes.Integer)
                {
                    prop.SetValue(item, GetNextSequence<MType>());
                }
                else
                {
                    prop.SetValue(item, Guid.NewGuid());
                }
            }

            await collection.InsertOneAsync(item);
            return item;
        }

        public virtual async Task<MType> Delete(MType item)
        {
            BsonDocument filter = new BsonDocument("Id", GetIDValue(item));
            await collection.FindOneAndDeleteAsync(filter);
            return item;
        }
        public virtual async Task<MType> DeleteById(int id)
        {
            BsonDocument filter = new BsonDocument("Id", id);
            var item = await collection.FindOneAndDeleteAsync(filter);
            return (MType)item;
        }
        public virtual async Task<MType> GetById(int id)
        {
            BsonDocument filter = new BsonDocument("Id", id);
            var result = await collection.FindAsync(filter);

            return result.FirstOrDefault();
        }

        public virtual async Task<MType> Update(MType item)
        {
            BsonDocument filter = new BsonDocument("Id", GetIDValue(item));
            await Collection.ReplaceOneAsync(filter, item);
            return item;
        }
        public virtual async Task<IList<MType>> GetAllAsync()
        {

            var result = collection.Find(FilterDefinition<MType>.Empty);
            if (result == null)
                return await Task.FromResult(new List<MType>());
            else
                return await Task.FromResult(result.ToList());

        }

        public virtual async Task<bool> AddRange(IList<MType> items)
        {
            foreach (var item in items)
            {
                var prop = (from p in item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            where p.GetCustomAttribute(typeof(IdentityFieldAttribute)) != null
                            select p).FirstOrDefault();
                var identityAttr = prop.GetCustomAttribute(typeof(IdentityFieldAttribute)) as IdentityFieldAttribute;
                if (identityAttr.IdentityFieldType == IdentityFieldTypes.Integer)
                {
                    prop.SetValue(item, GetNextSequence<MType>());
                }
                else
                {
                    prop.SetValue(item, Guid.NewGuid());
                }
            }
            await collection.InsertManyAsync(items);
            return true;
        }

        public virtual async Task<IList<MType>> GetByFilter(dynamic filter)
        {
            BsonDocument query = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(filter);

            var result = await collection.FindAsync(query);
            return result.ToList();
        }

        protected string GetCollectionName<T>()
        {
            string collectionName = "";
            var attr = typeof(T).GetCustomAttributes(typeof(ModelAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                collectionName = (attr as ModelAttribute).CollectionName;
            }
            return collectionName;
        }
        protected bool GetIgnoreIdentitySeed()
        {
            bool ignoreIdentitySeed = false;
            var attr = typeof(MType).GetCustomAttributes(typeof(ModelAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                ignoreIdentitySeed = (attr as ModelAttribute).IgnoreIdentitySeed;
            }
            return ignoreIdentitySeed;
        }
        protected int GetIDValue(MType item)
        {
            return (int)item.GetType().GetProperty("Id").GetValue(item);

        }

        protected int GetNextSequence<T>()
        {
            var idCollectionName = GetCollectionName<T>();
            var collectionName = GetCollectionName<Sequence>();
            var sequenceCollection = Database.GetCollection<Sequence>(collectionName);

            var filter = Builders<Sequence>.Filter.Eq(s => s.SequenceName, idCollectionName);
            var findResult = sequenceCollection.Find(filter);

            var update = Builders<Sequence>.Update.Inc(s => s.SequenceValue, 1);
            if (findResult.CountDocuments() == 0)
            {
                update = Builders<Sequence>.Update.Set(s => s.SequenceValue, GetMaxIdValueOfCollection<T>());
            }

            var result = sequenceCollection.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<Sequence, Sequence> { IsUpsert = true, ReturnDocument = ReturnDocument.After });
            return result.SequenceValue;

        }

        protected int GetMaxIdValueOfCollection<T>()
        {
            var result = collection.Find(FilterDefinition<MType>.Empty).Sort("{Id: -1}").FirstOrDefault();

            if (result == null) return 1;
            var idValue = GetIDValue(result);
            idValue++;
            return idValue;
        }

        public void Dispose()
        {

        }

        public async Task<MType> GetById(Guid id)
        {
            BsonDocument filter = new BsonDocument("Id", id);
            var result = await collection.FindAsync(filter);

            return result.FirstOrDefault();
        }

        public async Task<MType> DeleteById(Guid id)
        {
            BsonDocument filter = new BsonDocument("Id", id);
            var item = await collection.FindOneAndDeleteAsync(filter);
            return (MType)item;
        }
    }
}
