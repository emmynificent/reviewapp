using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using reviewapp.Dto;
using reviewapp.Interfaces;
using reviewapp.Model;

namespace reviewapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCountry()
        {
            var countries = _countryRepository.GetCountries();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet ("{countryId}")]
        public IActionResult GetCountry(int countryId)
        {
            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);  
            return Ok(country);
        }

        [HttpGet ("/owners/{ownerId}")]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _countryRepository.GetCountryByOwner(ownerId);
            if (!ModelState.IsValid)    
                return BadRequest(ModelState);  

            return Ok(country); 
        }
        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var country = _countryRepository.GetCountries()
                .Where(c=>c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                //expeted feedback is null.
                ModelState.AddModelError("", "Country already exists");
                return StatusCode(422, ModelState);

            }

            if(!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_countryRepository.CreateCountry(countryMap))
                return BadRequest();
          
            return Ok("country created ");

        }
       
        [HttpPut("{countryId}")]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{countryId}")]
        public IActionResult DeleteCountry(int countryId)
        {
            if(!_countryRepository.CountryExists(countryId))
                return BadRequest(ModelState);  
            var dtc = _countryRepository.GetCountry(countryId);
            if (dtc == null)
                return NotFound();
            if (!_countryRepository.DeleteCountry(dtc))
            {
                ModelState.AddModelError("", "something went wrong deleting this country");
                return BadRequest(ModelState);
            }
            return Ok($"{dtc.Name} has been deleted successfully");
        }
    }
}
