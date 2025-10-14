namespace buildingBlock.DTO
{
    public class PaginationDTO
    {
        public long PageLimit { get; set; }
        public long PageIndex { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "desc";
    }
}
