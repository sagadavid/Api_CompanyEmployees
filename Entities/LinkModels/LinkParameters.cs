using Microsoft.AspNetCore.Http;
using Shared.RequestFeatures;

/* We are going to use this record to transfer required parameters from our controller to the service layer and avoid the installation of an additional NuGet package inside the Service and Service.Contracts projects. */
namespace Entities.LinkModels
{
    public record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);
}
