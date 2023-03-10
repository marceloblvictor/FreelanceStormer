using System.ComponentModel.DataAnnotations;

namespace FreelanceStormer.Models
{
    public class Organization
    {
        [Key]        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
