namespace Tournament.API.Exceptions;

public class TournamentNotFoundException : Exception
{
    public TournamentNotFoundException(string message) : base(message)
    {
    }
}