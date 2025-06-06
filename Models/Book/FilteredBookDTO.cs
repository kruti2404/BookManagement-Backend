namespace Models.Book
{
    public class FilteredBookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NoOfPages { get; set; }
        public int Price { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string FormType { get; set; }
        public string Categories { get; set; }
    }
}
