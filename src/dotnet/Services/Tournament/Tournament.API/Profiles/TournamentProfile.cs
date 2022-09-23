﻿using AutoMapper;
using Tournament.API.Controllers.Payloads.Requests;
using Tournament.API.Models.DTOs;
using Tournament.API.Models.Entities.Tournament;
using Tournament.API.Models.Statuses;

namespace Tournament.API.Profiles;

public class TournamentProfile : Profile
{
    public TournamentProfile()
    {
        CreateMap<TournamentEntity, TournamentDTO>();
        CreateMap<TournamentDTO, TournamentEntity>();
        CreateMap<DraftTournamentRequest, TournamentDTO>()
            .BeforeMap((src, dest) => dest.Status = TournamentStatus.Draft)
            .BeforeMap((src, dest) => dest.CreateTime = DateTimeOffset.Now);
    }
}