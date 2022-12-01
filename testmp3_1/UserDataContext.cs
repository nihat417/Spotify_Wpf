using Microsoft.EntityFrameworkCore;
using testmp3_1.Models;

namespace testmp3_1;

public class UserDataContext : DbContext
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source = {App.path}\\DataFile.db");
    }

    public DbSet<User> users { get; set; }
}

