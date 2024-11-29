using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuestionAndJokeSit.Controllers
{
    public class Lab8Controller : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://lab.vntu.org/api-server/lab8.php");
            string json = httpClient.GetStringAsync("?user=student&pass=p@ssw0rd").Result;
            var users = JsonSerializer.Deserialize<List<List<User>>>(json);

            return View(users);
        }
    }
    public class User
    {
        [JsonInclude]
        public string name;
        [JsonInclude]
        public string affiliation;
        [JsonInclude]
        public string rank;
        [JsonInclude]
        public string location;
    }
}
