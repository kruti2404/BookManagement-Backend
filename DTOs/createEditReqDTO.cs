using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class createEditReqDTO
    {

        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Pages { get; set; }
        public string? Price { get; set; }
        public string? Language { get; set; }
        public Guid AuthorId { get; set; }
        public Guid formTypeID { get; set; }
        public string CategoriesTD { get; set; }
        public Guid PublisherId { get; set; }
    }
}
