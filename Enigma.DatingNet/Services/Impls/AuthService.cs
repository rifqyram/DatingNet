using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;
using Enigma.DatingNet.Utils;

namespace Enigma.DatingNet.Services.Impls;

public class AuthService : IAuthService
{
    private readonly IRepository<MemberUserAccess> _repository;
    private readonly IPersistence _persistence;
    private readonly AuthUtil _authUtil;

    public AuthService(IRepository<MemberUserAccess> repository, IPersistence persistence, AuthUtil authUtil)
    {
        _repository = repository;
        _persistence = persistence;
        _authUtil = authUtil;
    }

    public async Task<RegisterResponse> Register(AuthRequest request)
    {
        var memberUser = new MemberUserAccess
        {
            Username = request.Username,
            Password = _authUtil.HashPassword(request.Password),
            JoinDate = DateTime.Now,
            VerificationStatus = "N",
        };
        var memberUserSave = await _repository.SaveAsync(memberUser);
        await _persistence.SaveChangesAsync();
        return new RegisterResponse
        {
            Username = memberUserSave.Username
        };
    }

    public async Task<LoginResponse> Login(AuthRequest request)
    {
        var memberUser = await _repository.FindAsync(access => access.Username.Equals(request.Username));
        if (memberUser is null) throw new UnauthorizedException("Unauthorized");
        var verify = _authUtil.Verify(request.Password, memberUser.Password);
        if (!verify) throw new UnauthorizedException("Unauthorized");
        return new LoginResponse
        {
            Username = memberUser.Username,
            Token = Guid.NewGuid().ToString()
        };
    }
}