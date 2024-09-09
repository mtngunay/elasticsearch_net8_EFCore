namespace Elasticsearch.API.Data
{
    public class ProductLog
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string OperationType { get; set; } // Insert, Update, Delete
        public DateTime OperationDate { get; set; }
        public string Details { get; set; }
    }
}
