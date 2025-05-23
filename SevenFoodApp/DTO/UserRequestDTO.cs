using static SevenFoodApp.Util.Enums;
public class UserRequestDTO
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public TYPE_USER Type { get; set; }

}