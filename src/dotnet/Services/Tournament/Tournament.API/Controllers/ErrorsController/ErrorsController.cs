using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tournament.API.Exceptions;

namespace Tournament.API.Controllers.ErrorsController;

/// <summary>
/// 
/// </summary>
public class ErrorsController : ControllerBase
{
    /// <summary>
    /// Produces error
    /// </summary>
    /// <returns>An IActionResult</returns>
    [Route("/error")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Index()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        int statusCode;
        if (exception is TournamentNotFoundException)
        {
            statusCode = 404;
        }
        else if (exception is TournamentInvalidException)
        {
            statusCode = 400;
        }
        else if (exception is BadHttpRequestException)
        {
            statusCode = 400;
        }
        else
        {
            statusCode = 500;
        }
        return Problem(statusCode: statusCode, detail: exception.Message);
    }
}