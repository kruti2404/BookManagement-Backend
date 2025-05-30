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
        public string Pages { get; set; }
        public string Price { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string Categories { get; set; }
        public string Publisher { get; set; }
        public string FormType { get; set; }
    }
}
