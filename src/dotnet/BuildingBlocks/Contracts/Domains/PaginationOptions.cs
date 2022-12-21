namespace Contracts.Domains.Implementations;

public class PaginationObject<T, K> 
    where T : EntityBase<K>
{
    private readonly int _pageSize;
    public PaginationObject(int? pageSize)
    {
        _pageSize = pageSize ?? 0;
    }

    public List<T> Content { get; set; } = new();
    public int Total => Content.Count;

    public int PageCount
    {
        get
        {
            if (_pageSize > 0)
            {
                return (int)Math.Ceiling((decimal)Total / _pageSize);
            }
            
            return Total > 0 ? 1 : 0;
        }
    }
}