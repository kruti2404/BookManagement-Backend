using System.ComponentModel;

namespace Models.Book
{
    public class Publication
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        [DisplayName("Established Year")]
        public DateOnly Established { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    
    }

}
