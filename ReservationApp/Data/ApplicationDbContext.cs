using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;

namespace ReservationApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Rafael",
                    PasswordHash = HashPassword("admin1"),
                    Email = "testMail@gmail.com",
                    IsAdministrator = true,
                },
                new User
                {
                    Id = 2,
                    Name = "RandomUser",
                    PasswordHash = HashPassword("user1"),
                    Email = "user_testMail@gmail.com",
                    IsAdministrator = false,
                });
            //no needed due inherits directly from DbContext
            //base.OnModelCreating(modelBuilder);
        }

        //The HashPassword method is used to convert a user's plain password into a secure, hashed form using the SHA-256 algorithm.
        public static string HashPassword(string password)
        {
            //it consumes system resources (e.g., native cryptographic resources) that must be released after use. Thats why it is in using
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            };
        }

    }
}
