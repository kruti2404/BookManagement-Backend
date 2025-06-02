using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Book;

namespace DTOs
{
    public  class bookResultDTO
    {
        public int totalCount;
        public IEnumerable<getBookDTO> books;
    }
}
