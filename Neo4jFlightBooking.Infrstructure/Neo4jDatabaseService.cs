using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

namespace Neo4jFlightBooking.Infrstructure
{
    public class Neo4jDatabaseService
    {
        private readonly Neo4jClient _client;
        public Neo4jDatabaseService(Neo4jClient client)
        {
            _client = client;
        }

        public IAsyncSession Session => _client.Session;
        public Neo4jClient Client => _client;

        /// <summary>
        /// Execute read list as an asynchronous operation.
        /// </summary>
        public async Task<List<string>> ExecuteReadListAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadTransactionAsync<string>(query, returnObjectKey, parameters);
        }

        /// <summary>
        /// Execute read dictionary as an asynchronous operation.
        /// </summary>
        public async Task<List<Dictionary<string, object>>> ExecuteReadDictionaryAsync(string query, string returnObjectKey, IDictionary<string, object>? parameters = null)
        {
            return await ExecuteReadTransactionAsync<Dictionary<string, object>>(query, returnObjectKey, parameters);
        }

        /// <summary>
        /// Execute read scalar as an asynchronous operation.
        /// </summary>
        public async Task<T> ExecuteReadScalarAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {

            parameters ??= new Dictionary<string, object>();

            var result = await Session.ExecuteReadAsync(async tx =>
            {
                T scalar = default(T);

                var res = await tx.RunAsync(query, parameters);

                scalar = (await res.SingleAsync())[0].As<T>();

                return scalar;
            });

            return result;
        }

        /// <summary>
        /// Execute write transaction
        /// </summary>
        public async Task<T> ExecuteWriteTransactionAsync<T>(string query, IDictionary<string, object>? parameters = null)
        {

            parameters ??= new Dictionary<string, object>();

            var result = await Session.ExecuteWriteAsync(async tx =>
            {
                T scalar = default(T);

                var res = await tx.RunAsync(query, parameters);

                scalar = (await res.SingleAsync())[0].As<T>();

                return scalar;
            });

            return result;

        }

        /// <summary>
        /// Execute read transaction as an asynchronous operation.
        /// </summary>
        private async Task<List<T>> ExecuteReadTransactionAsync<T>(string query, string returnObjectKey, IDictionary<string, object>? parameters)
        {

            parameters ??= new Dictionary<string, object>();

            var result = await Session.ExecuteReadAsync(async tx =>
            {
                var data = new List<T>();

                var res = await tx.RunAsync(query, parameters);

                var records = await res.ToListAsync();

                data = records.Select(x => (T)x.Values[returnObjectKey]).ToList();

                return data;
            });

            return result;

        }


    }
}
