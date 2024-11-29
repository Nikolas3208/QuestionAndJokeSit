namespace QuestionAndJokeSit.Models
{
    public enum QuestionType
    {
        Radio,
        CheckBox,
        Text
    }
    public class QuestionModel
    {
        public QuestionType QuestionType { get; set; } = QuestionType.Radio;

        public int Id { get; set; } = 0;

        public string QuestionText { get; set; } = string.Empty;

        public List<string> questions = null;
    }
}
