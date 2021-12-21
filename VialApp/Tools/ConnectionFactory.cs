using System.Data;
using System.Data.SqlClient;
namespace VialApp.Tools
{
    public static class ConnectionFactory
    {
        public static IDbConnection CreateConnection(string ConnectionId = "VialApp")
        {
            string cnnstr = Configuration.GetConnectionString(ConnectionId);
            return new SqlConnection(cnnstr);
            
        }
    }
}
