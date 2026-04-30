using Microsoft.Data.Sqlite;
using Polly;

namespace API.Web.Infrastructure
{
    public static class CircuitBreaker
    {
        private static AsyncPolicy _breaker = null;

        public static AsyncPolicy Breaker
        {
            get
            {
                if (_breaker == null)
                {
                    //5  is Busy status for SQL Lite and 773 is timeout
                    _breaker = Policy.Handle<SqliteException>(x => x.SqliteErrorCode == 5 || x.SqliteErrorCode == 773 ).CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)); 
                }

                return _breaker;
            }
        }
    }
}
