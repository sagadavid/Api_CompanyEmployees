using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public class EmployeeParameters : RequestParameters // will hold the specific parameters.
    {
        public uint MinAge { get; set; }//unsigned int properties (to avoid negative year values)

        public uint MaxAge { get; set; } = int.MaxValue;

        public bool ValidAgeRange => MaxAge > MinAge;

        public string? SearchTerm { get; set; }
    }
}
