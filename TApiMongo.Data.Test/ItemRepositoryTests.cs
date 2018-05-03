using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TApiMongo.Data.Entities;
using TApiMongo.Data.Repositories;
using TApiMongo.Test;

namespace TApiMongo.Data.Test
{
    [TestClass]
    public class ItemRepositoryTests
    {
        public readonly IItemRepository _mockProductRepository;
        private IItemRepository _itemRepository;

        public ItemRepositoryTests()
        {
            var mockProductRepository = new Mock<IItemRepository>();
            mockProductRepository.Setup(mr => mr.GetAll()).Returns(DataItems.Data);
            mockProductRepository.Setup(mr => mr.SavePartial(It.IsAny<Item>())).Callback(
               (Item entity) =>
               {
                   var source = DataItems.Data.FirstOrDefault(e => e.ID == entity.ID);
                   if (!string.IsNullOrEmpty(entity.Description) && source.Description != entity.Description)
                   {
                       source.Description = entity.Description;
                   }
                   if (!string.IsNullOrEmpty(entity.Name) && source.Name != entity.Name)
                   {
                       source.Name = entity.Name;
                   }
                   var sourceTags = string.Join(",", source.Tags);
                   var entityTags = string.Join(",", entity.Tags);
                   if (!string.IsNullOrEmpty(entityTags) && sourceTags != entityTags)
                   {
                       source.Tags = entityTags.Split(",");
                   }
               });
            mockProductRepository.Setup(mr => mr.Add(It.IsAny<Item>())).Callback(
                (Item target) =>
                {
                    DataItems.Data.Add(target);
                });
            _mockProductRepository = mockProductRepository.Object;
            _itemRepository = new ItemRepository(connectionString: DataItems.ConnectionString);
        }

        [TestMethod]
        public void Exists_Items_In_Collection()
        {
            Assert.AreNotEqual(_itemRepository.GetAll().Count(), 0);
        }

        [TestMethod]
        public void Can_Select_Item_By_ID()
        {
            Assert.AreNotEqual(_itemRepository.SingleOrDefault(e => e.ID == 1)?.ID, 1);
        }

        [TestMethod]
        public void Can_UpdatePartial()
        {
            var entity = new Item
            {
                ID = 10,
                Description = "Описание 10",
                Name = "Объект 10_",
                Tags = new List<string>
                {
                    "Первый tag_",
                    "Второй tag",
                }
            };
            _mockProductRepository.SavePartial(entity);

            Assert.AreEqual(entity.Name, DataItems.Data[9].Name);
        }

        [TestMethod]
        public void Can_Insert()
        {
            var newProduct = new Item
            { ID = 11, Name = "test name 11", Description = "Short description here" };

            int productCount = _mockProductRepository.GetAll().Count();
            Assert.AreEqual(10, productCount);

            _mockProductRepository.Add(newProduct);

            productCount = _mockProductRepository.GetAll().Count();
            Assert.AreEqual(11, productCount);

            var testProduct = _mockProductRepository.GetAll().FirstOrDefault(e => e.ID == 11);
            Assert.IsNotNull(testProduct);
            Assert.IsInstanceOfType(testProduct, typeof(Item));
            Assert.AreEqual(11, testProduct.ID);
        }

        [TestMethod]
        public void Can_Update()
        {
            // Find a product by id
            var testProduct = _mockProductRepository.GetAll().FirstOrDefault(e => e.ID == 1);

            // Change one of its properties
            testProduct.Name = "Renamed 1";

            // Save our changes.
            _mockProductRepository.SavePartial(testProduct);

            // Verify the change
            Assert.AreEqual("Renamed 1", _mockProductRepository.GetAll().FirstOrDefault(e => e.ID == 1)?.Name);
        }
    }
}