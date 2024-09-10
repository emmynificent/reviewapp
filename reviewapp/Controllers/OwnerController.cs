using Microsoft.AspNetCore.Mvc;
using reviewapp.Interfaces;
using reviewapp.Model;
using reviewapp.Dto;
using AutoMapper;

namespace reviewapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController :ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper, ICountryRepository countryRepository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRepository = countryRepository; 
        }
        [HttpGet]
        public IActionResult GetOwners()
        {
            var owners = _ownerRepository.GetOwners();
            return Ok(owners);
        }
        [HttpGet ("{ownerId}")]
        public IActionResult GetOwner(int id)
        {
            if(!_ownerRepository.OwnerExists(id))
                return NotFound();
            var owner = _ownerRepository.GetOwner(id);
            if(!ModelState.IsValid)
                return BadRequest();
            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if(!_ownerRepository.OwnerExists(ownerId))
                return NotFound();
            var pokemonOwner = _ownerRepository.GetPokemonByOwner(ownerId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);    

            return Ok(pokemonOwner);

        }
        [HttpPost]
        public IActionResult CreateOwner( [FromQuery] int countryId, [FromBody] OwnerDto createowner)
        {
            if(createowner == null)
                return BadRequest();
            var owner = _ownerRepository.GetOwners()
                .Where(own => own.LastName.Trim().ToUpper() == createowner.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "This owner exist");
                return StatusCode(422, ModelState);
            }
            
            if(!ModelState.IsValid)
                return BadRequest();
            var ownermap = _mapper.Map<Owner>(createowner);

            ownermap.Country = _countryRepository.GetCountry(countryId);
           

            if (!_ownerRepository.CreateOwner(ownermap))
                return BadRequest();


            return Ok("Owner Created sucessfully");

        }
        [HttpPut("{ownerId}")]

        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updateOwner)
        {
            if(updateOwner == null)
                return BadRequest();

            
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();
            if(!ModelState.IsValid)
                return BadRequest();

            var ownerMap = _mapper.Map<Owner>(updateOwner);

            //ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        public IActionResult DeleteOwner(int ownerId)
        {
            var ownerToDelete = _ownerRepository.GetOwner(ownerId);
            if (ownerToDelete == null)
                return NotFound();
            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "something went wrong deleting this owner");
                return BadRequest(ModelState);
            }
            return NoContent(); 
        }

    }
}