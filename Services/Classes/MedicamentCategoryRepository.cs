using Pharmacix.DatabaseContexts;
using Pharmacix.Models.Classes;
using Pharmacix.Services.Interfaces;

namespace Pharmacix.Services;

public class MedicamentCategoryRepository : IRepository<MedicamentCategory>
{
    private readonly PharmacixDbContext _dbContext;
    
    public MedicamentCategoryRepository(PharmacixDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool Create(MedicamentCategory category)
    {
        try
        {
            bool success = _dbContext.MedicamentCategories.Add(category) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(MedicamentCategory category)
    {
        try
        {
            bool success = _dbContext.MedicamentCategories.Update(category) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(MedicamentCategory category)
    {
        try
        {
            bool success = _dbContext.MedicamentCategories.Remove(category) is not null;
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
            var target = _dbContext.MedicamentCategories.FirstOrDefault(category => category.Id == id);
            bool success = _dbContext.MedicamentCategories.Remove(target) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public List<MedicamentCategory> GetAll()
    {
        return _dbContext.MedicamentCategories
            .OrderByDescending(category => category.Id)
            .ToList();
    }

    public MedicamentCategory GetById(int id)
    {
        return _dbContext.MedicamentCategories.FirstOrDefault(category => category.Id == id) ?? null;
    }
}