using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TApiMongo.Data.Entities;
using TApiMongo.Data.Repositories;
using TApiMongo.Test;
using TApiMongo.Web.Api.Controllers;
using TApiMongo.Web.ViewModels.Items;

namespace TApiMongo.Web.Api.Test
{
    [TestClass]
    public class ItemsControllerTests
    {
        private readonly ItemRepository _itemRepository;
        public readonly Mock<IItemRepository> _mockProductRepository;
        private readonly IDataProtector _protector;
        private ItemsController _itemsController;

        public ItemsControllerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();
            _protector = services.GetService<IDataProtectionProvider>().CreateProtector("MyDataProtector");
            _mockProductRepository = new Mock<IItemRepository>();
            _itemRepository = new ItemRepository(DataItems.ConnectionString);
            _itemsController = new ItemsController(_itemRepository, _protector);
        }

        [TestMethod]
        public void Get_Instant_ReturnInternalServerError_When_Repository_Null()
        {
            var controller = new ItemsController(null, _protector);
            var actionResult = controller.Get();
            Assert.IsNotNull(actionResult);
            var result = actionResult as StatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [TestMethod]
        public void Get_Instant_ReturnInternalServerError_When_Protector_Null()
        {
            _mockProductRepository.Setup(mr => mr.GetAll()).Returns(DataItems.Data);
            var controller = new ItemsController(_mockProductRepository.Object, null);
            var actionResult = controller.Get();
            Assert.IsNotNull(actionResult);
            var result = actionResult as StatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [TestMethod]
        public void Get_Instant_ReturnNotFound_When_Data_Null()
        {
            IEnumerable<Item> data = null;
            _mockProductRepository.Setup(mr => mr.GetAll()).Returns(data);

            var controller = new ItemsController(_mockProductRepository.Object, _protector);
            var actionResult = controller.Get();
            Assert.IsNotNull(actionResult);
            var result = actionResult as NotFoundObjectResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void Get_Instant_ReturnOk()
        {
            _mockProductRepository.Setup(mr => mr.GetAll()).Returns(DataItems.Data);
            var controller = new ItemsController(_mockProductRepository.Object, _protector);

            var actionResult = controller.Get();
            Assert.IsNotNull(actionResult);
            var result = actionResult as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var data = result.Value as List<List>;
            Assert.AreEqual(DataItems.Data.Count, data.Count);
        }

        [TestMethod]
        public void Get_Instant_ReturnOk_ProtectedID()
        {
            _mockProductRepository.Setup(mr => mr.GetAll()).Returns(DataItems.Data);
            var controller = new ItemsController(_mockProductRepository.Object, _protector);

            var actionResult = controller.Get();
            Assert.IsNotNull(actionResult);
            var result = actionResult as OkObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            var data = result.Value as IEnumerable<List>;
            Assert.AreEqual(Convert.ToInt32(_protector.Unprotect(data.ElementAt(0).ID)), DataItems.Data[0].ID);
        }
    }
}