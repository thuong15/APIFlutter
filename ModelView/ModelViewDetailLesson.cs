namespace project4.ModelView
{
    public class ModelViewDetailLesson
    {
        public string LessonCode { get; set; }
    }
    public class ItemAddSentence
    {
        public string LessonCode { get; set; }
        public string Sentences { get; set; }
        public string Description { get; set; }
    }
    public class ItemAddQuestion
    {
        public string LessonCode { get; set; }
        public string Question { get; set; }
        public string Avatar { get; set; }
        public string ListAnswer { get; set; }
    }
}
