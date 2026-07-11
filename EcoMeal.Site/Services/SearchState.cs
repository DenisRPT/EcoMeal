namespace EcoMeal.Site.Services;

public class SearchState
{
    private string _query = string.Empty;
    public string Query
    {
        get => _query;
        set
        {
            if (_query == value) return;
            _query = value;
            OnChanged?.Invoke();
        }
    }

    public event Action? OnChanged;
}
