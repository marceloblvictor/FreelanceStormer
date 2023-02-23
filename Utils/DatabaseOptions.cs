namespace FreelanceStormer.Utils
{
    public class DatabaseOptions
    {
        public const string ConnectionStrings = "ConnectionStrings";

        public string FreelanceStormerDbContext { get; set; } = string.Empty;
        public int Test { get; set; } = default;
    }
}
