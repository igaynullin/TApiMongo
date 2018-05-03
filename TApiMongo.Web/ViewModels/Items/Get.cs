using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TApiMongo.Data.Entities;
using TApiMongo.Interfaces;

namespace TApiMongo.Web.ViewModels.Items
{
    public class Get : IItem, IHasID<string>
    {
        public string Description { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public IList<string> Tags { get; set; }
    }
}