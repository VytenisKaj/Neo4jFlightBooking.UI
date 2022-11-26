using Microsoft.AspNetCore.Mvc;
using Neo4jFlightBooking.Infrstructure;

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

        [HttpPost("/test")]
        public async Task<string> Test()
        {
            var message = "Heeeeeeloooooo, world";
            var result = await _service.Session.ExecuteWriteAsync(tx =>
            {
                return tx.RunAsync("CREATE(a: Greeting) " + "SET a.message = $message ", new { message });
            });
            var result1 = await _service.Session.ExecuteReadAsync(tx =>
            {
                return tx.RunAsync("MATCH(a: Greeting) RETURN a");
            });



            return "";
        }
    }
}
