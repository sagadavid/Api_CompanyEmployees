using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    /*A Record type provides us an easier way to create an immutable reference type 
     * in .NET. This means that the Record’s instance property values cannot change 
     * after its initialization. The data are passed by value and the equality 
     * between two Records is verified by comparing the value of their properties.
Records can be a valid alternative to classes when we have to send or receive data. 
    The very purpose of a DTO is to transfer data from one part of the code to another, 
    and immutability in many cases is useful.We use them to return data from a
    Web API or to represent events in our application.
This is the exact reason why we are using records for our DTOs.
    In our DTO, we have removed the Employees property and we are going to
    use the FullAddress property to concatenate the Address and Country properties 
    from the Company class. Furthermore, we are not using validation attributes in 
    this record, because we are going to use this record only to return a response 
    to the client. Therefore, validation attributes are not required.*/
    public record CompanyDTO(Guid id, string Name, string FullAddress);

    /*So, the first thing we have to do is to add the reference from the 
     * Shared project to the Service.Contracts project, and remove the
     * Entities reference. At this moment the Service.Contracts project 
     * is only referencing the Shared project.*/

}
