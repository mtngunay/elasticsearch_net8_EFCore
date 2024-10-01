using Nest;

namespace Elasticsearch.API.Services
{
    public class ElasticsearchService
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchService(string url)
        {
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex("products");

            _elasticClient = new ElasticClient(settings);
        }

        public void IndexDocument<T>(T document) where T : class
        {
            var response = _elasticClient.IndexDocument(document);
            if (!response.IsValid)
            {
                Console.WriteLine($"Error indexing document: {response.OriginalException.Message}");
            }
        }

        public T GetDocument<T>(string id) where T : class
        {
            var response = _elasticClient.Get<T>(id);
            if (!response.IsValid)
            {
                Console.WriteLine($"Error getting document: {response.OriginalException.Message}");
                return null;
            }
            return response.Source;
        }
    }
}
