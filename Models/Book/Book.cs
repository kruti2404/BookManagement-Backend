using System;
using System.ComponentModel;
using Models.Book;

namespace Models.Book
{
    public class Book
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayName("No of Pages")]
        public int NoOfPages { get; set; }
        public string Language {  get; set; }
        public int Price { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PublicationId { get; set; }
        public Guid FormTypeId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Publication Publication { get; set; }
        public virtual FormType FormType { get; set; }



    }
}
