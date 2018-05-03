using System;

namespace TApiMongo.Data
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class MongoCollectionName : Attribute
    {
        public MongoCollectionName(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}