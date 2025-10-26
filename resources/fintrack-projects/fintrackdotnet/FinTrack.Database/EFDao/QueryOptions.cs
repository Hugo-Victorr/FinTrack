namespace FinTrack.Database.EFDao
{
    public class QueryOptions
    {
        public int? _Start { get; set; }
        public int? _End { get; set; }
        public string? _Sort { get; set; }
        public string? _Order { get; set; }
        public Dictionary<string, string>? Filters { get; set; }
    }
}
