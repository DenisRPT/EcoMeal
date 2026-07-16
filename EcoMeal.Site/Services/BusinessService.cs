using EcoMeal.Site.Models;

namespace EcoMeal.Site.Services;

public class BusinessService
{
    private readonly HttpClient _http;
    public BusinessService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<BusinessModel>> GetAllAsync()
    {
        var businesses = await _http.GetFromJsonAsync<List<BusinessModel>>("api/business");
        return businesses ?? new List<BusinessModel>();
    }

    public string? BuildBusinessImageUrl(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return null;
        }

        if (Uri.TryCreate(imagePath, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.ToString();
        }

        if (_http.BaseAddress is null)
        {
            return imagePath;
        }

        var apiBase = _http.BaseAddress.ToString().TrimEnd('/');
        var relativePath = imagePath.TrimStart('/');
        return $"{apiBase}/{relativePath}";
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/business/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<BusinessDetailsModel?> GetOneById(int id)
    {
        var business = await _http.GetFromJsonAsync<BusinessDetailsModel>($"api/business/{id}");

        return business;
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateBusinessAsync(MultipartFormDataContent business)
    {
        var response = await _http.PostAsync("api/business", business);
        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            errorMessage = $"HTTP {(int)response.StatusCode} ({response.StatusCode})";
        }

        return (false, errorMessage);
    }

    public async Task UpdateBusinessAsync(int businessId, BusinessUpdateModel business)
    {
        var response = await _http.PutAsJsonAsync($"api/business/{businessId}", business);
        response.EnsureSuccessStatusCode();
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateBusinessAsync(int businessId, MultipartFormDataContent business)
    {
        var response = await _http.PutAsync($"api/business/{businessId}", business);
        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var errorMessage = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(errorMessage))
        {
            errorMessage = $"HTTP {(int)response.StatusCode} ({response.StatusCode})";
        }

        return (false, errorMessage);
    }

    public async Task AddPackageToBusiness(int businessId, PackageAddModel PackageAddModel)
    {
        var response = await _http.PostAsJsonAsync($"api/business/{businessId}/addPackage", PackageAddModel);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<BusinessTypeModel>> GetBusinessTypesAsync()
    {
        var businessTypes = await _http.GetFromJsonAsync<List<BusinessTypeModel>>("api/business/businessTypes");
        return businessTypes ?? new List<BusinessTypeModel>();
    }

    public async Task<List<PackageTypeModel>> GetPackageTypesAsync()
    {
        var packageTypes = await _http.GetFromJsonAsync<List<PackageTypeModel>>("api/business/packageTypes");
        return packageTypes ?? new List<PackageTypeModel>();
    }

    public async Task UpdatePackageAsync(int businessId, int packageId, PackageEditModel package)
    {
        var response = await _http.PutAsJsonAsync($"api/business/{businessId}/packages/{packageId}", package);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeletePackageAsync(int businessId, int packageId)
    {
        var response = await _http.DeleteAsync($"api/business/{businessId}/packages/{packageId}");
        response.EnsureSuccessStatusCode();
    }
}