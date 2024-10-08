﻿using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.DatabaseContext
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(options =>
            {
                options.HasIndex(user => user.Email)
                    .IsUnique();
                options.Property(user => user.UserName)
                    .HasMaxLength(500)
                    .IsUnicode(true);
            });
        }
    }
}
