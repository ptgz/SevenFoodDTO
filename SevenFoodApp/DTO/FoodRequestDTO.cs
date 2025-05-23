using SevenFoodApp.Model;
using static SevenFoodApp.Util.Enums;
public class FoodRequestDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool Status { get; set; }
    public Restaurant Restaurant { get; set; }

}