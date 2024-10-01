using Elasticsearch.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticsearchTestController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public ElasticsearchTestController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpPost("test-index")]
        public async Task<IActionResult> CreateTestIndex()
        {
            var createIndexResponse = await _elasticClient.Indices.CreateAsync("test-index", c => c
                .Map<Product>(m => m
                    .AutoMap()
                )
            );

            if (!createIndexResponse.IsValid)
            {
                return BadRequest(createIndexResponse.OriginalException.Message);
            }

            return Ok("Index created successfully");
        }

        [HttpPost("index-document")]
        public async Task<IActionResult> IndexDocument([FromBody] Product product)
        {
            var indexResponse = await _elasticClient.IndexDocumentAsync(product);

            if (!indexResponse.IsValid)
            {
                return BadRequest(indexResponse.OriginalException.Message);
            }

            return Ok("Document indexed successfully");
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
                .Index("test-index")
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name)
                        .Query(query)
                    )
                )
            );

            if (!searchResponse.IsValid)
            {
                return BadRequest(searchResponse.OriginalException.Message);
            }

            return Ok(searchResponse.Documents);
        }
    }
}
