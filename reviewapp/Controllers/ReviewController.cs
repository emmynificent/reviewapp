using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewapp.Dto;
using reviewapp.Interfaces;
using reviewapp.Model;
using reviewapp.Repository;
using System.Runtime.CompilerServices;

namespace reviewapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepositoy _reviewerRepository;
        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IReviewerRepositoy reviewerRepository, IPokemonRepository pokemonRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _reviewerRepository = reviewerRepository ;
            _pokemonRepository = pokemonRepository;
        }

        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _reviewRepository.GetReviews();
            return Ok(reviews);
        }
        [HttpGet("reviewId")]
        public IActionResult GetReview(int id)
        {
            if (!_reviewRepository.ReviewExists(id))
                return NotFound();
            var review = _reviewRepository.GetReview(id);
            return Ok(review);
        }
        [HttpGet("review/{pokemonId}")]
        public IActionResult GetReviewByPokemon(int pokemonId)
        {
            var pokemon = _reviewRepository.GetReviewsOfAPokemon(pokemonId);
            if (!ModelState.IsValid)
                return NotFound();
            return Ok(pokemon);
        }


        [HttpPost]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto review)
        {

            if (review == null)
                return BadRequest();
            var reviews = _reviewRepository.GetReviews().Where(r => r.Title.Trim().ToUpper() == review.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(review);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            
            if (!_reviewRepository.CreateReview(reviewMap))
                return NotFound();
            return Ok("Review created successfully");
        }
        [HttpPut ("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updateReview)
        {
            if (updateReview == null)
                return BadRequest(ModelState);

            if(reviewId != updateReview.Id)
                return BadRequest(ModelState);

            if(!_reviewRepository.ReviewExists(reviewId))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState );

            var reviewMap = _mapper.Map<Review>(updateReview);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "An Error occured updating this review");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return BadRequest(ModelState);
            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            
            if (reviewToDelete == null) 
                return NotFound();
            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return BadRequest(ModelState);
            }
            return NoContent();
        }
        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound();

            var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
