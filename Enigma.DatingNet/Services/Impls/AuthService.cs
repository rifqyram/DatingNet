using System.Net;
using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;
using Enigma.DatingNet.Securities;

namespace Enigma.DatingNet.Services.Impls;

public class AuthService : IAuthService
{
    private readonly IRepository<MemberUserAccess> _repository;
    private readonly IPersistence _persistence;
    private readonly AuthUtil _authUtil;
    private readonly IJwtUtils _jwtUtils;

    public AuthService(IRepository<MemberUserAccess> repository, IPersistence persistence, AuthUtil authUtil, IJwtUtils jwtUtils)
    {
        _repository = repository;
        _persistence = persistence;
        _authUtil = authUtil;
        _jwtUtils = jwtUtils;
    }

    public async Task<RegisterResponse> Register(AuthRequest request)
    {
        var mua = await LoadByUsername(request.Username);
        if (mua is not null) throw new DuplicateDataException("Username already exist");
        
        var memberUser = new MemberUserAccess
        {
            Username = request.Username.ToLower(),
            Password = _authUtil.HashPassword(request.Password),
            JoinDate = DateTime.Now,
            VerificationStatus = "Y",
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
        var memberUser = await _repository.FindAsync(access => access.Username.ToLower().Equals(request.Username.ToLower()));
        if (memberUser is null) throw new UnauthorizedException("Unauthorized");
        var verify = _authUtil.Verify(request.Password, memberUser.Password);
        if (!verify) throw new UnauthorizedException("Unauthorized");
        
        var token = _jwtUtils.GenerateToken(memberUser);

        return new LoginResponse
        {
            Username = memberUser.Username,
            Token = token
        };
    }

    private async Task<MemberUserAccess> LoadByUsername(string username)
    {
        var mua = await _repository.FindAsync(access => access.Username.ToLower().Equals(username.ToLower()));
        return mua;
    }
}