namespace StudentRecords.API.Models
{
    public class DataTableResponse
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public dynamic data { get; set; }
        public string error { get; set; }
    }
}