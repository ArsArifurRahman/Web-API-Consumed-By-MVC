using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Book { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Reviewer> Reviewers { get; set; }
    public virtual DbSet<BookAuthor> BookAuthors { get; set; }
    public virtual DbSet<BookCategory> BookCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Author & Country relationship
        modelBuilder.Entity<Author>()
            .HasOne(a => a.Country)
            .WithMany(c => c.Authors)
            .HasForeignKey(a => a.CountryId);

        // Configure Book & Review relationship
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId);

        // Configure Book & BookAuthor relationship
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);

        // Configure Book & BookCategory relationship
        modelBuilder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);

        // Configure Review & Reviewer relationship
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany(rev => rev.Reviews)
            .HasForeignKey(r => r.ReviewerId);

        // Seed data for Country
        modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "United States" },
                new Country { Id = 2, Name = "United Kingdom" },
                new Country { Id = 3, Name = "Canada" },
                new Country { Id = 4, Name = "Australia" },
                new Country { Id = 5, Name = "India" }
            );

        // Seed data for Author
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "Stephen", LastName = "King", CountryId = 1 },
            new Author { Id = 2, FirstName = "J.K.", LastName = "Rowling", CountryId = 2 },
            new Author { Id = 3, FirstName = "Margaret", LastName = "Atwood", CountryId = 3 },
            new Author { Id = 4, FirstName = "Tim", LastName = "Winton", CountryId = 4 },
            new Author { Id = 5, FirstName = "Arundhati", LastName = "Roy", CountryId = 5 }
        );

        // Seed data for Category
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Fiction" },
            new Category { Id = 2, Name = "Non-Fiction" },
            new Category { Id = 3, Name = "Biography" },
            new Category { Id = 4, Name = "Children's" },
            new Category { Id = 5, Name = "Mystery" }
        );

        // Seed data for Book
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Isbn = "9781501142970", Title = "It", PublishedAt = DateTimeOffset.Parse("1986-09-15") },
            new Book { Id = 2, Isbn = "9780747532743", Title = "Harry Potter and the Philosopher's Stone", PublishedAt = DateTimeOffset.Parse("1997-06-26") },
            new Book { Id = 3, Isbn = "9780385490818", Title = "The Handmaid's Tale", PublishedAt = DateTimeOffset.Parse("1985-02-17") },
            new Book { Id = 4, Isbn = "9780330412388", Title = "Cloudstreet", PublishedAt = DateTimeOffset.Parse("1991-05-23") },
            new Book { Id = 5, Isbn = "9780006550686", Title = "The God of Small Things", PublishedAt = DateTimeOffset.Parse("1997-05-01") }
        );

        // Seed data for Reviewer
        modelBuilder.Entity<Reviewer>().HasData(
            new Reviewer { Id = 1, FirstName = "James", LastName = "Wood" },
            new Reviewer { Id = 2, FirstName = "Michiko", LastName = "Kakutani" },
            new Reviewer { Id = 3, FirstName = "Ron", LastName = "Charles" },
            new Reviewer { Id = 4, FirstName = "Maureen", LastName = "Corrigan" },
            new Reviewer { Id = 5, FirstName = "Dwight", LastName = "Garner" }
        );


        // Seed data for Review
        modelBuilder.Entity<Review>().HasData(
            new Review { Id = 1, Headline = "Terrifying Read", ReviewText = "Stephen King's 'It' is a terrifying journey into the depths of fear.", Rating = 5, BookId = 1, ReviewerId = 1 },
            new Review { Id = 2, Headline = "Magical Adventure", ReviewText = "Rowling's debut novel is a magical adventure for all ages.", Rating = 5, BookId = 2, ReviewerId = 2 },
            new Review { Id = 3, Headline = "Dystopian Masterpiece", ReviewText = "Atwood's 'The Handmaid's Tale' is a chilling vision of a dystopian future.", Rating = 5, BookId = 3, ReviewerId = 3 },
            new Review { Id = 4, Headline = "Australian Classic", ReviewText = "Winton's 'Cloudstreet' is a sprawling epic of Australian life.", Rating = 4, BookId = 4, ReviewerId = 4 },
            new Review { Id = 5, Headline = "Beautiful and Heartbreaking", ReviewText = "Roy's debut novel is a beautiful and heartbreaking tale of love and loss.", Rating = 4, BookId = 5, ReviewerId = 5 }
        );

        // Seed data for BookAuthor
        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor { BookId = 1, AuthorId = 1 },
            new BookAuthor { BookId = 2, AuthorId = 2 },
            new BookAuthor { BookId = 3, AuthorId = 3 },
            new BookAuthor { BookId = 4, AuthorId = 4 },
            new BookAuthor { BookId = 5, AuthorId = 5 }
        );

        // Seed data for BookCategory
        modelBuilder.Entity<BookCategory>().HasData(
            new BookCategory { BookId = 1, CategoryId = 5 },
            new BookCategory { BookId = 2, CategoryId = 4 },
            new BookCategory { BookId = 3, CategoryId = 1 },
            new BookCategory { BookId = 4, CategoryId = 1 },
            new BookCategory { BookId = 5, CategoryId = 1 }
        );
    }
}
