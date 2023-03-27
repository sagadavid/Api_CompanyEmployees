using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAuthenticationService
    {
        //This method will execute the registration logic and return the identity result to the caller.
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    }
}
