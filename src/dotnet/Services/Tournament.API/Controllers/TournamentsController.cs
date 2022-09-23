using Microsoft.AspNetCore.Mvc;
using Tournament.API.Controllers.Payloads.Requests;
using Tournament.API.Services.Interfaces.Tournament;

namespace Tournament.API.Controllers;

/// <summary>
/// Tournament service
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _service;
    private readonly ILogger<TournamentsController> _logger;
    public TournamentsController(ITournamentService service, ILogger<TournamentsController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get information of a tournament
    /// </summary>
    /// <param name="id">id of the tournament to be retrieved</param>
    /// <returns>An IActionResult representing the tournament retrieved</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTournamentInformation(Guid id)
    {
        var tournament = await _service.GetTournamentById(id);

        // if (tournament is null)
        // {
        //     return NotFound();
        // }

        return Ok(tournament);
    }

    /// <summary>
    /// Get a page of public tournaments
    /// </summary>
    /// <returns>An IActionResult representing a list of tournaments</returns>
    [HttpGet("get-public")]
    public async Task<IActionResult> GetPublicTournaments()
        // int? pageIndex, 
        // int? pageSize, 
        // int? hostId, 
        // int? game
    {
        var tournaments = await _service.GetTournaments();
        return Ok(tournaments);
    }

    /// <summary>
    /// Draft a tournament
    /// </summary>
    /// <param name="tournamentDraft">The tournament to be drafted</param>
    /// <returns>An IActionResult representing the task of drafting a tournament</returns>
    [HttpPost("draft")]
    public async Task<IActionResult> DraftTournament([FromBody] DraftTournamentRequest tournamentDraft)
    {
        var tournamentToReturn = await _service.CreateNewTournament(tournamentDraft);
        return Ok(tournamentToReturn);
    }
    
    /// <summary>
    /// Validate a tournament for publish
    /// </summary>
    /// <param name="id">id of the tournament to be validated</param>
    /// <param name="idFromBody">id of the tournament to be validated but from body</param>
    /// <returns>An IActionResult representing the task of validating a tournament</returns>
    [HttpPost("validate-publish/{id:guid}")]
    public async Task<IActionResult> ValidatePublishTournament([FromQuery] Guid id,
        [FromBody] Guid idFromBody)
    {
        var payload = new Dictionary<string, String>();
        id = idFromBody;
        try
        {
            await _service.ValidateTournamentToPublish(id);
            payload.Add("result", true.ToString());
        }
        catch (Exception e)
        {
            payload.Add("result", false.ToString());
            payload.Add("error", e.Message);
        }

        return Ok(payload);
    }

    /// <summary>
    /// Publish a tournament
    /// </summary>
    /// <param name="id">id of the drafted tournament to be published</param>
    /// <returns>An IActionResult representing the task of publishing a tournament</returns>
    [HttpPost("publish/{id:guid}")]
    public async Task<IActionResult> PublishTournament(Guid id)
    {
        var tournament = await _service.PublishTournament(id);
        return Ok(tournament);
    }

    /// <summary>
    /// Update a tournament
    /// </summary>
    /// <param name="id">id of the tournament to be updated</param>
    /// <param name="updatedTournament">The tournament updated informations</param>
    /// <returns>An IActionResult representing the task of updating a tournament</returns>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTournament(Guid id, UpdateTournamentRequest updatedTournament)
    {
        // var tournamentEntity = await _service.GetTournamentById(id);
        // if (tournamentEntity is null)
        // {
        //     return NotFound();
        // }
        
        var tournamentToReturn = await _service.UpdateTournament(id, updatedTournament);
        return Ok(tournamentToReturn);
    }

    /// <summary>
    /// Cancel a tournament
    /// </summary>
    /// <param name="id">id of the tournament to be canceled</param>
    /// <returns>An IActionResult representing the task of canceling a tournament</returns>
    [HttpPost("cancel/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTournament(Guid id)
    {
        // var tournamentEntity = await _service.GetTournamentById(id);
        // if (tournamentEntity is null)
        // {
        //     return NotFound();
        // }

        var canceledTournament= await _service.CancelTournament(id);
        return Ok(canceledTournament);
    }

    /// <summary>
    /// Delete a tournament
    /// </summary>
    /// <param name="id">id of the tournament to be canceled</param>
    /// <returns>An IActionResult representing the task of deleting a tournament</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTournament(Guid id)
    {
        await _service.DeleteTournament(id);
        return NoContent();
    }
}