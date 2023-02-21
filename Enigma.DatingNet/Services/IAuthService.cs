using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IAuthService
{
    Task<RegisterResponse> Register(AuthRequest request);
    Task<LoginResponse> Login(AuthRequest request);
}