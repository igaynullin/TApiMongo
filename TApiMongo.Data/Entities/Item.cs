using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TApiMongo.Data.Repositories;
using TApiMongo.Interfaces;

namespace TApiMongo.Data.Entities
{
    [MongoCollectionName("ref_Items")]
    public class Item : IHasID<int>, IHasName, IHasDescription, IEntity, IItem
    {
        [BsonId]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public IList<string> Tags { get; set; }
    }
}