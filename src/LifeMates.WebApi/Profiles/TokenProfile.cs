using AutoMapper;
using LifeMates.Core.Commands.Token;
using LifeMates.WebApi.Controllers.v0.Models.Token.Invoke;
using LifeMates.WebApi.Controllers.v0.Models.Token.Refresh;

namespace LifeMates.WebApi.Profiles;

public class TokenProfile : Profile
{
    public TokenProfile()
    {
        RefreshMap();
        
        RevokeMap();
    }

    private void RevokeMap()
    {
        CreateMap<RevokeTokenRequest, RevokeTokenCommand>();
    }

    private void RefreshMap()
    {
        CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
        CreateMap<RefreshTokenCommandResponse, RefreshTokenResponse>();
    }
}