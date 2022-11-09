namespace Tournament.API.Helpers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow { get; set; } = DateTime.Now;
}

public interface IDateTimeProvider
{
    public DateTime DateTimeNow { get; set; }
}