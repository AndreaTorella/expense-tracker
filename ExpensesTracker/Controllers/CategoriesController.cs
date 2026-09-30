using AutoMapper;
using ExpensesTracker.Application.Models;
using ExpensesTracker.Application.Services;
using ExpensesTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;

        public CategoriesController(
            IMapper mapper,
            ICategoryService categoryService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return Ok(this.mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(this.mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult> AddCategoryAsync(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest();
            }

            var createCategoryCommand = this.mapper.Map<CreateCategoryCommand>(categoryDto);
            var result = await categoryService.AddCategoryAsync(createCategoryCommand);

            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            var isDeleted = await categoryService.DeleteCategoryAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
