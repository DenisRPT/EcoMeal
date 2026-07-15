using System.Net.Http.Headers;
using EcoMeal.Site.Models;

namespace EcoMeal.Site.Services;

public class OrderService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;
    public OrderService(HttpClient http,AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<bool> PlaceOrderAsync(int packageId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/order")
        {
            Content = JsonContent.Create(new { PackageId = packageId })
        };
        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<OrderGetModel>> GetMyOrderAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/order/my");
        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return new List<OrderGetModel>();
        }

        var orders = await response.Content.ReadFromJsonAsync<List<OrderGetModel>>();
        return orders ?? new List<OrderGetModel>();
    }

    public async Task<List<OrderGetModel>> GetAllOrdersForAdminAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/order/admin");
        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return new List<OrderGetModel>();
        }

        var orders = await response.Content.ReadFromJsonAsync<List<OrderGetModel>>();
        return orders ?? new List<OrderGetModel>();
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/order/{orderId}/status")
        {
            Content = JsonContent.Create(new { Status = status })
        };

        await AddAuthHeaderAsync(request);

        var response = await _http.SendAsync(request);
        return response.IsSuccessStatusCode;
    }


    private async Task AddAuthHeaderAsync(HttpRequestMessage request)
    {
        if (string.IsNullOrEmpty(_authService.Token))
        {
            await _authService.LoadTokenAsync();
        }

        if (!string.IsNullOrEmpty(_authService.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
        }
    }


   
}