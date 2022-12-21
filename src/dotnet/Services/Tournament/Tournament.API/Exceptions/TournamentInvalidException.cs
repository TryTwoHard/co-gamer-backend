namespace Tournament.API.Exceptions;

public class TournamentInvalidException : Exception
{
    public TournamentInvalidException(string message) : base(message)
    {
        
    }
}