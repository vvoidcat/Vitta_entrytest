using System.Configuration;

namespace VittatestApp.Model
{
    class ConnectionHelper
    {
        public static string GetValue(string connectionString)
        {
            return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
        }
    }
}
