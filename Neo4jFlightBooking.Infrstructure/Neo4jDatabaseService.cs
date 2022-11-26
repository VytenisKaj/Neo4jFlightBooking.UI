using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace Neo4jFlightBooking.Infrstructure
{
    public class Neo4jDatabaseService
    {
        private readonly IAsyncSession _session;
        private readonly Neo4jClient _client;
        public Neo4jDatabaseService(Neo4jClient client)
        {
            _client = client;
            _session = _client.Session;
        }

        //public IAsyncSession Session => _client.Session;
        public IAsyncSession Session => _session;
        public Neo4jClient Client => _client;
    }
}
