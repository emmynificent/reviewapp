using reviewapp.Data;
using reviewapp.Interfaces;
using reviewapp.Model;

namespace reviewapp.Repository
{
    public class ReviewerRepository : IReviewerRepositoy
    {
        private readonly DataContext _context;
        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            var viewer = _context.Add(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerid)
        {
            var reviewer =   _context.Reviewers.Where(r => r.Id == reviewerid).FirstOrDefault();
            return reviewer;

        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerid)
        {
            return _context.Reviews.Where(r=> r.Reviewer.Id == reviewerid).ToList();
        }

        public bool ReviewerExist(int reviewerid)
        {
            return _context.Reviewers.Any(r=>r.Id==reviewerid);
        }
   
        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public bool Save()
        {
            var newReview = _context.SaveChanges();
            return newReview>0? true: false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Update(reviewer);
            return Save();
        }
    }
}
