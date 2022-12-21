using System.Data.Entity.Infrastructure.Design;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Tournament.API.Controllers.Payloads;
using Tournament.API.Controllers.Tournaments;
using Tournament.API.Exceptions;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities;
using Tournament.API.Models.Statuses;
using Tournament.API.Repositories.Interfaces;
using Tournament.API.Services.Implementations;

namespace Tournament.API.Tests.Unit.Application;

public class TournamentServiceTests
{
    private readonly TournamentService _sut;
    private readonly ITournamentRepository _repository = Substitute.For<ITournamentRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();

    public TournamentServiceTests()
    {
        _sut = new TournamentService(_repository, _mapper);
    }


    // [Fact]
    // public async Task GetTournaments_ShouldReturnEmptyList_WhenNoTournamentsExist()
    // {
    //     // Arrange
    //     var tournaments = Enumerable.Empty<TournamentEntity>().AsQueryable();
    //     _repository.GetTournamentsAsync().Returns(tournaments);
    //     var parameters = new TournamentQueryParameters()
    //     {
    //
    //     };
    //     var expected = new List<TournamentDTO>();
    //     _mapper.Map<List<TournamentDTO>>(tournaments.ToList()).Returns(expected);
    //     var page = new Page<TournamentEntity>(expected.Count) {Content = tournaments.ToList()};
    //     
    //     // Act
    //     var result = await _sut.GetTournaments(parameters);
    //
    //     // Assert
    //     result.Should().BeEquivalentTo(page);
    // }

    [Fact]
    public async Task GetTournamentById_ShouldReturnTournament_WhenThatTournamentExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournament = new TournamentEntity() {Id = id};
        var expectedTournament = new TournamentDTO();
        _repository.GetTournamentByIdAsync(id).Returns(tournament);
        _mapper.Map<TournamentDTO>(tournament).Returns(expectedTournament);

        // Act
        var result = await _sut.GetTournamentById(id);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task GetTournamentById_ShouldThrowsException_WhenThatTournamentDoesntExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetTournamentByIdAsync(id).ReturnsNull();

        // Act
        var result = async () => await _sut.GetTournamentById(id);

