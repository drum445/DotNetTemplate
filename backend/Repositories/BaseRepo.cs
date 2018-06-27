using MySql.Data.MySqlClient;

namespace backend.Repositories
{
    public class BaseRepo
    {
        public MySqlConnection GetConn()
        {
            return new MySqlConnection
            {
                ConnectionString = "server=localhost;user id=root;password=password;persistsecurityinfo=True;port=3306;database=test;SslMode=None"
            };
        }
    }
}