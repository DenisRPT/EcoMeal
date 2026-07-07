using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.BusinessCard;

public partial class BusinessCard
{
    [Parameter]
    public required BusinessModel Business { get; set; }
    [Inject]
    public required BusinessService BusinessService { get; set; }
    [Parameter]
    public EventCallback<int> OnDeleted { get; set; }

    public async Task Delete()
    {
        var success = await BusinessService.DeleteAsync(Business.Id);
        if (success)
        {
            await OnDeleted.InvokeAsync(Business.Id);
        }
    }
}