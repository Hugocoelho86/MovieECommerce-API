namespace ECommerceMovies.API.Models
{
    public class OrderItem : BaseEntity
    {

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public int Quantity { get; set; }
        public int PriceAtTimeOfOrder { get; set; }
    }
}
