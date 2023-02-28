using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMemberContactInfoService
{
    Task<MemberContactInfoResponse> Create(MemberContactInfoRequest request);
    Task<MemberContactInfoResponse> GetByMemberId(string memberId);
    Task<MemberContactInfoResponse> Update(MemberContactInfoUpdateRequest request);
}