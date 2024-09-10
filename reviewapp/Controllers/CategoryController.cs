using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewapp.Interfaces;
using reviewapp.Dto;
using reviewapp.Model;

namespace reviewapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController: ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper ) 
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;   
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetCategory(int id)
        {
            if(!_categoryRepository.CategoryExists(id)) 
                return NotFound();

            var category = _categoryRepository.GetCategory(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        public IActionResult GetPokemonByCategoryId( int categoryId)
        {
            var pokemon =  _categoryRepository.GetPokemonByCategory(categoryId);
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);  
            return Ok(pokemon);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto createCategory)
        {
            if(createCategory == null)  
                return BadRequest(ModelState);
            var categegory = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == createCategory.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(categegory != null)
            {
                ModelState.AddModelError("", "This category exist already");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(createCategory);
            if(!_categoryRepository.CreateCategory(categoryMap))
                return BadRequest(ModelState);
            return Ok(" Catergory Created");
        }

        [HttpPut ("{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatecategory)
        {

            if(updatecategory == null)
                return BadRequest(ModelState);  
            if(categoryId != updatecategory.Id)
                return BadRequest(ModelState);
            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound("Category does not exist");
            if (!ModelState.IsValid)
                return BadRequest();
            var categoryMap = _mapper.Map<Category>(updatecategory);
            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Somethig went wrong updating this category");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        [HttpDelete ("{categoryId}")]
        public IActionResult DeleteCategoryt(int categoryId)
        {
            if(!_categoryRepository.CategoryExists(categoryId))
                return BadRequest(ModelState);
            var categoryToDelete = _categoryRepository.GetCategory(categoryId);
            if (categoryToDelete == null)
                return NotFound();
            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this category");
                return BadRequest(ModelState);
            }
            return Ok("Ccategory has been deleted");
        }

    }
}
