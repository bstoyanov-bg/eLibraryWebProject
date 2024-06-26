﻿using LibraryManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Data.Configuration
{
    /// <summary>
    /// This configuration class is used for custom additions to the fluent api of the LendedBooks entity.
    /// </summary> 

    public class LendedBookConfiguration : IEntityTypeConfiguration<LendedBook>
    {
        public void Configure(EntityTypeBuilder<LendedBook> builder)
        {
            builder.HasKey(lb => lb.Id);

            builder
                .HasOne(lb => lb.Book)
                .WithMany(b => b.LendedBook)
                .HasForeignKey(lb => lb.BookId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasOne(lb => lb.User)
                .WithMany(u => u.LendedBooks)
                .HasForeignKey(lb => lb.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
