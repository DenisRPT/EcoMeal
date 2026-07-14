namespace EcoMeal.Site.Models;

public class OrderGetModel
{
    public int Id{get;set;}
    public DateTime Date{get;set;}
    public string? PackageName{get;set;}
    public string? BusinessName{get;set;}
    public string? UserName{get;set;}
    public string? UserContact{get;set;}
    public string Status{get;set;} = "";
    public double Price{get;set;}
    public int BusinessID{get;set;}
}