using System.ComponentModel;

namespace Models.Book
{
    public class Publication
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Established Year")]
        public DateOnly Established { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string Location { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    
    }

}
