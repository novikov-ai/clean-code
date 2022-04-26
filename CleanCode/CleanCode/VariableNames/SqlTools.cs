using System;
using System.Data.SqlClient;

namespace CleanCode.VariableNames
{
    public class SqlTools
    {
        public static User GetUserFromRow(SqlDataReader reader)
        {
            var user = new User()
            {
                Id = (int)reader["Id"],
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Picture = reader["Picture"].ToString(),
                Title = reader["Title"].ToString(),
                Gender = reader["Gender"].ToString(),
                Email = reader["Email"].ToString(),
                DateBirth = (DateTime)reader["DateBirth"],
                DateRegister = (DateTime)reader["DateRegister"]
            };

            return user;
        }

        public static SqlCommand GetCommandWithUserParams(SqlConnection connection, 
            string storedProcedure,
            User user)
        {
            // context => SQL-stored procedure basic command
            // old name: cmd
            // new name: spCommand
            var spCommand = connection.CreateCommand();
            spCommand.CommandType = System.Data.CommandType.StoredProcedure;
            spCommand.CommandText = storedProcedure;

            // context => SQL-parameter for user first name
            // old name: pFirstName
            // new name: parameterUserFirstName
            SqlParameter parameterUserFirstName = new SqlParameter()
            {
                ParameterName = "@FirstName",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.FirstName}"
            };

            // context => SQL-parameter for user last name
            // old name: pLastName
            // new name: parameterUserLastName
            SqlParameter parameterUserLastName = new SqlParameter()
            {
                ParameterName = "@LastName",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.LastName}"
            };

            // context => SQL-parameter for user picture
            // old name: pPicture
            // new name: parameterUserPicture
            SqlParameter parameterUserPicture = new SqlParameter()
            {
                ParameterName = "@Picture",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.Picture}"
            };

            // context => SQL-parameter for user title
            // old name: pTitle
            // new name: parameterUserTitle
            SqlParameter parameterUserTitle = new SqlParameter()
            {
                ParameterName = "@Title",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.Title}"
            };

            // context => SQL-parameter for user gender
            // old name: pGender
            // new name: parameterUserGender
            SqlParameter parameterUserGender = new SqlParameter()
            {
                ParameterName = "@Gender",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.Gender}"
            };

            // context => SQL-parameter for user email
            // old name: pEmail
            // new name: parameterUserEmail
            SqlParameter parameterUserEmail = new SqlParameter()
            {
                ParameterName = "@Email",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.Email}"
            };

            // context => SQL-parameter for user date birth
            // old name: pDateBirth
            // new name: parameterUserDateBirth
            SqlParameter parameterUserDateBirth = new SqlParameter()
            {
                ParameterName = "@DateBirth",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Direction = System.Data.ParameterDirection.Input,
                Value = $"{user.DateBirth}"
            };

            spCommand.Parameters.Add(parameterUserFirstName);
            spCommand.Parameters.Add(parameterUserLastName);
            spCommand.Parameters.Add(parameterUserPicture);
            spCommand.Parameters.Add(parameterUserTitle);
            spCommand.Parameters.Add(parameterUserGender);
            spCommand.Parameters.Add(parameterUserEmail);
            spCommand.Parameters.Add(parameterUserDateBirth);

            return spCommand;
        }
    }
}
