using System.Collections;
using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Responses;
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

    public async Task<List<MemberInterestResponse>> FindListByMemberId(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var memberInterests = await _repository.FindAllAsync(interest => interest.MemberId.Equals(guid), new []{"Interest"});

        var interests = memberInterests.ToList();
        if (!interests.Any()) return new List<MemberInterestResponse>();

        return interests.Select(interest => new MemberInterestResponse
        {
            MemberId = interest.MemberId.ToString(),
            InterestId = interest.MemberId.ToString(),
            Interest = interest.Interest!.Interest
        }).ToList();
    }

    public async Task<List<MemberInterest>> FindAllByMemberId(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var memberInterests = await _repository.FindAllAsync(interest => interest.MemberId.Equals(guid), new [] {"Interest"});
        var interests = memberInterests.ToList();
        return !interests.Any() ? new List<MemberInterest>() : interests.ToList();
    }

    public async Task DeleteAllByMemberId(string memberId)
    {
        var memberInterests = await FindAllByMemberId(memberId);
        _repository.DeleteAll(memberInterests);
        await _persistence.SaveChangesAsync();
    }
}