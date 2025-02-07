namespace JollyCustomerClient.Helper
{
    public class UserRating
    {
        public static string GetStarRatingHtml(int quantity)
        {
            const int maxStars = 5;
            const string fullStar = "<span class='star'>&#9733;</span>";
            const string emptyStar = "<span class='star'>&#9734;</span>";

            int fullStars = Math.Min(quantity, maxStars);
            int emptyStars = Math.Max(maxStars - fullStars, 0);

            return string.Concat(Enumerable.Repeat(fullStar, fullStars))
                + string.Concat(Enumerable.Repeat(emptyStar, emptyStars));
        }
    }
}
