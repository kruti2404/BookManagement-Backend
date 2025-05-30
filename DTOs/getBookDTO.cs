using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Book;

namespace DTOs
{
    public class getBookDTO
    {
        public getBookDTO(Guid id, string title, string description, int pages, int price, string language, ICollection<Category> categories, Author author, Publication publisher)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.pages = pages;
            this.Categories = categories != null
                        ? string.Join(", ", categories.Select(c => c.Name))
                        : string.Empty;



            this.author = author.Name;
            this.publisher = publisher.Name;
            this.price = price;
            this.language = language;
        }
        public Guid id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int pages { get; set; }
        public string Categories { get; set; }
        public string author { get; set; }
        public string publisher { get; set; }
        public int price { get; set; }
        public string language { get; set; }
    }
}
