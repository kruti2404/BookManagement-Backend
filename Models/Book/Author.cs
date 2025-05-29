namespace Models.Book
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }        
        public int Age { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateOnly DeathDate { get; set; }
        public string Nationality { get; set; }
        public string Biography { get; set; }
        public string email { get; set; }
        public string awards {  get; set; }
        public virtual ICollection<Book> Books { get; set; }


    }
}
