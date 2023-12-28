using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using refactor_me.DataAccessLibraray;
using refactor_me.DataAccessLibraray.Interface;
using refactor_me.Models;
using refactor_me.Models.DTOs;
using refactor_this.Controllers;

namespace refactor_me.Tests
{
    [TestClass]
    public class ProductTest
    {
        private Mock<IProductRepository> productRepositoryMock;
        private Mock<IProductOptionRepository> productOptionRepositoryMock;
        private static bool isMapperInitialized = false;



        [TestInitialize]
        public void Initialize()
        {
            if (!isMapperInitialized)
            {
                Mapper.Initialize(cfg => {
                    cfg.CreateMap<ProductDTO,Product>();
                    cfg.CreateMap<ProductOptionDTO, ProductOption>();
                });

                isMapperInitialized = true;
            }

            productRepositoryMock = new Mock<IProductRepository>();
            productOptionRepositoryMock = new Mock<IProductOptionRepository>();
        }

        [TestMethod]
        public async Task GetProducts_ShouldReturnProducts()
        {

            var productsController = new ProductsController(productRepositoryMock.Object, productOptionRepositoryMock.Object);

            var mockProducts = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product1", Price = 10.0m, DeliveryPrice = 2.0m },
            new Product { Id = Guid.NewGuid(), Name = "Product2", Price = 15.0m, DeliveryPrice = 3.0m }
        };

            productRepositoryMock.Setup(repo => repo.LoadProductsAsync(It.IsAny<string>()))
                .ReturnsAsync(mockProducts);

            
            var result = await productsController.GetProducts();

            
            Assert.IsNotNull(result);
            var products = result as IEnumerable<ProductDTO>;
            Assert.IsNotNull(products);
            Assert.AreEqual(2, products.Count()); 
        }

        [TestMethod]
        public async Task CreateProduct_ShouldReturnOk()
        {
           
            var productController = new ProductsController(productRepositoryMock.Object, productOptionRepositoryMock.Object);
            var productDTO = new ProductDTO
            {
                Name = "New Product",
                Description = "Description for the new product",
                Price = 19.99m,
                DeliveryPrice = 5.99m
            };

            IHttpActionResult actionResult = await productController.CreateProduct(productDTO);

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldReturnOk()
        {
            var productController = new ProductsController(productRepositoryMock.Object, productOptionRepositoryMock.Object);
            var productId = Guid.NewGuid();

            IHttpActionResult actionResult = await productController.DeleteProduct(productId);

            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }
    }
}
