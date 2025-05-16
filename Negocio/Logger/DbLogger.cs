
using Microsoft.Extensions.Logging;
using Negocio.Persistencia.Modelos;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace Negocio.Logger
{
    public class DbLogger : ILogger
    {
        private readonly DbLoggerProvider _dbLoggerProvider;

        public IPrincipal? Principal { get; set; }

        private string? Usuario { get; set; }

        private string CALIPSO = "Calipso";

        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider) {
        
            _dbLoggerProvider = dbLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) {
                return;
            }

            if (String.IsNullOrWhiteSpace(eventId.Name)) {
                Debug.WriteLine(formatter(state, exception));
            }

            try
            {
                SaveToDB(logLevel, eventId, formatter(state, exception));
            }
            catch { 
                Debug.WriteLine(formatter(state, exception));  
            }
        }


        protected virtual void SaveToDB(LogLevel loglevel, EventId eventId, string formatter) {

            using (var connection = new SqlConnection(_dbLoggerProvider.ConnectionString)) {
            
                connection.Open();

                using (var command = new SqlCommand()) {
                
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "INSERT INTO dbo.Registros (Usuario, Aplicacion, Nivel, Evento, Operacion) Values (@Usuario, @Aplicacion, @Nivel, @Evento, @Operacion)";
                    command.Parameters.Add(new SqlParameter("@Usuario", GetCurrentUser()));
                    command.Parameters.Add(new SqlParameter("@Aplicacion", CALIPSO));
                    command.Parameters.Add(new SqlParameter("@Nivel", loglevel.ToString()));
                    command.Parameters.Add(new SqlParameter("@Evento", eventId.Name ?? String.Empty));
                    command.Parameters.Add(new SqlParameter("@Operacion", formatter));
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private string GetCurrentUser()
        {
            if (_dbLoggerProvider.ServiceProvider != null) {
                Principal = (IPrincipal)_dbLoggerProvider.ServiceProvider.GetService(typeof(IPrincipal));
                Usuario = Principal?.Identity?.Name;
            }

            return Usuario ?? "CalypsoApp";

        }
    }
}
