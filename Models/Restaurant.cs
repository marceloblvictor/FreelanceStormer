using System.ComponentModel.DataAnnotations;

namespace RestaurantScheduler.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Seats { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        
        public int UserId { get; set; }
        public virtual Organization? Organization { get; set; } = default;
    }
}
