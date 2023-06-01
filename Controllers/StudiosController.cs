using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Data;


namespace TattooTgBotApi.Controllers
{
    public class StudiosController : Controller
    {

        [HttpPost("PostFavStudios")]
        public async Task<FavouriteStudio> PostFavStudioAsync([FromBody] FavouriteStudio favouritestudio)
        {
            DataTable table = new DataTable();
            ConnectDB connectDB = new ConnectDB();
            MySqlCommand command = new MySqlCommand("INSERT IGNORE INTO favstudios(Name, Rating) VALUES (@Name, @Rating)", connectDB.GetConnection());
           
            command.Parameters.Add("@Name", MySqlDbType.String).Value = favouritestudio.Name;
            command.Parameters.Add("@Rating", MySqlDbType.Int32).Value = favouritestudio.Rating;

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(table);

            return favouritestudio;

        }
        //[HttpPost("DB/PostFavStudios")]

        //public async Task Post([FromBody] FavouriteStudio favouriteStudio)
        //{
        //    DataTable tableSt = new DataTable();
        //    ConnectDB dbConnectSt = new ConnectDB();
        //    MySqlCommand command = new MySqlCommand($"INSERT IGNORE INTO favstudios(Id, Name, Rating) VALUES ({favouriteStudio.Id}, {favouriteStudio.Name}, {favouriteStudio.Rating})", dbConnectSt.GetConnection());
        //    command.ExecuteReader();
        //}

        [HttpGet("GetStudiosList")]
        public async Task<List<Studio>> GetStudiosList()
        {
            List<Studio> listSt = new List<Studio>();
            ConnectDB dBconnect = new ConnectDB();

            using (MySqlConnection connect = dBconnect.GetConnection())
            {
                MySqlCommand command = new MySqlCommand("SELECT Name, Rating FROM studios", connect);

                await connect.OpenAsync();

                MySqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var read = new Studio
                        {
                            Name = dataReader.GetString(0),
                            Rating = dataReader.GetInt32(1)
                        };
                        listSt.Add(read);
                    }
                }
                connect.Close();
            }
            return listSt;
        }
        [HttpGet("GetFavStudios")]
        public async Task<List<Studio>> GetFavStudios()
        {
            List<Studio> FavlistSt = new List<Studio>();
            ConnectDB dBconnect = new ConnectDB();

            using (MySqlConnection connect = dBconnect.GetConnection())
            {
                MySqlCommand command = new MySqlCommand("SELECT Name, Rating FROM favstudios", connect);

                await connect.OpenAsync();

                MySqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var read = new Studio
                        {
                            Name = dataReader.GetString(0),
                            Rating = dataReader.GetInt32(1)
                        };
                        FavlistSt.Add(read);
                    }
                }
                connect.Close();
            }
            return FavlistSt;
        }

            [HttpDelete("DeleteFavStudio")]
            public async Task<int> DeleteFavStudioAsync(string Name, int Rating)
            {
                ConnectDB connectDB = new ConnectDB();
                using (MySqlConnection connection = connectDB.GetConnection())
                {
                    MySqlCommand command = new MySqlCommand("DELETE FROM favstudios WHERE Name = @Name AND Rating = @Rating", connection);


                    command.Parameters.Add("@Name", MySqlDbType.String).Value = Name;
                    command.Parameters.Add("@Rating", MySqlDbType.Int32).Value = Rating; 

                    await connection.OpenAsync();

                    int deletedRows = await command.ExecuteNonQueryAsync();

                    connection.Close();

                    return deletedRows;
                }
            }
        
    }
}
