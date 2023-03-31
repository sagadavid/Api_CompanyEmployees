﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    ////[ApiVersion("2.0", Deprecated = true)]//no need to use when version by convention is added in extension method
    ////[Route("api/{v:apiversion}/companies")]//versioning by url
    ////[Route("api/companies")]//versioning by query string
    //[Route("api/companies")]//versioning by http header
    //[ApiController]
    //[ApiExplorerSettings(GroupName = "v2")]
    //public class CompaniesV2Controller : ControllerBase
    //{
    //    private readonly IServiceManager _service;

    //    public CompaniesV2Controller(IServiceManager service) => _service = service;

    //    [HttpGet]
    //    public async Task<IActionResult> GetCompanies()
    //    {
    //        var companies = await _service.CompanyService
    //            .GetAllCompaniesAsync(trackChanges: false);

    //        var companiesV2 = companies.Select(x => $"{x.Name} V2");

    //        return Ok(companiesV2);
    //    }
    //}
}
