
namespace EcoMeal.Backend.Entities;

public class Package
{
    public int Id {get;set;}
    public required string Name {get;set;}
    public int NOOfMeals {get;set;}
    public int BusinessId {get;set;}
    public Business Business {get;set;}
    public int PackageTypeId {get;set;}
    public  PackageType PackageType {get;set;}
    public string? Description {get;set;}
    public decimal Price {get;set;}
    public DateTime PickupStart {get;set;}
    public DateTime PickupEnd {get;set;}
    public ICollection<Order> Orders {get;set;} = new List<Order>();
}