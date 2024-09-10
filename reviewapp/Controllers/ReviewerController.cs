using Microsoft.AspNetCore.Mvc;
using reviewapp.Interfaces;
using reviewapp.Dto;
using reviewapp.Model;
using AutoMapper;

namespace reviewapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController: ControllerBase
    {
        private readonly IReviewerRepositoy _reviewerRepository;
        private readonly IMapper _mapper;
        public ReviewerController(IReviewerRepositoy reviewRepository, IMapper mapper) 
        {
            _reviewerRepository = reviewRepository;   
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetReviewers()
        {
            var reviews = _reviewerRepository.GetReviewers();
            return Ok(reviews);
        }

        [HttpGet("reviewerId")]
        public IActionResult GetReviewer(int id)
        {
            if(!_reviewerRepository.ReviewerExist(id)) 
                return NotFound("Reviewer does not exist");
            var reviewer = _reviewerRepository.GetReviewer(id);   

            return Ok(reviewer);
        }
    

        [HttpGet ("{reviewerid}/reviews")]
        public IActionResult GetReviewByReviewer(int reviewerid)
        {
            var reviews =_reviewerRepository.GetReviewsByReviewer(reviewerid);
            if(!ModelState.IsValid)
                return NotFound();
            return Ok(reviews);
        }
        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest();
            var country = _reviewerRepository.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper()== reviewerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
                return BadRequest();

            return Ok("Created successfully");

        }
        [HttpPut ("{reviewerId}")]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updateReviewer)
        {
            if (reviewerId == null)
                return BadRequest(ModelState);

            if(updateReviewer ==  null)
                return BadRequest();
            if (!_reviewerRepository.ReviewerExist(reviewerId))
                return NotFound("Reviewer does not exist");
            var reviewerMap = _mapper.Map<Reviewer>(updateReviewer);
            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something happened updating this reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok($"Reviewer has been updated");
        }

        [HttpDelete ("{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerExist(reviewerId))
            {
                return BadRequest(ModelState);
            }
            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "error");
            }
            return NoContent();
     
          
        }

    }
}
