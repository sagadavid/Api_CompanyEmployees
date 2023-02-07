using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Company
    {
        [Column("CompanyId")]//will be mapped to DB as shown
        public Guid Id { get; set; }
        
        [Required(ErrorMessage ="name is required")]
        [MaxLength(60, ErrorMessage ="up to 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "address is required ")]
        [MaxLength(60, ErrorMessage = "up to 60 characters")]
        public string? Address { get; set; }

        public string? Country { get; set; }

        public ICollection<Employee>? Employees { get; set; }//navigational property
                                                            
    }
}