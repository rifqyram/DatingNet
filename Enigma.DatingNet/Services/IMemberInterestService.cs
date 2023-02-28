using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMemberInterestService
{
    Task<MemberInterest> Create(Guid memberId, Guid interestId);
    Task<List<MemberInterestResponse>> FindListByMemberId(string memberId);
    Task<List<MemberInterest>> FindAllByMemberId(string memberId);
    Task DeleteAllByMemberId(string memberId);
}