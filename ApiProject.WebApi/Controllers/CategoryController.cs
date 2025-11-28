using ApiProject.BusinessLayer.Abstract;
using ApiProject.DtoLayer.CategoryDtos;
using ApiProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult CategoryList()
        {
            var values = _categoryService.TGetAll();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDto createCategoryDto)
        {
            Category category = new Category();
            category.CategoryName= createCategoryDto.CategoryName;
            _categoryService.TInsert(category);
            return Ok("ekleme başarılı");
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            _categoryService.TDelete(id);
            return Ok("silme başarılı");
        }

        [HttpGet("GetCategory")]

        public IActionResult GetById(int id) 
        {
            var value=_categoryService.TGetById(id);
            return Ok(value);
        }

        [HttpPut]
        public IActionResult CategoryUpdate(UpdateCategoryDto updateCategoryDto)
        {
            Category category = new Category();
            _categoryService.TUpdate(category);
            category.CategoryId = updateCategoryDto.CategoryId;
            category.CategoryName = updateCategoryDto.CategoryName;
            return Ok("güncelleme başarılı");
        }

        [HttpGet("CategoryCount")]

        public IActionResult CategoryCount()
        {
            return Ok(_categoryService.TCategoryCount());
        }
    }
}
