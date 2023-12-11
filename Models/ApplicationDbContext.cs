using Microsoft.EntityFrameworkCore;

namespace ClientApp.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<GenderModel> Genders { get; set; }
    public DbSet<CountryModel> Countries { get; set; }

}