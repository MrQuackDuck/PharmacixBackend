namespace Pharmacix.Models.Classes.AcceptModels;

public class CreateMedicamentModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Amount { get; set; }
    public int CategoryId { get; set; }
}