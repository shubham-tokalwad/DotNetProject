using System;
using System.Collections.Generic;
using System.Web.Http;
using refactor_me.DataAccessLibraray;
using log4net;
using System.Threading.Tasks;
using AutoMapper;
using refactor_me.Models.DTOs;
using refactor_me.Models;
using refactor_me.Middleware;
using System.Linq;
using refactor_me.DataAccessLibraray.Interface;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionRepository;
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductsController));


        public ProductsController(IProductRepository productRepository, IProductOptionRepository productOptionRepository)
        {
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
        }

        //[Authorize] //valid login or login user
        [Route("getProducts")]
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            try
            {
                var products = await _productRepository.LoadProductsAsync("");
                log.Info("Products retrieved successfully}");
                return Mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while retrieving products.", ex);
                //return errormessage and statuscode can put in dto
                throw;
            }
        }

        [Route("searchByName/{name}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByName(string name)
        {
            try
            {
                if (String.IsNullOrEmpty(name))
                    return BadRequest("Invalid product name.");
                
                var product = await _productRepository.LoadProductsAsync(name);
                if (product.FirstOrDefault() == null)
                {
                    log.Error("Product not found.");
                    throw new CustomException(404, "Product not found.");
                }
                log.Info("Product retrieved successfully: {name}");
                return Ok(Mapper.Map<ProductDTO>(product.FirstOrDefault()));
            }
            catch(Exception ex)
            {
                log.Error("Error retrieving product with Name: {name}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }


        }

        [Route("getProductById/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductById(Guid? id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid product ID.");

                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null || product.IsNew)
                {
                    log.Error("Product not found.");
                    return NotFound();
                } 

                //log.Info("Product retrieved successfully: {ProductId}", id);
                log.Info("Product retrieved successfully: {id}");
                var productDTO = Mapper.Map<ProductDTO>(product);
                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                log.Error("Error retrieving product with ID: {id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }

        }

        [Route("createProduct")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateProduct(ProductDTO product)
        {
            try 
            { 
                if (product == null)
                    return BadRequest("Product cannot be empty.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _productRepository.SaveOrUpdateProductAsync(Mapper.Map<Product>(product));
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Error creating product with ID: {product.id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }
        }

        [Route("updateProduct/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct(Guid id, ProductDTO product)
        {
            //var orig = new Product(id)
            //{
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    DeliveryPrice = product.DeliveryPrice
            //};

            //if (!orig.IsNew)
            //    orig.Save();
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid product ID.");
                if (product == null)
                    return BadRequest("Product cannot be empty.");
                //if(product.Id == Guid.Empty)
                //    return BadRequest("Invalid product ID.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                product.Id = id;
                await _productRepository.SaveOrUpdateProductAsync(Mapper.Map<Product>(product));
                return Ok();
            }
            catch(Exception ex)
            {
                log.Error("Error updating product with ID: {product.id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }

        }

        [Route("deleteProduct/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct(Guid id)
        {

            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid product ID.");

                //var product = new Product(id);
                await _productRepository.DeleteProductAsync(id);
                return Ok();
            }
            catch(Exception ex)
            {
                log.Error("Error deleting product with ID: {id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }
        }

        [Route("getProductOptions/{productId}/options")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductOptions(Guid productId)
        {
            try
            {
                if (productId == Guid.Empty)
                    return BadRequest("Invalid product ID.");

                //return new ProductOptions(productId);
                var productOptions = await _productOptionRepository.GetOptionsAsync(productId);
                log.Info("Product options retrieved successfully}");
                return Ok(Mapper.Map<IEnumerable<ProductOptionDTO>>(productOptions));
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while retrieving product options.", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }

        }

        [Route("getProductOptionById/{productId}/options/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductOptionById(Guid id)//Guid productId, 
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid ID.");

                var option = await _productOptionRepository.GetOptionAsync(id);
                if (option == null || option.IsNew)
                    return NotFound();

                log.Info("Product retrieved successfully: {id}");
                return Ok(Mapper.Map<ProductOptionDTO>(option));
            }
            catch(Exception ex)
            {
                log.Error("Error retrieving product option with ID: {id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }

        }

        [Route("createProductOption/{productId}/options")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateProductOption(Guid productId, ProductOptionDTO option)
        {
            try
            {
                if (productId == Guid.Empty)
                    return BadRequest("Invalid product ID.");
                if (option == null)
                    return BadRequest("ProductOption cannot be empty.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                option.ProductId = productId;

                await _productOptionRepository.CreateOrUpdateOptionAsync(Mapper.Map<ProductOption>(option));
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Error creating product option with ID: {option.id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }

        }

        [Route("updateProductOption/{productId}/options/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProductOption(Guid productId, ProductOptionDTO option)
        {
            try
            {
                if (productId == Guid.Empty)
                    return BadRequest("Invalid product ID.");
                if (option == null)
                    return BadRequest("ProductOption cannot be empty.");
                //if(option.Id == Guid.Empty)
                //    return BadRequest("Invalid option ID.");
                option.ProductId = productId;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _productOptionRepository.CreateOrUpdateOptionAsync(Mapper.Map<ProductOption>(option));
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Error updating product with ID: {option.id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }
            //var orig = new ProductOption(id)
            //{
            //    Name = option.Name,
            //    Description = option.Description
            //};

            //if (!orig.IsNew)
            //    orig.Save();

        }

        [Route("deleteProductOption/{productId}/options/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProductOption(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invalid option ID.");

                await _productOptionRepository.DeleteOptionAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error("Error deleting option with ID: {id}", ex);
                throw new CustomException(500, "Either invalid data or internal server error.");
            }
            //var opt = new ProductOption(id);
            //opt.Delete();

        }
    }
}
