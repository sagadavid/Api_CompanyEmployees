using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestFeatures
{
    public abstract class RequestParameters//an abstract class to hold the common
                                           //properties for all the entities in our project,
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }

        }
        //implemented page 2 size 2 .. if the remaining employee is only 1 in the last page, 
        //respond body shows an empty array..
        //so, i change last query to size 1 then, grasps the last entity
    }
}
