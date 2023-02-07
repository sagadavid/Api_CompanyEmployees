using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
   public class Employee
    {
        [Column("EmployeeId")]//will be mapped to DB as shown
        public Guid Id { get; set; }

        [Required(ErrorMessage = "name is required")]
        [MaxLength(60, ErrorMessage = "up to 30 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage ="age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "position is required ")]
        [MaxLength(60, ErrorMessage = "up to 20 characters")]
        public string? Position { get; set; }

        [ForeignKey(nameof(Company))]//A nameof expression produces
                                     //the name of a variable, type, or member
                                     //as the string constant.
        public Guid CompanyId { get; set; }//navigational property
        public Company? Company { get; set; }////navigational property
    }
}
