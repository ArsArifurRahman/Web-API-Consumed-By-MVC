using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Critique> Critiques { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookGenre> BookGenres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Author
        modelBuilder.Entity<Author>()
            .HasIndex(a => new { a.FirstName, a.LastName })
            .IsUnique();
        modelBuilder.Entity<Author>()
            .HasMany(a => a.BookAuthors)
            .WithOne(ba => ba.Author)
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Author>()
            .HasOne(a => a.Country)
            .WithMany(c => c.Authors)
            .HasForeignKey(a => a.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Isbn)
            .IsUnique();
        modelBuilder.Entity<Book>()
            .HasIndex(b => b.Title)
            .IsUnique();
        modelBuilder.Entity<Book>()
            .Property(b => b.PublishedAt)
            .HasColumnType("Date");

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Book>()
            .HasMany(b => b.BookAuthors)
            .WithOne(ba => ba.Book)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Book>()
            .HasMany(b => b.BookGenres)
            .WithOne(bg => bg.Book)
            .HasForeignKey(bg => bg.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        // Country
        modelBuilder.Entity<Country>()
            .HasIndex(c => c.Name)
            .IsUnique();

        // Critique
        modelBuilder.Entity<Critique>()
            .HasOne(c => c.Book)
            .WithMany(b => b.Reviews)
            .HasForeignKey(c => c.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Critique>()
            .HasOne(c => c.Reviewer)
            .WithMany(r => r.Critiques)
            .HasForeignKey(c => c.ReviewerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Genre
        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();
        modelBuilder.Entity<Genre>()
            .HasMany(g => g.BookGenres)
            .WithOne(bg => bg.Genre)
            .HasForeignKey(bg => bg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Reviewer
        modelBuilder.Entity<Reviewer>()
            .HasIndex(r => new { r.FirstName, r.LastName })
            .IsUnique();
        modelBuilder.Entity<Reviewer>()
            .HasMany(r => r.Critiques)
            .WithOne(c => c.Reviewer)
            .HasForeignKey(c => c.ReviewerId)
            .OnDelete(DeleteBehavior.Cascade);

        // BookAuthor
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        // BookGenre
        modelBuilder.Entity<BookGenre>()
            .HasKey(bg => new { bg.BookId, bg.GenreId });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
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

        // Seed data for Genre
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Fiction" },
            new Genre { Id = 2, Name = "Non-Fiction" },
            new Genre { Id = 3, Name = "Biography" },
            new Genre { Id = 4, Name = "Children's" },
            new Genre { Id = 5, Name = "Mystery" }
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
        modelBuilder.Entity<Critique>().HasData(
            new Critique { Id = 1, Headline = "Terrifying Read", ReviewText = "Stephen King's 'It' is a terrifying journey into the depths of fear.", Rating = 5, BookId = 1, ReviewerId = 1 },
            new Critique { Id = 2, Headline = "Magical Adventure", ReviewText = "Rowling's debut novel is a magical adventure for all ages.", Rating = 5, BookId = 2, ReviewerId = 2 },
            new Critique { Id = 3, Headline = "Dystopian Masterpiece", ReviewText = "Atwood's 'The Handmaid's Tale' is a chilling vision of a dystopian future.", Rating = 5, BookId = 3, ReviewerId = 3 },
            new Critique { Id = 4, Headline = "Australian Classic", ReviewText = "Winton's 'Cloudstreet' is a sprawling epic of Australian life.", Rating = 4, BookId = 4, ReviewerId = 4 },
            new Critique { Id = 5, Headline = "Beautiful and Heartbreaking", ReviewText = "Roy's debut novel is a beautiful and heartbreaking tale of love and loss.", Rating = 4, BookId = 5, ReviewerId = 5 }
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
        modelBuilder.Entity<BookGenre>().HasData(
            new BookGenre { BookId = 1, GenreId = 5 },
            new BookGenre { BookId = 2, GenreId = 4 },
            new BookGenre { BookId = 3, GenreId = 1 },
            new BookGenre { BookId = 4, GenreId = 1 },
            new BookGenre { BookId = 5, GenreId = 1 }
        );
    }
}