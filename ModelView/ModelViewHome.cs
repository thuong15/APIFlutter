namespace project4.ModelView
{
    public class ModelViewHome
    {
        public string test { get; set; }
    }
    public class Item
    {
        public string Code { get; set; }
    }
    public class ItemGetListLesson
    {
        public string CodeUser { get; set; }
        public string CodeTopic { get; set; }
    }
    public class ResultCombineSentences
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ListAnswer { get; set; }
    }
}
