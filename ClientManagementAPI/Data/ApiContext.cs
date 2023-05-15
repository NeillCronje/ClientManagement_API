using ClientManagementAPI.Models;
using ClientManagementAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ClientManagementAPI.Data
{
    public class ApiContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ClientDb");
        }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Address = "1 Blue Street",
                    Age = 32,
                    City = "Pretoria",
                    CreatedDate = DateTime.Now,
                    FirstName = "Neill",
                    LastName = "Cronje",
                    Region = "Gauteng",
                    PostalCode = "0044",
                    Email = "neillcronje@gmail.com",
                });
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 2,
                    Address = "123 Red Street",
                    Age = 30,
                    City = "Cape Town",
                    CreatedDate = DateTime.Now,
                    FirstName = "Gideon",
                    LastName = "Dewd",
                    Region = "Western Cape",
                    PostalCode = "5521",
                    Email = "randomdude04@gmail.com",
                });
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 3,
                    Address = "7 Law Street",
                    Age = 44,
                    City = "Johannesburg",
                    CreatedDate = DateTime.Now,
                    FirstName = "Ivan",
                    LastName = "Smith",
                    Region = "Gauteng",
                    PostalCode = "1223",
                    Email = "hippylover52@gmail.com",
                });
        }
    }
}
