namespace project4.ModelView
{
    public class ModelViewLearnWord
    {
    }
    public class ItemGetDataQuestion
    {
        public string Code { get; set; }
    }
    public class ItemAnswer
    {
        public string Code { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsChoose { get; set; }
        public string NameEN { get; set; }
        public string NameVN { get; set; }
    }
    public class ResultGetDataQuestion
    {
        public string Avatar { get; set; }
        public string NameEN { get; set; }
        public List<ItemAnswer> listAnswer { get; set; }
    }
}
