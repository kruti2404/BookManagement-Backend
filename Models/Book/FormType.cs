namespace Models.Book
{
    public class FormType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }

    }
}
