using ClassLibraryDatabaseConnection;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCmsv2025.Repository
{
    public class UserRepositoryImpl
    {
        // ConnectionString from App.config
        string winConnString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        #region 1- Implement login functionality with database
        public async Task<int> AuthenticateUserByRoleIdAsyn(string username, string password)
        {
            // Declare the variable for storing the return type - RoleId
            int roleId = 0;
            try
            {
                // sql connection -- connectionstring
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    SqlCommand command = new SqlCommand("sp_GetUserNamePassword", conn);

                    // CommandType
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Password", password);

                    // Output parameter
                    SqlParameter roleIdParameter = new SqlParameter("@RoleId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };

                    command.Parameters.Add(roleIdParameter);
                    await command.ExecuteNonQueryAsync();

                    // DbNull
                    if (roleIdParameter.Value != DBNull.Value)
                    {
                        roleId = Convert.ToInt32(roleIdParameter.Value);
                    }
                    return roleId;
                }
            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error occured :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured :" + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
