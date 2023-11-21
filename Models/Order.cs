namespace ECommerceMovies.API.Models
{
    public class Order: BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }


    }
}
