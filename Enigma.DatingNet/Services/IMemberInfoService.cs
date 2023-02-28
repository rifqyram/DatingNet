using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMemberInfoService
{
    Task<MemberInfoResponse> GetByUsername(string username);
    Task VerificationUpdate(string memberId);
}