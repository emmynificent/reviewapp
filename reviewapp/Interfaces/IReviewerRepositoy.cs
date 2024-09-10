using reviewapp.Model;

namespace reviewapp.Interfaces
{
    public interface IReviewerRepositoy
    {
        public ICollection<Reviewer> GetReviewers();
        public ICollection<Review> GetReviewsByReviewer(int reviewerid);
        Reviewer GetReviewer(int reviewerid);
        bool ReviewerExist(int reviewerid);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);

        bool Save();
    }
}
