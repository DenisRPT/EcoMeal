using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.BusinessList;

public partial class BusinessList
{
    [Inject]
    public required BusinessService BusinessService { get; set; }
    private List<BusinessModel>? Businesses { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Businesses = await BusinessService.GetAllAsync();
    }

    public void HandleDeleted(int id)
    {
        if (Businesses is null) return;
        Businesses.RemoveAll(b => b.Id == id);
        StateHasChanged();
    }
}