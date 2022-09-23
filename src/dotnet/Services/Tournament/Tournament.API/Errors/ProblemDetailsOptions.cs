namespace Tournament.API.Errors;

public class ProblemDetailsOptions
{
    public Action<ProblemDetailsContext>? CustomizeProblemDetails { get; set; }
}