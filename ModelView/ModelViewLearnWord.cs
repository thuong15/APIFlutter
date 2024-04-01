namespace project4.ModelView
{
    public class ModelViewLearnWord
    {
    }
    public class ItemGetDataQuestion
    {
        public string Code { get; set; }
        public int Stt { get; set; }
    }
    public class ItemAnswer
    {
        public string Code { get; set; }
        public bool IsCorrect { get; set; }
        public string NameEN { get; set; }
        public string NameVN { get; set; }
    }
}
