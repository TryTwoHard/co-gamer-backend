namespace Tournament.API.Controllers.ErrorsController;

public class ProblemDetailsOptions
{
    public Action<ProblemDetailsContext>? CustomizeProblemDetails { get; set; }
}