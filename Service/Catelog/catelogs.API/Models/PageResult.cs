namespace catelogs.API.Models
{
    public class PageResult<T>
    {
        public T Data { get; set; }
        public long TotalRecords {get;set;}
        public long PageLimit { get;set;}
        public long PageCount {get;set;}
        public long TotalPages {get;set;}
        public long PageIndex { get;set;}
    }
}