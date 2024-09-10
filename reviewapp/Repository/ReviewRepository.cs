using reviewapp.Data;
using reviewapp.Interfaces;
using reviewapp.Model;

namespace reviewapp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _reviewRepository;  

        public ReviewRepository(DataContext datacontext)
        {
            _reviewRepository = datacontext;
        }
        public bool CreateReview(Review review)
        {
            _reviewRepository.Reviews.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _reviewRepository.Reviews.Remove(review);  
            return Save();  
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _reviewRepository.Reviews.RemoveRange(reviews);
            return Save();  
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _reviewRepository.Pokemons.ToList();
         }

        public Review GetReview(int id)
        {
            var review =  _reviewRepository.Reviews.Where(r => r.id == id).FirstOrDefault();
            return review;


        }

        public Reviewer GetReviewer(int id)
        {
            return _reviewRepository.Reviewers.Where(re=> re.Id == id).FirstOrDefault();   

        }

        public ICollection<Review> GetReviews()
        {
            return _reviewRepository.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _reviewRepository.Pokemons.Where(p => p.Id == pokeId).Select(r => r.Reviews).FirstOrDefault();
        }

        public bool ReviewExists(int id)
        {
            return _reviewRepository.Reviews.Any(r => r.id == id);
        }

        public bool Save()
        {
            var saved = _reviewRepository.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
             _reviewRepository.Reviews.Update(review);
            return Save();

        }
    }
}
