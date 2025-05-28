namespace Models.Book
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }        
        public int Age { get; set; }

        public virtual ICollection<Book> Books { get; set; }


    }
}
