using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMemberPreferencesService
{
    Task<MemberPreferencesResponse> Create(MemberPreferencesRequest request);
    Task<MemberPreferencesResponse> FindByMemberId(string memberId);
    Task<MemberPreferencesResponse> Update(MemberPreferencesUpdateRequest request);
}