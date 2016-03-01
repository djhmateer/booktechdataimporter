using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace Four
{
    public static class Util
    {
        public static string CapitaliseFirstLetter(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpper();
            return s.Remove(1).ToUpper() + s.Substring(1);
        }

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BookTechConnectionString"].ConnectionString);
            connection.Open();
            return connection;
        }
    }
}