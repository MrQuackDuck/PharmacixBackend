using Microsoft.EntityFrameworkCore;
using Pharmacix.Models.Classes;

namespace Pharmacix.DatabaseContexts;

public class PharmacixDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Medicament> Medicines { get; set; }
    public DbSet<MedicamentCategory> MedicamentCategories { get; set; }
    
    public string DbPath { get; }

    public PharmacixDbContext() => DbPath = "data.db";
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}