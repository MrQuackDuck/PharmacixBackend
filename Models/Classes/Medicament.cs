using System.ComponentModel.DataAnnotations;

namespace Pharmacix.Models.Classes;

public class Medicament
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Amount { get; set; }
    
    public MedicamentCategory Category { get; set; }
}