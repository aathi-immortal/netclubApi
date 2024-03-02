using Microsoft.Data.SqlClient;
using NetClubApi.Model;

namespace NetClubApi.Helper
{
    public class sqlHelper
    {
        public static string conStr;
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(conStr);
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
