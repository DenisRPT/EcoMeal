namespace EcoMeal.Backend.Models;

public class OrderGetDTO
{
    public int Id{get;set;}
    public required string PackageName{get;set;}
    public required string Status{get;set;}
    public double Price{get;set;}
    public int BusinessId{get;set;}
    public required string BusinessName{get;set;}
    public DateTime Date{get;set;}
    public DateTime PickupStart{get;set;}
    public DateTime PickupEnd{get;set;}
    public string? UserName{get;set;}
    public string? UserContact{get;set;}
}