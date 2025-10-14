using authentication.API.Models;
using Microsoft.EntityFrameworkCore;
using UserModel = authentication.API.Models.User;

namespace authentication.API.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext>    option): base(option) { }

        public DbSet<UserModel> Users { get; set; }

    }
}