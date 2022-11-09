namespace Tournament.API.Controllers;

public class QueryParameters
{
    private int _size = 30;
    private const int _maxSize = 50;
    public int Page { get; set; } = 1;

    public int Size
    {
        get
        {
            return _size;
        }
        set
        {
            _size = Math.Min(_maxSize, value);
        }
    }

    public string SortBy { get; set; } = String.Empty;
    
    private string _sortOrder = "asc";

    public string SortOrder
    {
        get
        {
            return _sortOrder;
        }
        set
        {
            if (value.Equals("asc") || value.Equals("desc"))
            {
                _sortOrder = value;
            }
        }
    }
}