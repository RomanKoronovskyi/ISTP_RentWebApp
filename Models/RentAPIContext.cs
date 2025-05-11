using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RentAPIWebApp.Models
{
    public class RentAPIContext : DbContext
    {
        public RentAPIContext(DbContextOptions<RentAPIContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Realtors> Realtors { get; set; }
        public virtual DbSet<UsersHasRealtors> UsersHasRealtors { get; set; }
        public virtual DbSet<Flats> Flats { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Favourites> Favourites { get; set; }
    }
}