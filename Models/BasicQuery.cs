namespace FreelanceStormer.Models
{
    public class BasicQuery
    {
        public int Page { get; init; }
        public int Size { get; init; }
        public string Search { get; init; }

        public int Skip => (Page + 1) * Size;

        public BasicQuery(int page, int size, string search) 
        { 
            Page = page;
            Size = size;
            Search = search;
        }

        public BasicQuery() 
        {
            Page = 0;
            Size = 5;
            Search = string.Empty;
        }

    }
}
