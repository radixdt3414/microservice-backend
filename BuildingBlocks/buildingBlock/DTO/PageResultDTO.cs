namespace buildingBlock.DTO
{
    public class PageResultDTO<T>
    {
        public T Data { get; set; }
        public long TotalRecords { get; set; }
        public long PageLimit { get; set; }
        public long PageCount { get; set; }
        public long PageIndex { get; set; }
    }
}