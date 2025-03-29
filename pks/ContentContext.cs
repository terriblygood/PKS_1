using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pks.models;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;

public class ContentContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(
                "server=localhost;user=root;password=;database=pks;",
                new MySqlServerVersion(new Version(8, 0, 30))
            );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связей

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        // Настройка столбцов
        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<Book>()
            .Property(b => b.PublishYear)
            .IsRequired();

        modelBuilder.Entity<Author>()
            .Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(255);

        modelBuilder.Entity<Genre>()
            .Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(255);
    }
}
