using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class createBookDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public string AuthorName { get; set; }
        public string CategoriesName { get; set; }
        public string PublicationName { get; set; }
        public string FormType {  get; set; }
            }
}
