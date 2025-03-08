using FptUOverflow.Infra.EfCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<QuestionVote> QuestionVotes { get; set; }
        public DbSet<TagUser> TagUsers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerVote> AnswerVotes { get; set; }
        public DbSet<ImageUpload> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.DisplayName).IsRequired();
                entity.Property(e => e.Location).IsRequired(false);
                entity.Property(e => e.Title).IsRequired(false);
                entity.Property(e => e.AboutMe).IsRequired(false);
                entity.Property(e => e.ProfileImage).IsRequired(false);
                entity.HasMany(e => e.Tags)
                    .WithOne(e => e.CreatedUser)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.Questions)
                    .WithOne(e => e.CreatedUser)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.QuestionVotes)
                    .WithOne(e => e.CreatedUser)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.TagUsers)
                    .WithOne(e => e.CreatedUser)
                    .HasForeignKey(e => e.CreatedBy);
                entity.HasMany(e => e.Answers)
                    .WithOne(e => e.CreatedUser)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.AnswerVotes)
                    .WithOne(e => e.CreatedUser)
                    .HasForeignKey(e => e.CreatedBy)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.QuestionId)
                    .IsRequired();
                entity.Property(e => e.Content)
                    .IsRequired();
                entity.Property(e => e.CreatedBy)
                    .IsRequired(false);
                entity.Property(e => e.IsApproved)
                    .IsRequired();
                entity.HasMany(e => e.AnswerVotes)
                    .WithOne(e => e.Answer)
                    .HasForeignKey(e => e.AnswerId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<AnswerVote>(entity =>
            {
                entity.HasKey(e => new { e.AnswerId, e.CreatedBy });
                entity.Property(e => e.AnswerId)
                    .IsRequired();
                entity.Property(e => e.CreatedBy)
                    .IsRequired();
                entity.Property(e => e.Score)
                    .IsRequired();
            });

            builder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Title)
                    .IsRequired();
                entity.Property(e => e.DetailProblem)
                    .IsRequired();
                
                entity.Property(e => e.Expecting)
                    .IsRequired(false);
               
                entity.Property(e => e.CreatedBy)
                    .IsRequired();
                entity.HasMany(e => e.Answers)
                    .WithOne(e => e.Question)
                    .HasForeignKey(e => e.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.QuestionVotes)
                    .WithOne(e => e.Question)
                    .HasForeignKey(e => e.QuestionId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<QuestionTag>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.TagId });
                entity.Property(e => e.QuestionId)
                    .IsRequired();
                entity.Property(e => e.TagId)
                    .IsRequired();
            });

            builder.Entity<QuestionVote>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.CreatedBy });
                entity.Property(e => e.QuestionId)
                    .IsRequired();
                entity.Property(e => e.CreatedBy)
                    .IsRequired();
                entity.Property(e => e.Score)
                    .IsRequired();
            });

            builder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.TagName)
                    .IsRequired();
                entity.HasIndex(e => e.TagName).IsUnique();
                entity.Property(e => e.CreatedBy)
                    .IsRequired();
                entity.HasMany(e => e.QuestionTags)
                    .WithOne(e => e.Tag)
                    .HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.NoAction);
                entity.HasMany(e => e.TagUsers)
                    .WithOne(e => e.Tag)
                    .HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<TagUser>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.CreatedBy });
                entity.Property(e => e.TagId)
                    .IsRequired();
                entity.Property(e => e.CreatedBy)
                    .IsRequired();
            });
            builder.Entity<ImageUpload>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.PublicId)
                    .IsRequired();
                entity.Property(e => e.Url)
                    .IsRequired();
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                entity.Property(e => e.UpdatedAt)
                    .IsRequired();
            });
        }
    }
}
