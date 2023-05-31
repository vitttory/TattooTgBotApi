using MySqlConnector;

namespace TattooTgBotApi
{
    public class ConnectDB
    {

        private MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;database=botdb;password=dfg5p04U*Tuk;Allow User Variables=true");
        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
