using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class filterBookDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Pages { get; set; }
        public string? Price { get; set; }
        public string? Language { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public string? Publisher { get; set; }
        public string form { get; set; }

        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }
}
