using Microsoft.AspNetCore.Components;
using EcoMeal.Site.Services;
using EcoMeal.Site.Models;

namespace EcoMeal.Site.Components.Pages.Orders;

public partial class Orders
{
    [Inject]
    public required OrderService OrderService {get;set;}
    private List<OrderGetModel>? MyOrders;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender){
            MyOrders = await OrderService.GetMyOrderAsync();
            StateHasChanged();
        }
    }
}