using Microsoft.Extensions.Logging;

namespace Negocio.Logger
{
    [ProviderAlias("Database")]
    public class DbLoggerProvider : ILoggerProvider
    {
        public string ConnectionString { get; internal set; }

        public IServiceProvider? ServiceProvider { get; set; } = null;

        public DbLoggerProvider(string connectionString) {
            ConnectionString = connectionString;
        }
       
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {

        }
    }
}