﻿// <auto-generated />
using System;
using FptUOverflow.Infra.EfCore.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FptUOverflow.Infra.EfCore.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250224134742_AddColumnToPresentInnerHtmlQuestion")]
    partial class AddColumnToPresentInnerHtmlQuestion
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.AnswerVote", b =>
                {
                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("AnswerId", "CreatedBy");

                    b.HasIndex("CreatedBy");

                    b.ToTable("AnswerVotes");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AboutMe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DetailProblem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DetailProblemHtml")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Expecting")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpectingHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.QuestionTag", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("QuestionId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("QuestionTags");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.QuestionVote", b =>
                {
                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("QuestionId", "CreatedBy");

                    b.HasIndex("CreatedBy");

                    b.ToTable("QuestionVotes");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("TagName")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.TagUser", b =>
                {
                    b.Property<Guid>("TagId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TagId", "CreatedBy");

                    b.HasIndex("CreatedBy");

                    b.ToTable("TagUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Answer", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", "CreatedUser")
                        .WithMany("Answers")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("FptUOverflow.Infra.EfCore.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.AnswerVote", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.Answer", "Answer")
                        .WithMany("AnswerVotes")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", "CreatedUser")
                        .WithMany("AnswerVotes")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("CreatedUser");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Question", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", "CreatedUser")
                        .WithMany("Questions")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.QuestionTag", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FptUOverflow.Infra.EfCore.Models.Tag", "Tag")
                        .WithMany("QuestionTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.QuestionVote", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", "CreatedUser")
                        .WithMany("QuestionVotes")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FptUOverflow.Infra.EfCore.Models.Question", "Question")
                        .WithMany("QuestionVotes")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Tag", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.TagUser", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", "CreatedUser")
                        .WithMany("TagUsers")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FptUOverflow.Infra.EfCore.Models.Tag", "Tag")
                        .WithMany("TagUsers")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedUser");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FptUOverflow.Infra.EfCore.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Answer", b =>
                {
                    b.Navigation("AnswerVotes");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.ApplicationUser", b =>
                {
                    b.Navigation("AnswerVotes");

                    b.Navigation("Answers");

                    b.Navigation("QuestionVotes");

                    b.Navigation("Questions");

                    b.Navigation("TagUsers");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Question", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("QuestionVotes");
                });

            modelBuilder.Entity("FptUOverflow.Infra.EfCore.Models.Tag", b =>
                {
                    b.Navigation("QuestionTags");

                    b.Navigation("TagUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
