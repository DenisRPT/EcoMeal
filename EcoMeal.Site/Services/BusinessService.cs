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

    public async Task CreateBusinessAsync(BusinessAddModel business)
    {
        var response = await _http.PostAsJsonAsync("api/business", business);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBusinessAsync(int businessId, BusinessUpdateModel business)
    {
        var response = await _http.PutAsJsonAsync($"api/business/{businessId}", business);
        response.EnsureSuccessStatusCode();
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