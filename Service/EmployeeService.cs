using AutoMapper;
using Contracts;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repoMan;
        private readonly ILoggerManager _logMan;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repoMan, ILoggerManager logMan,
            IMapper mapper)
        {
            _repoMan = repoMan;
            _logMan = logMan;
            _mapper = mapper;
        }
    }
}
