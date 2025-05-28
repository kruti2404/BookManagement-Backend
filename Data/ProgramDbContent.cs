using Models.Book;
using Microsoft.EntityFrameworkCore;
namespace Data
    
{
    public class ProgramDbContent : DbContext
    {
        public ProgramDbContent(DbContextOptions<ProgramDbContent> options) : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Cluster Index for books 
            modelBuilder.Entity<Book>()
                .HasKey(book => book.Id)
                .IsClustered();

            // many to many relationship with Book and Category  
            modelBuilder.Entity<Book>()
                .HasMany(book => book.Categories)
                .WithMany(category => category.Books);
            

            // one to many relationship with Book and Author
            modelBuilder.Entity<Book>()
                .HasOne(book => book.Author)
                .WithMany(author => author.Books)
                .HasForeignKey(book => book.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);


            // one to many relationship with Book and Publication
            modelBuilder.Entity<Book>()
                .HasOne(book => book.Publication)
                .WithMany(pub => pub.Books)
                .HasForeignKey(book => book.PublicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // one to many relationship with Book and FormType
            modelBuilder.Entity<Book>()
                .HasOne(book => book.FormType)
                .WithMany(formtype => formtype.Books)
                .HasForeignKey(book => book.FormTypeId)
                .OnDelete(DeleteBehavior.Cascade);


            //Cluster Index for category 
            modelBuilder.Entity<Category>()
                .HasKey(category => category.Id)
                .IsClustered();
            
            
            //Cluster Index for author 
            modelBuilder.Entity<Author>()
                .HasKey(author => author.Id)
                .IsClustered();
                
            //Cluster Index for FormType 
            modelBuilder.Entity<FormType>()
                .HasKey(Form => Form.Id)
                .IsClustered();


        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publication> Publications { get; set; }


    }
}
