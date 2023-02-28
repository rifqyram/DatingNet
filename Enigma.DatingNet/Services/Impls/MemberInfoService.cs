using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class MemberInfoService : IMemberInfoService
{
    private readonly IRepository<MemberUserAccess> _repository;
    private readonly IPersistence _persistence;

    public MemberInfoService(IRepository<MemberUserAccess> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    public async Task<MemberInfoResponse> GetByUsername(string username)
    {
        var member = await _repository.FindAsync(access => access.Username.ToLower().Equals(username.ToLower()));
        if (member is null) throw new NotFoundException("Member not found");
        return new MemberInfoResponse
        {
            MemberId = member.MemberId.ToString(),
            Username = member.Username,
            JoinDate = member.JoinDate,
            VerificationStatus = member.VerificationStatus
        };
    }

    public async Task VerificationUpdate(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var member = await _repository.FindAsync(access => access.MemberId.Equals(guid));
        if (member is null) throw new NotFoundException("Member not found");
        member.VerificationStatus = "Y";
        await _persistence.SaveChangesAsync();
    }
}