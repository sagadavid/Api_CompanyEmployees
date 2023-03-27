using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CodeMaze_CompanyEmployees
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company,CompanyDto>()
                
                ////exception.. happens because AutoMapper is not able to find the specific 
                ////FullAddress property as we specified in the MappingProfile class.
                
                .ForMember(c => c.FullAddress,////preferred to get xml response



              //.ForCtorParam("FullAddress",////we are not using the ForMember method but the ForCtorParam method
                                            ///to specify the name of the parameter in the constructor that 
                                            ///AutoMapper needs to map to.
                    opt => opt.MapFrom(x => string.Join
                        (' ', x.Address, x.Country)));
            
            //any misspelling cause exception:
            //AutoMapper.AutoMapperConfigurationException: CompanyDTO does not have a matching constructor with a parameter named 'FullAdress'.
            //Shared.DataTransferObjects.CompanyDTO.When mapping to records, consider excluding non-public constructors.
            
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<CompanyForUpdateDto, Company>()
                                                    .ReverseMap();//for patch to work
            CreateMap<UserForRegistrationDto, User>();//userdto to user
        }
    }
}
