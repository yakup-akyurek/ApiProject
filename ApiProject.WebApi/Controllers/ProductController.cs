using ApiProject.BusinessLayer.Abstract;
using ApiProject.DtoLayer.ProductDtos;
using ApiProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]

        public IActionResult ProductList()
        {
            var values = _productService.TGetAll();
            return Ok(values);
        }

        [HttpPost]

        public IActionResult CreateProduct(CreateProductDto createProductDto)
        {
            Product product = new Product();
            product.ProductName = createProductDto.ProductName;
            product.ProductStock = createProductDto.ProductStock;
            product.ProductPrice = createProductDto.ProductPrice;
            product.CategoryId = createProductDto.CategoryId;
            _productService.TInsert(product);
            return Ok("ekleme başarılı");
        }


        [HttpDelete]

        public IActionResult DeleteProduct(int id)
        {
            _productService.TDelete(id);
            return Ok("işlem başarılı");
        }

        [HttpGet("GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var value = _productService.TGetById(id);
            return Ok("işlem başarılı");
        }

        [HttpPut]

        public IActionResult UpdateProduct(UpdateProductDto updateProductDto)
        {
            Product product =new Product();
            product.ProductId = updateProductDto.ProductId;
            product.ProductName = updateProductDto.ProductName;
            product.ProductStock = updateProductDto.ProductStock;
            product.ProductPrice = updateProductDto.ProductPrice;
            product.CategoryId = updateProductDto.CategoryId;
            _productService.TUpdate(product);
            return Ok("güncelleme başarılı");
        }

    }
}
