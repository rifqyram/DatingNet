using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMemberPersonalInfoService
{
    Task<MemberPersonalInfoResponse> Create(MemberPersonalInfoRequest request);
    Task<MemberPersonalInfoResponse> GetByMemberId(string memberId);
    Task<MemberPersonalInfoResponse> Update(MemberPersonalInfoUpdateRequest request);
}