using Pharmacix.DatabaseContexts;
using Pharmacix.Models.Classes;
using Pharmacix.Services.Interfaces;

namespace Pharmacix.Services;

public class MedicamentRepository : IRepository<Medicament>
{
    private readonly PharmacixDbContext _dbContext;
    
    public MedicamentRepository(PharmacixDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool Create(Medicament medicament)
    {
        try
        {
            bool success = _dbContext.Medicines.Add(medicament) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(Medicament medicament)
    {
        try
        {
            bool success = _dbContext.Medicines.Update(medicament) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Medicament medicament)
    {
        try
        {
            bool success = _dbContext.Medicines.Remove(medicament) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            var target = _dbContext.Medicines.FirstOrDefault(medicament => medicament.Id == id);
            bool success = _dbContext.Medicines.Remove(target) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public List<Medicament> GetAll()
    {
        return _dbContext.Medicines
            .OrderByDescending(medicament => medicament.Id)
            .ToList();
    }

    public Medicament? GetById(int id)
    {
        return _dbContext.Medicines.FirstOrDefault(medicament => medicament.Id == id) ?? null;
    }
}