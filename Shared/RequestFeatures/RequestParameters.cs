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

        public string? OrderBy { get; set; } //able to send requests with the orderBy clause in them
        public string? Fields { get; set; }//to achieve data shaping
    }
}
