using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Text.Json;

namespace Codehelper.WebServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {

        public StatisticController(string cnnSting) => cnn = new MySqlConnection(cnnSting);
        
        MySqlConnection cnn;

        //http://www.website.ru/Statistic/AddSolution
        [HttpPost(Name = "AddSolution")]
        public void Set(string json)
        {
            cnn.Open();
            var req = JsonSerializer.Deserialize<Statistic>(json);
            var cmd = cnn.CreateCommand();
            cmd.CommandText = $@"INSERT INTO History
                VALUES({req.ErrorCode}, {req.Description}, {req.Link}, {req.SuccessRate})";
        }
        //http://www.website.ru/Statistic/GetStatistic?=errcode
        [HttpGet(Name = "GetStatistic")]
        public IEnumerable<string> Get(string errCode)
        {
            cnn.Open();
            var cmd = cnn.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM History WHERE error_code = '{errCode}'";
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                yield return JsonSerializer.Serialize<Statistic>(
                    new Statistic
                {
                    Id = reader.GetFieldValue<int>(0),
                    ErrorCode = reader.GetFieldValue<string>(2),
                    Description = reader.GetFieldValue<string>(3),
                    Link = reader.GetFieldValue<string>(4)
                });
            }
        }
    }
}