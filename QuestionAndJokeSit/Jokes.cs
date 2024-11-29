using QuestionAndJokeSit.Models;
using System.Xml;

namespace QuestionAndJokeSit
{
    public static class Jokes
    {
        private static List<JokeModel> jokes;
        public static List<JokeModel> Next(string xmlPath)
        {
            jokes = new List<JokeModel>();

            XmlReaderSettings settings = new XmlReaderSettings();

            using (XmlReader reader = XmlReader.Create(File.OpenRead(xmlPath), settings))
            {
                reader.MoveToContent();
                JokeModel joke = null;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        joke = new JokeModel();
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        if (joke != null)
                            joke.Content = reader.Value;
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        jokes.Add(joke);
                    }
                }
            }

            return jokes;
        }
    }
}
