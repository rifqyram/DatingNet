using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Services;

public interface IMemberInterestService
{
    Task<MemberInterest> Create(Guid memberId, Guid interestId);
    Task<List<MemberInterest>> FindListByMemberId(string memberId);
}