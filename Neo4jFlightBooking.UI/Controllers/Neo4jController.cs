using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jFlightBooking.Infrstructure;
using Neo4jFlightBooking.UI.Models;

namespace Neo4jFlightBooking.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Neo4jController : ControllerBase
    {
        private readonly Neo4jDatabaseService _service;
        ////private readonly Neo4jClient _client;
        public Neo4jController(Neo4jClient client)
        {
           // _client = client;
            _service = new Neo4jDatabaseService(client);
        }

       /* [HttpPost("/test")]
        public async Task<string> Test()
        {
            var message = "Heeeeeeloooooo, world";
            *//*var result = await _service.Session.ExecuteWriteAsync(tx =>
            {
                return tx.RunAsync("CREATE(a: Greeting) " + "SET a.message = $message ", new { message });
            });
            var result1 = await _service.Session.ExecuteReadAsync(tx =>
            {
                return tx.RunAsync("MATCH(a: Greeting) RETURN a");
            });*//*

            var greeting = await _service.Session.ExecuteWriteAsync(async tx =>
            {
                var result = tx.RunAsync("CREATE (a:Greeting) " + "SET a.message = $message " + "RETURN a.message + ', from node ' + id(a)", new { message });
                return result;
            });



            return "";
        }*/

        [HttpGet("/getAirplaneByDestination")]
        public async Task<List<Dictionary<string, object>>> SearchAirplaneByDestination(string destiantion)
        {
            var query = @"MATCH (a:Airplane) WHERE toUpper(a.destination) CONTAINS toUpper($destination) 
                                RETURN a{ name: a.name, destination: a.destination } ORDER BY a.Name LIMIT 5";

            IDictionary<string, object> parameters = new Dictionary<string, object> { { "destination", destiantion } };

            var airpalnes = await _service.ExecuteReadDictionaryAsync(query, "a", parameters);

            return airpalnes;
        }

        /// <summary>
        /// Adds a new airplane
        /// </summary>
        [HttpPost("/addAirplane")]
        public async Task<bool> AddAirplane([FromBody] Airplane airplane)
        {
            if (airplane != null && !string.IsNullOrWhiteSpace(airplane.Name))
            {
                var query = @"MERGE (a:Airplane {name: $name}) ON CREATE SET a.destination = $destination 
                            ON MATCH SET a.destination = $destination, a.updatedAt = timestamp() RETURN true";
                IDictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "name", airplane.Name },
                    { "destination", airplane.Destination }
                };
                return await _service.ExecuteWriteTransactionAsync<bool>(query, parameters);
            }
            else
            {
                throw new System.ArgumentNullException(nameof(Airplane), "Airplane must not be null");
            }
        }

        /// <summary>
        /// Get count of persons
        /// </summary>
        [HttpGet("/getAirplaneCount")]
        public async Task<long> GetAirplaneCount()
        {
            var query = @"Match (a:Airplane) RETURN count(a) as airplaneCount";
            var count = await _service.ExecuteReadScalarAsync<long>(query);
            return count;
        }
    }

}

