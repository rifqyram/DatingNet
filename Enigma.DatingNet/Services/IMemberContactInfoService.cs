using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMemberContactInfoService
{
    Task<MemberContactInfoResponse> Create(MemberContactInfoRequest request);
}