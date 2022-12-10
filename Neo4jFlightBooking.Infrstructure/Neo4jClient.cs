using Neo4j.Driver;

namespace Neo4jFlightBooking.Infrstructure
{
    public class Neo4jClient : IAsyncDisposable
    {
        private readonly IDriver _driver;
        private readonly IAsyncSession _session;

        public Neo4jClient(string connection)
        {
            _driver = GraphDatabase.Driver(connection);
            _session = _driver.AsyncSession();
        }

        public IAsyncSession Session => _session;
        public IDriver Driver => _driver;

        public async ValueTask DisposeAsync()
        {
            await _session.CloseAsync();
        }
    }
}