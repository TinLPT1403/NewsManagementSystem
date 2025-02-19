using Microsoft.EntityFrameworkCore;
using DAL.Entities;
using DAL.Extensions;

namespace DAL.Data
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }
        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data if necessary
            modelBuilder.Seed();

            // Configure the relationship between NewsArticle and Category
            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.Category)
                .WithMany()
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the Many-to-Many relationship between NewsArticle and Tag through NewsTag
            modelBuilder.Entity<NewsTag>()
                .HasKey(nt => new { nt.NewsArticleId, nt.TagId });

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(na => na.NewsTags)
                .HasForeignKey(nt => nt.NewsArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            // Self-referencing Category Relationship (ParentCategory)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships for CreatedBy and UpdatedBy in NewsArticle
            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.CreatedBy)
                .WithMany(a => a.CreatedArticles)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.UpdatedBy)
                .WithMany(a => a.UpdatedArticles)
                .HasForeignKey(n => n.UpdatedById)
                .OnDelete(DeleteBehavior.NoAction);

        }

    }
}
