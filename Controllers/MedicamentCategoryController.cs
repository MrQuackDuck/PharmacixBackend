using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacix.Models.Classes;
using Pharmacix.Models.Classes.AcceptModels;
using Pharmacix.Models.Classes.AcceptModels.MedicamentCategory;
using Pharmacix.Services;

namespace Pharmacix.Controllers;

public class MedicamentCategoryController : Controller
{
    private readonly MedicamentRepository _medicamentRepository;
    private readonly MedicamentCategoryRepository _medicamentCategoryRepository;

    public MedicamentCategoryController(MedicamentRepository medicamentRepository, MedicamentCategoryRepository medicamentCategoryRepository)
    {
        _medicamentRepository = medicamentRepository;
        _medicamentCategoryRepository = medicamentCategoryRepository;
    }
    
    [HttpGet]
    [Authorize]
    public ActionResult<List<MedicamentCategory>> GetAll()
    {
        return _medicamentCategoryRepository.GetAll();
    }
    
    [HttpGet]
    [Authorize]
    public ActionResult<MedicamentCategory> GetById(int id)
    {
        var target = _medicamentCategoryRepository.GetById(id) ?? null;
        if (target is null) return NotFound();
        return target;
    }
    
    [HttpPost]
    [Authorize]
    public ActionResult<bool> Create([FromBody]CreateMedicamentCategoryModel model)
    {
        if (model is null) return BadRequest();

        var success = this._medicamentCategoryRepository.Create(
            new MedicamentCategory()
            {
                Name = model.Name
            });

        if (success) return Ok();
        else return BadRequest();
    }
    
    [HttpPut]
    [Authorize]
    public ActionResult<bool> Edit([FromBody]EditMedicamentCategoryModel model)
    {
        if (model is null) return BadRequest();

        var category = _medicamentCategoryRepository.GetById(model.Id);

        if (category is null) return NotFound("Provided category wasn't found");

        category.Name = model.Name;
        var success = this._medicamentCategoryRepository.Update(category);

        if (success) return Ok();
        else return BadRequest();
    }
    
    [HttpPost]
    [Authorize]
    public ActionResult<bool> Delete([FromBody]DeleteMedicamentCategoryModel model)
    {
        var medicament = _medicamentCategoryRepository.GetById(model.Id);

        if (medicament is null) return NotFound("Provided category wasn't found");
        
        bool success = _medicamentCategoryRepository.Delete(medicament);
        
        if (success) return Ok();
        else return BadRequest();
    }
}