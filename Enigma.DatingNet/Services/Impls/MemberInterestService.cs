using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class MemberInterestService : IMemberInterestService
{
    private readonly IRepository<MemberInterest> _repository;
    private readonly IPersistence _persistence;

    public MemberInterestService(IRepository<MemberInterest> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    public async Task<MemberInterest> Create(Guid memberId, Guid interestId)
    {
        var currentMemberInterest = await _repository.FindAsync(interest =>
            interest.InterestId.Equals(interestId) && interest.MemberId.Equals(memberId));
        if (currentMemberInterest is not null) return currentMemberInterest;
        var memberInterest = await _repository.SaveAsync(new MemberInterest
        {
            InterestId = interestId,
            MemberId = memberId,
        });
        await _persistence.SaveChangesAsync();
        return memberInterest;
    }

    public async Task<List<MemberInterest>> FindListByMemberId(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var memberInterests = await _repository.FindAllAsync(interest => interest.MemberId.Equals(guid));
        return memberInterests.ToList();
    }
}