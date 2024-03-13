using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacix.Models.Classes;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

public class MedicamentController : Controller
{
    private readonly MedicamentRepository _medicamentRepository;

    public MedicamentController(MedicamentRepository medicamentRepository)
    {
        _medicamentRepository = medicamentRepository;
    }
    
    [HttpGet]
    [Authorize]
    public ActionResult<List<Medicament>> GetAll()
    {
        return _medicamentRepository.GetAll();
    }
    
    [HttpGet]
    [Authorize]
    public ActionResult<Medicament> GetById(int id)
    {
        var target = _medicamentRepository.GetById(id) ?? null;
        if (target is null) return NotFound();
        return target;
    }
}