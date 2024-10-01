using Newtonsoft.Json;

namespace Elasticsearch.API.Models
{
    public class SearchResult<T>
    {
        [JsonProperty("hits")]
        public Hits<T> Hits { get; set; }
    }

    public class Hits<T>
    {
        [JsonProperty("hits")]
        public List<Hit<T>> HitList { get; set; }
    }

    public class Hit<T>
    {
        [JsonProperty("_source")]
        public T Source { get; set; }
    }
}
