namespace Contracts.Domains.Implementations;

public class PaginationOptions
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
}