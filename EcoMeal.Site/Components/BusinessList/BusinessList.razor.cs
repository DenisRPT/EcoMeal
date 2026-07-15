using EcoMeal.Site.Models;
using EcoMeal.Site.Services;
using Microsoft.AspNetCore.Components;

namespace EcoMeal.Site.Components.BusinessList;

public partial class BusinessList : IDisposable
{
    [Inject]
    public required BusinessService BusinessService { get; set; }

    [Inject]
    public required SearchState SearchState { get; set; }

    private List<BusinessModel>? Businesses { get; set; }
    private List<BusinessTypeModel> BusinessTypes { get; set; } = new();
    private Dictionary<string, List<BusinessModel>>? GroupedBusinesses { get; set; }
    private Dictionary<string, List<BusinessModel>> GroupedBusinessesView => GroupedBusinesses ?? new Dictionary<string, List<BusinessModel>>();
    private string SelectedType { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        SearchState.OnChanged += OnSearchChanged;
        Businesses = await BusinessService.GetAllAsync();
        BusinessTypes = await BusinessService.GetBusinessTypesAsync();
        BuildGroups();
    }

    private void OnSearchChanged()
    {
        BuildGroups();
        InvokeAsync(StateHasChanged);
    }

    private void BuildGroups()
    {
        if (Businesses is null)
        {
            GroupedBusinesses = null;
            return;
        }

        var query = SearchState.Query?.Trim() ?? string.Empty;
        var filtered = string.IsNullOrWhiteSpace(query)
            ? Businesses
            : Businesses.Where(b => b.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                || (b.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) == true)
                || b.BusinessTypeName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var groupedBusinesses = filtered
            .GroupBy(b => b.BusinessTypeName)
            .ToDictionary(g => g.Key, g => g.ToList());

        GroupedBusinesses = BusinessTypes
            .OrderBy(bt => bt.Name)
            .ToDictionary(
                bt => bt.Name,
                bt => groupedBusinesses.TryGetValue(bt.Name, out var businesses) ? businesses : new List<BusinessModel>());

        if (!GroupedBusinesses.ContainsKey(SelectedType))
        {
            SelectedType = string.Empty;
        }
    }

    private void SelectType(string type)
    {
        SelectedType = SelectedType == type ? string.Empty : type;
    }

    private void ClearSelectedType()
    {
        SelectedType = string.Empty;
    }

    public void HandleDeleted(int id)
    {
        if (Businesses is null) return;
        Businesses.RemoveAll(b => b.Id == id);
        BuildGroups();
        StateHasChanged();
    }

    public void Dispose()
    {
        SearchState.OnChanged -= OnSearchChanged;
    }
}
