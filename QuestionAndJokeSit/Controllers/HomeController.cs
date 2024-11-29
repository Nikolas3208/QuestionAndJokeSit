using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using QuestionAndJokeSit.Models;

namespace QuestionAndJokeSit.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public List<QuestionModel> questions = null;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            questions = Questions.GetQuestions();

            return View(questions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                Questions.SetQuestions(ReadFile(webHostEnvironment.WebRootPath + path));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public string SaveMyAnswers(string? userName, string? email)
        {
            if (!System.IO.File.Exists("Answers.txt"))
            {
                System.IO.File.Create("Answers.txt").Close();
            }


            string text = System.IO.File.ReadAllText("Answers.txt") + "\n\n";

            string text2 = $"Імя користувача: {userName},\n" +
                $"Електронна адреса: {email},\n" +
                $"Час відправки відповіді: {DateTime.Now.Date}\n";

            for (int i = 0; i < Questions.GetQuestions().Count; i++)
            {
                text += $"\nВідповідь{i + 1}: {Request.Form[$"Answer_{i + 1}"]}";
            }
            text += text2;


            System.IO.File.WriteAllText("Answers.txt", text);

            return $"Дякую за проходження опитування: {userName}\n" +
                $"Ваші відповіді: {text2}";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<QuestionModel> ReadFile(string path)
        {
            List<QuestionModel> questionModels = null;
            QuestionModel questionModel = null;
            int i = 0;
            using (XmlReader reader = XmlReader.Create(System.IO.File.OpenRead(path), new XmlReaderSettings()))
            {
                reader.MoveToContent();
                if (reader.Name == "questions")
                {
                    bool options = false;
                    bool type = false;
                    questionModels = new List<QuestionModel>();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "question")
                            {
                                questionModel = new QuestionModel();
                            }
                            else if(reader.Name == "type")
                            {
                                type = true;
                            }
                            else if(reader.Name == "options")
                            {
                                if (questionModel != null)
                                    questionModel.questions = new List<string>();
                                options = true;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.Text)
                        {
                            if (questionModel != null && options == true)
                            {
                                questionModel.questions.Add(reader.Value);
                            }
                            else if (questionModel != null && type == true)
                            {
                                questionModel.QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), reader.Value);
                            }
                            else if (questionModel != null && type == false && options == false)
                            {
                                questionModel.QuestionText = reader.Value;
                            }
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            if(reader.Name == "type")
                            {
                                type = false;
                            }
                            else if(reader.Name == "options")
                            {
                                options = false;
                            }
                            else if (reader.Name == "question")
                            {
                                questionModel.Id = questionModels.Count + 1;
                                questionModels.Add(questionModel);
                            }

                        }
                    }
                }
            }

            return questionModels;
        }
    }
}
