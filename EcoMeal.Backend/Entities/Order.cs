namespace EcoMeal.Backend.Entities;

public class Order
{
    public int Id { get; set; }
    public int PackageId { get; set; }
    public  Package Package { get; set; }
    public int UserId { get; set; }
    public  User User { get; set; }
    public  string Status { get; set; }
    public DateTime OrderDate { get; set; }
}