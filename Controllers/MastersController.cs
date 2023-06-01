using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TattooTgBotApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MastersController : Controller
    {
        [HttpPost("PostFavMasters")]
        public async Task<FavouriteMaster> PutFavMastersAsync([FromBody] FavouriteMaster favouriteMaster)
        {
            DataTable table = new DataTable();
            ConnectDB connectDB = new ConnectDB();
            MySqlCommand command = new MySqlCommand("INSERT IGNORE INTO favmasters(Age, Name, Sex) VALUES (@Age, @Name, @Sex)", connectDB.GetConnection());
            
            command.Parameters.Add("@Age", MySqlDbType.Int32).Value = favouriteMaster.Age;
            command.Parameters.Add("@Name", MySqlDbType.String).Value = favouriteMaster.Name;
            command.Parameters.Add("@Sex", MySqlDbType.String).Value = favouriteMaster.Sex;

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(table);

            return favouriteMaster;

        }
            //DataTable table = new DataTable();
            //ConnectDB dbConnect = new ConnectDB();
            //MySqlCommand command = new MySqlCommand($"INSERT IGNORE INTO favmasters(Id, Age, Name, Sex) VALUES ({favouriteMaster.Id}, {favouriteMaster.Age}, {favouriteMaster.Name}, {favouriteMaster.Sex})", dbConnect.GetConnection());
            //command.ExecuteNonQuery();

        

        [HttpGet("GetMastersList")]
        public async Task<List<Master>> GetMastersList()
        {
            List<Master> list = new List<Master>();
            ConnectDB connectDB = new ConnectDB();

            using (MySqlConnection connection = connectDB.GetConnection())
            {
                MySqlCommand command = new MySqlCommand("SELECT Age, Name, Sex FROM masters", connection);

                await connection.OpenAsync();

                MySqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var read = new Master
                        {                           
                            Age = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1),
                            Sex = dataReader.GetString(2)

                        };
                        list.Add(read);
                    }
                }
                connection.Close();
            }
            return list;
        }
        [HttpGet("GetFavMastersList")]
        public async Task<List<Master>> GetFavMastersList()
        {
            List<Master> FavMlist = new List<Master>();
            ConnectDB connectDB = new ConnectDB();

            using (MySqlConnection connection = connectDB.GetConnection())
            {
                MySqlCommand command = new MySqlCommand("SELECT Age, Name, Sex FROM favmasters", connection);

                await connection.OpenAsync();

                MySqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        var readFM = new Master
                        {
                            Age = dataReader.GetInt32(0),
                            Name = dataReader.GetString(1),
                            Sex = dataReader.GetString(2)

                        };
                        FavMlist.Add(readFM);
                    }
                }
                connection.Close();
            }
            return FavMlist;
        }
        [HttpDelete("DeleteFavMaster")]
        public async Task<int> DeleteFavMasterAsync(int Age, string Name, string Sex)
        {
            ConnectDB connectDB = new ConnectDB();
            using (MySqlConnection connection = connectDB.GetConnection())
            {
                await connection.OpenAsync();
                MySqlCommand command = new MySqlCommand("DELETE FROM favmasters WHERE Age = @Age AND Name = @Name AND Sex = @Sex", connection);

                command.Parameters.Add("@Age", MySqlDbType.Int32).Value = Age;
                command.Parameters.Add("@Name", MySqlDbType.String).Value = Name;
                command.Parameters.Add("@Sex", MySqlDbType.String).Value = Sex;

                int deletedRows = await command.ExecuteNonQueryAsync();

                connection.Close();

                return deletedRows;
            }
        }

    }
    
}
