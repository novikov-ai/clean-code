using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CleanCode.VariableNames;

namespace CleanCode.MethodNames
{
    public class Repository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // (1)
        // GetList - GetUsers
        public async Task<List<User>> GetUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var spCommandGetUsers = connection.CreateCommand();

                spCommandGetUsers.CommandType = System.Data.CommandType.StoredProcedure;
                spCommandGetUsers.CommandText = "User_GetList";
                
                var users = new List<User>();

                connection.Open();
                
                SqlCommand spCommandLogger = null;

                try
                {
                    using (var reader = await spCommandGetUsers.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            users.Add(SqlTools.GetUserFromRow(reader));
                        }

                        spCommandLogger = SqlLogger.GetLoggerInfoCommand(connection, $"Got {users.Count} user(s).");
                    }
                }
                catch (Exception e)
                {
                    spCommandLogger = SqlLogger.GetLoggerErrorCommand(connection, "User's list can't be received.",
                        $"{e.StackTrace}");
                }

                if (spCommandLogger is null)
                    return null;

                await spCommandLogger.ExecuteNonQueryAsync();

                return users;
            }
        }

        // (2)
        // Create - CreateUser
        public async Task<int> CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var spCommandCreateUser = SqlTools.GetCommandWithUserParams(connection, "User_Create", user);

                connection.Open();
                
                SqlCommand spCommandLogger = null;
                
                int createdUserId = 0;

                try
                {
                    User createdUser = null;
                    using (var reader = await spCommandCreateUser.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            createdUser = SqlTools.GetUserFromRow(reader);
                        }

                        if (reader.RecordsAffected == 0 || createdUser is null)
                        {
                            throw new Exception("Affected 0 rows. User wasn't created");
                        }

                        createdUserId = createdUser.Id;
                        spCommandLogger = SqlLogger.GetLoggerInfoCommand(connection, $"User #{createdUser.Id} was created.");
                    }
                }
                catch (Exception e)
                {
                    spCommandLogger = SqlLogger.GetLoggerErrorCommand(connection, "User wasn't created", $"{e.StackTrace}");
                }

                if (spCommandLogger is null)
                    throw new Exception("Logger is not available.");

                await spCommandLogger.ExecuteNonQueryAsync();

                return createdUserId;
            }
        }

        // (3)
        // Get - GetUser
        public async Task<User> GetUser(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var spCommandGetUser = connection.CreateCommand();
                spCommandGetUser.CommandType = System.Data.CommandType.StoredProcedure;
                spCommandGetUser.CommandText = "User_Read";
                
                SqlParameter parameterUserId = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = $"{userId}"
                };

                spCommandGetUser.Parameters.Add(parameterUserId);

                User user = null;
                
                SqlCommand spCommandLogger = null;

                connection.Open();
                try
                {
                    using (var reader = await spCommandGetUser.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            user = SqlTools.GetUserFromRow(reader);
                        }

                        if (user != null)
                            spCommandLogger = SqlLogger.GetLoggerInfoCommand(connection, $"User #{user.Id} was got.");
                    }
                }
                catch (Exception e)
                {
                    spCommandLogger = SqlLogger.GetLoggerErrorCommand(connection, "User wasn't got", $"{e.StackTrace}");
                }

                if (spCommandLogger is null)
                    return null;

                await spCommandLogger.ExecuteNonQueryAsync();

                return user;
            }
        }

        // (4)
        // Update - UpdateUser
        public async Task<bool> UpdateUser(User updated)
        {
            bool isUserUpdated = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                var spCommandUserUpdate = SqlTools.GetCommandWithUserParams(connection, "User_Update", updated);
                
                SqlParameter parameterUserId = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = $"{updated.Id}"
                };
                spCommandUserUpdate.Parameters.Add(parameterUserId);


                User updatedUser = null;
                
                SqlCommand spCommandLogger = null;

                connection.Open();
                try
                {
                    using (var reader = await spCommandUserUpdate.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            updatedUser = SqlTools.GetUserFromRow(reader);

                            if (reader.RecordsAffected > 0)
                            {
                                isUserUpdated = true;
                                spCommandLogger = SqlLogger.GetLoggerInfoCommand(connection,
                                    $"User #{updatedUser.Id} was updated.");
                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    spCommandLogger = SqlLogger.GetLoggerErrorCommand(connection, "User wasn't updated", $"{e.StackTrace}");
                }

                if (spCommandLogger is null)
                    return false;

                await spCommandLogger.ExecuteNonQueryAsync();

                return isUserUpdated;
            }
        }

        // (5)
        // Delete - DeleteUser
        public async Task<bool> DeleteUser(int id)
        {
            bool isUserDeleted = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                var spCommandUserDelete = connection.CreateCommand();
                spCommandUserDelete.CommandType = System.Data.CommandType.StoredProcedure;
                spCommandUserDelete.CommandText = "User_Delete";
                
                SqlParameter parameterUserId = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = $"{id}"
                };

                spCommandUserDelete.Parameters.Add(parameterUserId);
                
                SqlCommand spCommandLogger = null;

                connection.Open();
                try
                {
                    if (await spCommandUserDelete.ExecuteNonQueryAsync() == 1)
                    {
                        isUserDeleted = true;
                        spCommandLogger = SqlLogger.GetLoggerInfoCommand(connection, $"User #{id} was deleted.");
                    }
                }

                catch (Exception e)
                {
                    spCommandLogger = SqlLogger.GetLoggerErrorCommand(connection, "User wasn't deleted", $"{e.StackTrace}");
                }

                if (spCommandLogger is null)
                    return false;

                await spCommandLogger.ExecuteNonQueryAsync();

                return isUserDeleted;
            }
        }
    }
}