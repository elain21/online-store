using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class OnlineStoreContext : DbContext
{
    public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; } = null!;
}