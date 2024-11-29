using QuestionAndJokeSit.Models;

namespace QuestionAndJokeSit
{
    public static class Questions
    {
        private static List<QuestionModel> _questions;

        public static void SetQuestions(List<QuestionModel> questions)
            => _questions = questions;

        public static List<QuestionModel> GetQuestions()
            => _questions;
    }
}
