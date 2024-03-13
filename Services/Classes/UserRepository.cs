using System.Security.Cryptography;
using System.Text;
using Pharmacix.DatabaseContexts;
using Pharmacix.Models.Classes;
using Pharmacix.Services.Interfaces;

namespace Pharmacix.Services;

public class UserRepository : IRepository<User>
{
    private readonly PharmacixDbContext _dbContext;
    
    public UserRepository(PharmacixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Create(User user)
    {
        try
        {
            bool success = _dbContext.Users.Add(user) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Update(User user)
    {
        try
        {
            bool success = _dbContext.Users.Update(user) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(User user)
    {
        try
        {
            bool success = _dbContext.Users.Remove(user) is not null;
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
            var target = _dbContext.Users.FirstOrDefault(user => user.Id == id);
            bool success = _dbContext.Users.Remove(target) is not null;
            _dbContext.SaveChanges();
            return success;
        }
        catch
        {
            return false;
        }
    }

    public List<User> GetAll()
    {
        return _dbContext.Users
            .OrderByDescending(u => u.Id)
            .ToList();
    }

    public User GetById(int id)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Id == id) ?? null;
    }

    public User GetByUsername(string username)
    {
        return _dbContext.Users.FirstOrDefault(user => user.Username == username) ?? null;
    }
    
    public bool ChangePassword(int userId, string oldPassword, string newPassword)
    {
        var user = GetById(userId);
        if (user is null) return false;

        if (!IsPasswordCorrect(user.Username, oldPassword)) return false;

        user.PasswordHash = Sha256(newPassword);
        _dbContext.SaveChanges();
        return true;
    }
    
    public bool IsPasswordCorrect(string username, string password)
    {
        var user = GetByUsername(username);
        if (user == null) return false;
        if (user.PasswordHash == Sha256(password)) return true;

        return false;
    }

    public bool DoesUserExist(string username)
    {
        var target = _dbContext.Users.FirstOrDefault(user => user.Username == username);
        return !(target == null);
    }
    
    private string Sha256(string inputString)
    {
        var crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(inputString));
        
        foreach (byte theByte in crypto)
        {
            hash += theByte.ToString("x2");
        }
        
        return hash;
    }
}