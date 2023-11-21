using System.ComponentModel.DataAnnotations;

namespace ECommerceMovies.API.Models
{
    public partial class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
