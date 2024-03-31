using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacix.Models.Classes;
using Pharmacix.Models.Classes.AcceptModels;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

[Route("API/[controller]/[action]")]
public class MedicamentController : Controller
{
    private readonly MedicamentRepository _medicamentRepository;
    private readonly MedicamentCategoryRepository _medicamentCategoryRepository;

    public MedicamentController(MedicamentRepository medicamentRepository, MedicamentCategoryRepository medicamentCategoryRepository)
    {
        _medicamentRepository = medicamentRepository;
        _medicamentCategoryRepository = medicamentCategoryRepository;
    }
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpPost]
    [Authorize]
    public ActionResult<bool> Create([FromBody]CreateMedicamentModel model)
    {
        if (model is null) return BadRequest();
        
        var category = this._medicamentCategoryRepository.GetById(model.CategoryId);
        if (category is null) return NotFound("Provided category wasn't found");
        
        var success = this._medicamentRepository.Create(
            new Medicament()
            {
                Title = model.Title,
                Amount = model.Amount,
                Category = category,
                Price = model.Price,
            });

        if (success) return Ok();
        else return BadRequest();
    }
    
    [HttpPut]
    [Authorize]
    public ActionResult<bool> Edit([FromBody]EditMedicamentModel model)
    {
        if (model is null) return BadRequest();

        var medicament = _medicamentRepository.GetById(model.Id);

        if (medicament is null) return NotFound("Provided medicament wasn't found");
        
        var updatedCategory = this._medicamentCategoryRepository.GetById(model.CategoryId);
        if (updatedCategory is null) return NotFound("Provided category wasn't found");
        
        medicament.Title = model.Title;
        medicament.Amount = model.Amount;
        medicament.Price = model.Price;
        medicament.Category = updatedCategory;
        var success = this._medicamentRepository.Update(medicament);

        if (success) return Ok();
        else return BadRequest();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult<bool> Delete(int id)
    {
        var medicament = _medicamentRepository.GetById(id);

        if (medicament is null) return NotFound("Provided medicament wasn't found");
        
        bool success = _medicamentRepository.Delete(medicament);
        
        if (success) return Ok();
        else return BadRequest();
    }
}