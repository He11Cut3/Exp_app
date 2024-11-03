using System.Collections.Generic;
using System.Reflection.Emit;
using System;
using Microsoft.EntityFrameworkCore;
using Expenses_App.Models;

namespace Expenses_App.Infrastructure
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExpenseCategory>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseCategory>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }

    }
}