        // Assert
        await result.Should().ThrowAsync<TournamentNotFoundException>()
            .WithMessage($"Tournament with id {id} does not exist.");
    }

    [Fact]
    public async Task CreateNewTournament_ShouldCreateTournament_WhenCreateDetailsAreValid()
    {
        // Arrange
        var tournamentRequest = new DraftTournamentRequest();
        var tournamentEntity = new TournamentEntity();
        var expectedTournament = new TournamentDTO();
        _mapper.Map<TournamentEntity>(tournamentRequest).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = await _sut.CreateNewTournament(tournamentRequest);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task UpdateTournament_ShouldUpdateTournament_WhenUpdateDetailsAreValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentRequest = new UpdateTournamentRequest();
        var tournamentEntity = new TournamentEntity() {Id = id};
        var expectedTournament = new TournamentDTO();
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);
        _mapper.Map<TournamentEntity>(tournamentRequest).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = await _sut.UpdateTournament(id, tournamentRequest);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task UpdateTournament_ShouldThrowException_WhenTheTournamentToBeUpdatedDoesntExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentRequest = new UpdateTournamentRequest();
        _repository.GetTournamentByIdAsync(id).ReturnsNull();

        // Act
        var result = () => _sut.UpdateTournament(id, tournamentRequest);

        // Assert
        await result.Should().ThrowAsync<TournamentNotFoundException>()
            .WithMessage($"Tournament with id {id} does not exist.");
    }

    [Fact]
    public async Task CancelTournament_ShouldCancelTournament_WhenThatTournamentExistsAndIsPublished()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentEntity = new TournamentEntity() {Id = id, Status = TournamentStatus.Publish};
        var expectedTournament = new TournamentDTO() {Status = TournamentStatus.Cancel};
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = await _sut.CancelTournament(id);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task CancelTournament_ShouldThrowBadRequestException_WhenThatTournamentExistsAndWasNotPublished()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentEntity = new TournamentEntity() {Id = id, Status = TournamentStatus.Draft};
        var expectedTournament = new TournamentDTO() {Status = TournamentStatus.Cancel};
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = async () => await _sut.CancelTournament(id);

        // Assert
        await result.Should().ThrowAsync<BadHttpRequestException>()
            .WithMessage("Cannot cancel an unpublished tournament.");
    }

    [Fact]
    public async Task CancelTournament_ShouldThrowTournamentNotFoundException_WhenTournamentDoesntExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetTournamentByIdAsync(id).ReturnsNull();

        // Act
        var result = async () => await _sut.CancelTournament(id);

        // Assert
        await result.Should().ThrowAsync<TournamentNotFoundException>()
            .WithMessage($"Tournament with id {id} does not exist.");
    }

    [Fact]
    public async Task DeleteTournament_ShouldDeleteTournament_WhenThatTournamentExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentEntity = new TournamentEntity() {Id = id};
        var expectedTournament = new TournamentDTO();
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = await _sut.DeleteTournament(id);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task DeleteTournament_ShouldThrowTournamentNotFoundException_WhenTournamentDoesntExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetTournamentByIdAsync(id).ReturnsNull();

        // Act
        var result = async () => await _sut.DeleteTournament(id);

        // Assert
        await result.Should().ThrowAsync<TournamentNotFoundException>()
            .WithMessage($"Tournament with id {id} does not exist.");
    }

    [Fact]
    public async Task DeleteTournament_ShouldThrowTournamentInvalidException_WhenTournamentIsNotInDraftState()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentEntity = new TournamentEntity() {Id = id, Status = TournamentStatus.Publish};
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);

        // Act
        var result = async () => await _sut.DeleteTournament(id);

        // Assert
        await result.Should().ThrowAsync<TournamentInvalidException>()
            .WithMessage($"Cannot delete an {tournamentEntity.Status}ed tournament");
    }

    [Fact]
    public async Task ValidateTournamentToPublish_ShouldReturnTrue_WhenTournamentIsValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentEntity = new TournamentEntity()
        {
            Id = id,
            BeginTime = new(DateTime.Now + TimeSpan.FromDays(2)),
            EndTime = new(DateTime.Now + TimeSpan.FromDays(3))
        };
        var expectedTournament = new TournamentDTO();
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = await _sut.ValidateTournamentToPublish(id);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }

    [Fact]
    public async Task
        ValidateTournamentToPublish_ShouldThrowTournamentInvalidException_WhenBeginTimeIsInThePast()
    {    
        // Arrange
        var id = Guid.NewGuid();
        var beginTime = DateTimeOffset.Now - TimeSpan.FromDays(3);
        var endTime = DateTimeOffset.Now + TimeSpan.FromDays(3);
        var tournament = new TournamentEntity()
        {
            Id = id,
            BeginTime = beginTime,
            EndTime = endTime
        };
        _repository.GetTournamentByIdAsync(id).Returns(tournament);

        // Act
        var result = async () => await _sut.ValidateTournamentToPublish(id);

        // Assert
        await result.Should().ThrowAsync<TournamentInvalidException>()
            .WithMessage("Tournament's begin time cannot be in the past");
    }

    [Fact]
    public async Task
        ValidateTournamentToPublish_ShouldThrowTournamentInvalidException_WhenEndTimeIsInThePast()
    {    
        // Arrange
        var id = Guid.NewGuid();
        var beginTime = DateTimeOffset.Now - TimeSpan.FromDays(6);
        var endTime = DateTimeOffset.Now - TimeSpan.FromDays(3);
        var tournament = new TournamentEntity()
        {
            Id = id,
            BeginTime = beginTime,
            EndTime = endTime
        };
        _repository.GetTournamentByIdAsync(id).Returns(tournament);

        // Act
        var result = async () => await _sut.ValidateTournamentToPublish(id);

        // Assert
        await result.Should().ThrowAsync<TournamentInvalidException>()
            .WithMessage("Tournament's end time cannot be in the past");
    }
    
    [Fact]
    public async Task PublishTournament_ShouldPublishTournament_WhenTournamentIsValidToPublish()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tournamentEntity = new TournamentEntity()
        {
            Id = id, 
            BeginTime = new(DateTime.Now + TimeSpan.FromDays(2)),
            EndTime = new(DateTime.Now + TimeSpan.FromDays(3)),
            Status = TournamentStatus.Draft
        };
        var expectedTournament = new TournamentDTO() {Status = TournamentStatus.Publish};
        _repository.GetTournamentByIdAsync(id).Returns(tournamentEntity);
        _mapper.Map<TournamentDTO>(tournamentEntity).Returns(expectedTournament);

        // Act
        var result = await _sut.PublishTournament(id);

        // Assert
        result.Should().BeEquivalentTo(expectedTournament);
    }
    
    [Fact]
    public async Task PublishTournament_ShouldThrowsTournamentNotFoundException_WhenTournamentDoesntExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetTournamentByIdAsync(id).ReturnsNull();

        // Act
        var result = async () => await _sut.PublishTournament(id);

        // Assert
        await result.Should().ThrowAsync<TournamentNotFoundException>()
            .WithMessage($"Tournament with id {id} does not exist.");
    }
    // Arrange

    // Act

    // Assert
}