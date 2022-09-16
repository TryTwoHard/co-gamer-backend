using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities.Tournament;
using Tournament.API.Services.Interfaces.Tournament;

namespace Tournament.API.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _service;
    private readonly IMapper _mapper;
    public TournamentsController(ITournamentService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTournamentInformation(Guid id)
    {
        var tournament = await _service.GetTournamentById(id);

        if (tournament is null)
        {
            return NotFound();
        }

        return Ok(tournament);
    }

    [HttpGet("get-public")]
    public async Task<IActionResult> GetPublicTournaments()
    {
        var tournaments = await _service.GetTournaments();
        return Ok(tournaments);
    }

    [HttpPost("draft")]
    public async Task<IActionResult> DraftTournament([FromBody] TournamentCreationDTO tournamentDraft)
    {
        var tournament = _mapper.Map<TournamentDTO>(tournamentDraft);
        await _service.CreateNewTournament(tournament);
        return Ok(tournament);
    }
    
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

    [HttpPost("publish/{id:guid}")]
    public async Task<IActionResult> PublishTournament(Guid id)
    {
        await _service.PublishTournament(id);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTournament(Guid id, TournamentUpdateDTO updatedTournament)
    {
        var tournamentEntity = await _service.GetTournamentById(id);
        if (tournamentEntity is null)
        {
            return NotFound();
        }
        
        var tournament = _mapper.Map<TournamentDTO>(updatedTournament);
        await _service.UpdateTournament(tournament);
        return Ok(tournament);
    }

    [HttpPost("cancel/{id:guid}")]
    public async Task<IActionResult> CancelTournament(Guid id)
    {
        var tournamentEntity = await _service.GetTournamentById(id);
        if (tournamentEntity is null)
        {
            return NotFound();
        }

        await _service.CancelTournament(id);
        var canceledTournament = await _service.GetTournamentById(id);
        var tournamentToReturn = _mapper.Map<TournamentDTO>(canceledTournament);

        return Ok(tournamentToReturn);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTournament(Guid id)
    {
        await _service.DeleteTournament(id);
        return NoContent();
    }
}