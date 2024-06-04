using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Category> Catagories { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Reviewer> Reviewers { get; set; }
    public virtual DbSet<BookAuthor> BookAuthors { get; set; }
    public virtual DbSet<BookCategory> BookCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Author>()
            .HasOne(a => a.Country)
            .WithMany(c => c.Authors)
            .HasForeignKey(a => a.CountryId);

        builder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);

        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);

        builder.Entity<BookCategory>()
            .HasKey(bc => new { bc.BookId, bc.CategoryId });

        builder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);

        builder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);

        builder.Entity<Review>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(r => r.BookId);

        builder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany(rev => rev.Reviews)
            .HasForeignKey(r => r.ReviewerId);

        builder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "John", LastName = "Doe", CountryId = 1 },
            new Author { Id = 2, FirstName = "Jane", LastName = "Doe", CountryId = 1 },
            new Author { Id = 3, FirstName = "Alice", LastName = "Smith", CountryId = 2 },
            new Author { Id = 4, FirstName = "Bob", LastName = "Johnson", CountryId = 2 },
            new Author { Id = 5, FirstName = "Charlie", LastName = "Brown", CountryId = 3 }
        );

        builder.Entity<Book>().HasData(
            new Book { Id = 1, Isbn = "1234567890", Title = "Book 1", PublishedAt = DateTimeOffset.Now },
            new Book { Id = 2, Isbn = "2345678901", Title = "Book 2", PublishedAt = DateTimeOffset.Now },
            new Book { Id = 3, Isbn = "3456789012", Title = "Book 3", PublishedAt = DateTimeOffset.Now },
            new Book { Id = 4, Isbn = "4567890123", Title = "Book 4", PublishedAt = DateTimeOffset.Now },
            new Book { Id = 5, Isbn = "5678901234", Title = "Book 5", PublishedAt = DateTimeOffset.Now }
        );

        builder.Entity<BookAuthor>().HasData(
            new BookAuthor { BookId = 1, AuthorId = 1 },
            new BookAuthor { BookId = 2, AuthorId = 2 },
            new BookAuthor { BookId = 3, AuthorId = 3 },
            new BookAuthor { BookId = 4, AuthorId = 4 },
            new BookAuthor { BookId = 5, AuthorId = 5 }
        );

        builder.Entity<BookCategory>().HasData(
            new BookCategory { BookId = 1, CategoryId = 1 },
            new BookCategory { BookId = 2, CategoryId = 2 },
            new BookCategory { BookId = 3, CategoryId = 3 },
            new BookCategory { BookId = 4, CategoryId = 4 },
            new BookCategory { BookId = 5, CategoryId = 5 }
        );

        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" },
            new Category { Id = 3, Name = "Category 3" },
            new Category { Id = 4, Name = "Category 4" },
            new Category { Id = 5, Name = "Category 5" }
        );

        builder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Country 1" },
            new Country { Id = 2, Name = "Country 2" },
            new Country { Id = 3, Name = "Country 3" },
            new Country { Id = 4, Name = "Country 4" },
            new Country { Id = 5, Name = "Country 5" }
        );

        builder.Entity<Review>().HasData(
            new Review { Id = 1, Headline = "Review 1", ReviewText = "This is review 1", Rating = 5, BookId = 1, ReviewerId = 1 },
            new Review { Id = 2, Headline = "Review 2", ReviewText = "This is review 2", Rating = 4, BookId = 2, ReviewerId = 2 },
            new Review { Id = 3, Headline = "Review 3", ReviewText = "This is review 3", Rating = 3, BookId = 3, ReviewerId = 3 },
            new Review { Id = 4, Headline = "Review 4", ReviewText = "This is review 4", Rating = 2, BookId = 4, ReviewerId = 4 },
            new Review { Id = 5, Headline = "Review 5", ReviewText = "This is review 5", Rating = 1, BookId = 5, ReviewerId = 5 }
        );

        builder.Entity<Reviewer>().HasData(
            new Reviewer { Id = 1, FirstName = "Reviewer 1", LastName = "Last 1" },
            new Reviewer { Id = 2, FirstName = "Reviewer 2", LastName = "Last 2" },
            new Reviewer { Id = 3, FirstName = "Reviewer 3", LastName = "Last 3" },
            new Reviewer { Id = 4, FirstName = "Reviewer 4", LastName = "Last 4" },
            new Reviewer { Id = 5, FirstName = "Reviewer 5", LastName = "Last 5" }
        );
    }
}
