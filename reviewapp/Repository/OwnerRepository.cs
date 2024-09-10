using reviewapp.Data;
using reviewapp.Interfaces;
using reviewapp.Model;

namespace reviewapp.Repository
{
    public class OwnerRepository: IOwnerRepository
    {
        private readonly DataContext _ownerRepository;
        public OwnerRepository(DataContext context)
        {
            _ownerRepository = context; 
        }

        public bool CreateOwner(Owner owner)
       {
            _ownerRepository.Add(owner);
            return Save();

        }

        public bool DeleteOwner(Owner owner)
        {
            _ownerRepository.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
           var owner = _ownerRepository.Owners.Where(ow=> ow.Id == ownerId).FirstOrDefault();
            return owner;
        }

        public ICollection<Owner> GetOwnerOfPokemon(int pokeId)
        {
            var owner = _ownerRepository.PokemonOwners.Where(po => po.PokemonId == pokeId).Select(o => o.Owner).ToList();
            return owner;
        }

        public ICollection<Owner> GetOwners()
        {
            var owners = _ownerRepository.Owners.ToList();
            return owners;  
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _ownerRepository.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _ownerRepository.Owners.Where(o => o.Id == ownerId).Any();
        }

        public bool Save()
        {
            var saved = _ownerRepository.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _ownerRepository.Update(owner);
            return Save();
        }
    }
}
