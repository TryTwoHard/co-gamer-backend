using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tournament.API.Exceptions;

namespace Tournament.API.Controllers;

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
        else
        {
            statusCode = 403;
        }
        return Problem(statusCode: statusCode, detail: exception.Message);
    }
}