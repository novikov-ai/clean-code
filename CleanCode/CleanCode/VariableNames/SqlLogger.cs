using System.Data.SqlClient;

namespace CleanCode.VariableNames
{
    class SqlLogger
    {
        private SqlLogger() { }

        private static SqlLogger _logger = new SqlLogger();
        public static SqlLogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    lock (_logger)
                    {
                        if (_logger == null)
                            _logger = new SqlLogger();
                    }
                }

                return _logger;
            }
        }

        public static SqlCommand GetLoggerInfoCommand(SqlConnection connection, string message)
        {
            // context => SQL-stored procedure for log info event
            // old name: cmd
            // new name: spCommandLogInfo
            var spCommandLogInfo = connection.CreateCommand();
            spCommandLogInfo.CommandType = System.Data.CommandType.StoredProcedure;
            spCommandLogInfo.CommandText = "Logs_InsertInfo";

            // context => SQL-parameter log message
            // old name: pMessage
            // new name: parameterLogMessage
            SqlParameter parameterLogMessage = new SqlParameter()
            {
                ParameterName = "@Message",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = message
            };
            spCommandLogInfo.Parameters.Add(parameterLogMessage);

            return spCommandLogInfo;
        }
        public static SqlCommand GetLoggerErrorCommand(SqlConnection connection, string message, string stackTrace)
        {
            // context => SQL-stored procedure for log error event
            // old name: cmd
            // new name: spCommandLogError
            var spCommandLogError = GetLoggerInfoCommand(connection, message);
            spCommandLogError.CommandText = "Logs_InsertError";

            // context => SQL-parameter for log stacktrace
            // old name: pStackTrace
            // new name: parameterLogStackTrace
            SqlParameter parameterLogStackTrace = new SqlParameter()
            {
                ParameterName = "@StackTrace",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = stackTrace
            };
            spCommandLogError.Parameters.Add(parameterLogStackTrace);

            return spCommandLogError;
        }
    }
}
