using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NyscIdentify.Common.Infrastructure.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Data
{
    public class Context : DbContext
    {
        #region Properties
        public DbSet<User> Users { get; set; }
        public DbSet<ResourceBase> Resources { get; set; }

        #region Internals
        SQLiteConnection Connection { get; set; }
        #endregion

        #endregion

        #region Methods

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            await ConfigureConnection();
            optionsBuilder.UseSqlite(Connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());
        }

        async Task ConfigureConnection()
        {
            Connection = new SQLiteConnection($"Data Source={Core.DATABASE_PATH}");
            await Connection.OpenAsync();
        }
        #endregion
    }
}
