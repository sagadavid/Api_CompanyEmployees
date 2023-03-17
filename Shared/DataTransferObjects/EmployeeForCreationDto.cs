using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record EmployeeForCreationDto
    {
        //validation in positional record, in init setter style
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        string Name { get; init; }

        [Required(ErrorMessage = "Age is a required field.")]
        int Age { get; init; }

        [Required(ErrorMessage = "Position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        string Position { get; init; }
    }
       
        
    //(
    //    //validation in positional record
    //    //validation while creation.. 'cause dto is deserializing request body.. bring validation here !
    //    [Required(ErrorMessage = "Employee name is a required field.")]
    //    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters." )]
    //    string Name,
        
    //    [Required(ErrorMessage = "Age is a required field.")]
    //    int Age,
        
    //    [Required(ErrorMessage = "Position is a required field.")]
    //    [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
    //    string Position
    //    );
}
