using Microsoft.EntityFrameworkCore;

namespace BookManagement_Backend.Data
{
    public class ProgramDbContent : DbContext
    {
        public ProgramDbContent(DbContextOptions<ProgramDbContent> options) : base(options)
        { }
    }
}
