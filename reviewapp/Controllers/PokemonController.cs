using AutoMapper;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using reviewapp.Dto;
using reviewapp.Interfaces;
//using reviewapp.Repository;
using reviewapp.Model;
using System.Numerics;

namespace reviewapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;
        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;   

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet ("{pokeId}")]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound(); 

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);

        }

        [HttpGet("{pokeId}/rating")]
        public IActionResult GetPokemonRating (int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating =_pokemonRepository.GetPokemonRating(pokeId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);  
        }

        [HttpPost]
        public IActionResult CreatePokemon([FromQuery] int ownerId,  [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate)
        {    if(pokemonCreate == null)
                return BadRequest(ModelState);

            //this mapper is realy not important for now, i only implement or make use of it because i am following a tutorial.
            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
            
            if(!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)) 
                return BadRequest(ModelState);

            return Ok($"Created pokemon {pokemonCreate.Name.ToUpper()} Successfully !" );

        }

        [HttpPut ("{pokeId}")]
        public IActionResult UpdatePokemon(int pokeId,[FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto updatePokemon)
        {
            if (pokeId == null)
                return BadRequest();
            if(updatePokemon == null)
                return BadRequest(ModelState);
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound("Pokemon does not exist");
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokeMap = _mapper.Map<Pokemon>(updatePokemon);
            if(!_pokemonRepository.UpdatePokemon(ownerId, catId, pokeMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{pokeId}")]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var reviewToDelete = _reviewRepository.GetReviewsOfAPokemon(pokeId);
            var pokeToDelete = _pokemonRepository.GetPokemon(pokeId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.DeleteReviews(reviewToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            if (!_pokemonRepository.DeletePokemon(pokeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this!");
                //return BadRequest(ModelState);
            }
            
            return Ok($"Review Deleted");
        }
    }
}