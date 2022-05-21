using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CleanCode.VariableNames
{
    public class Repository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<User>> GetList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // context => SQL-stored procedure for getting all users
                // old name: cmd
                // new name: spCommandGetUsers
                var spCommandGetUsers = connection.CreateCommand();

                spCommandGetUsers.CommandType = System.Data.CommandType.StoredProcedure;
                spCommandGetUsers.CommandText = "User_GetList";

                // context => All users
                // old name: list
                // new name: users
                var users = new List<User>();

                connection.Open();

                // context => SQL-stored procedure for logger logic
                // old name: loggerCmd
                // new name: spCommandLogger
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

        public async Task<int> Create(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // context => SQL-stored procedure for creating new user
                // old name: cmd
                // new name: spCommandCreateUser
                var spCommandCreateUser = SqlTools.GetCommandWithUserParams(connection, "User_Create", user);

                connection.Open();

                // context => SQL-stored procedure for logger logic
                // old name: logger
                // new name: spCommandLogger
                SqlCommand spCommandLogger = null;
                
                // context => returned id of created user
                // old name: newId
                // new name: createdUserId
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

        public async Task<User> Get(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // context => SQL-stored procedure for getting user by id
                // old name: cmd
                // new name: spCommandGetUser
                var spCommandGetUser = connection.CreateCommand();
                spCommandGetUser.CommandType = System.Data.CommandType.StoredProcedure;
                spCommandGetUser.CommandText = "User_Read";

                // context => SQL-parameter user id
                // old name: pId
                // new name: parameterUserId
                SqlParameter parameterUserId = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = $"{userId}"
                };

                spCommandGetUser.Parameters.Add(parameterUserId);

                User user = null;
                
                // context => SQL-stored procedure for logger logic
                // old name: logger
                // new name: spCommandLogger
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

        public async Task<bool> Update(User updateUser)
        {
            // context => Is user updated
            // old name: result
            // new name: isUserUpdated
            bool isUserUpdated = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                // context => SQL-stored procedure for updating user
                // old name: cmd
                // new name: spCommandUserUpdate
                var spCommandUserUpdate = SqlTools.GetCommandWithUserParams(connection, "User_Update", updateUser);

                // context => SQL-parameter user id
                // old name: pId
                // new name: parameterUserId
                SqlParameter parameterUserId = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = $"{updateUser.Id}"
                };
                spCommandUserUpdate.Parameters.Add(parameterUserId);


                User updatedUser = null;
                
                // context => SQL-stored procedure for logger logic
                // old name: logger
                // new name: spCommandLogger
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

        public async Task<bool> Delete(int id)
        {
            // context => Is user deleted
            // old name: result
            // new name: isUserDeleted
            bool isUserDeleted = false;

            using (var connection = new SqlConnection(_connectionString))
            {
                // context => SQL-stored procedure for deleting user
                // old name: cmd
                // new name: spCommandUserDelete
                var spCommandUserDelete = connection.CreateCommand();
                spCommandUserDelete.CommandType = System.Data.CommandType.StoredProcedure;
                spCommandUserDelete.CommandText = "User_Delete";

                // context => SQL-parameter user id
                // old name: pId
                // new name: parameterUserId
                SqlParameter parameterUserId = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input,
                    Value = $"{id}"
                };

                spCommandUserDelete.Parameters.Add(parameterUserId);
                
                // context => SQL-stored procedure for logger logic
                // old name: logger
                // new name: spCommandLogger
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