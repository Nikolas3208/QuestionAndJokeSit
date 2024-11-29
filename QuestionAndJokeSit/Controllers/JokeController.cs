using Microsoft.AspNetCore.Mvc;
using QuestionAndJokeSit.Models;

namespace QuestionAndJokeSit.Controllers
{
    public class JokeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public List<JokeModel> jokes;

        public JokeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Joke()
        {
            jokes = Jokes.Next(_webHostEnvironment.WebRootPath + "/joke.xml");

            JokeModel jokeModel = jokes[Random.Shared.Next(0, jokes.Count)];

            return View(jokeModel);
        }
    }
}
