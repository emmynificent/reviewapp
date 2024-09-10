using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using reviewapp.Data;
using reviewapp.Interfaces;
using reviewapp.Model;

namespace reviewapp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _countryRepository;
        public CountryRepository (DataContext context)
        {
            _countryRepository = context; 
        }

        public bool CountryExists(int id)
        {
            return _countryRepository.Countries.Any(c => c.Id == id);
        }
        public  bool CreateCountry (Country country)
        {
            _countryRepository.Add(country);
            return Save();
        }
        public bool DeleteCountry(Country country)
        {
            _countryRepository.Remove(country);
            return Save();
            
        }
        public ICollection<Country> GetCountries()
        {
            return _countryRepository.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _countryRepository.Countries.Where(c => c.Id == id).FirstOrDefault();


        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _countryRepository.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();

        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _countryRepository.Owners.Where(c => c.Country.Id == countryId).ToList();

        }


        public bool Save()
        {
            var saved = _countryRepository.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _countryRepository.Update(country);
            return Save();
        }
    }
}
