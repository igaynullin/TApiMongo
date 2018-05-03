using System.Collections.Generic;

namespace TApiMongo.Data.Entities
{
    public interface IItem
    {
        string Description { get; set; }

        string Name { get; set; }
        IList<string> Tags { get; set; }
    }
}