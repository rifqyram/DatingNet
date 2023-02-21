using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class MasterInterestService : IMasterInterestService
{
    private readonly IRepository<MasterInterest> _repository;
    private readonly IPersistence _persistence;

    private readonly IMemberInterestService _memberInterestService;

    public MasterInterestService(IRepository<MasterInterest> repository, IPersistence persistence,
        IMemberInterestService memberInterestService)
    {
        _repository = repository;
        _persistence = persistence;
        _memberInterestService = memberInterestService;
    }

    public async Task<List<MasterInterestResponse>> Create(List<MasterInterestRequest> interests)
    {
        var memberInterests = new List<MemberInterest>();
        var masterInterests = new List<MasterInterestResponse>();

        foreach (var mir in interests)
        {
            if (!Guid.TryParse(mir.MemberId, out var memberId)) throw new NotFoundException("Member not found");

            var currentMasterInterest = await _repository.FindAsync(interest => interest.Interest.ToLower().Equals(mir.Interest.ToLower()));
            if (currentMasterInterest is not null)
            {
                var memberInterest = await _memberInterestService.Create(memberId, currentMasterInterest.InterestId);
                memberInterests.Add(memberInterest);
                currentMasterInterest.MemberInterests = memberInterests;
                masterInterests.Add(new MasterInterestResponse
                {
                    Interest = currentMasterInterest.Interest,
                    MemberId = mir.MemberId
                });
                await _persistence.SaveChangesAsync();
                return masterInterests;
            }
            
            var masterInterest = await _repository.SaveAsync(new MasterInterest
            {
                Interest = mir.Interest
            });
            await _memberInterestService.Create(memberId, masterInterest.InterestId);
            await _persistence.SaveChangesAsync();
            masterInterests.Add(new MasterInterestResponse
            {
                Interest = masterInterest.Interest,
                MemberId = mir.MemberId
            });
        }

        return masterInterests;
    }

    public async Task<MasterInterestResponse> FindById(string id)
    {
        if (!Guid.TryParse(id, out var guid)) throw new NotFoundException("Interest not found");
        var masterInterest = await _repository.FindAsync(interest => interest.InterestId.Equals(guid), new []{"MemberInterests"});
        if (masterInterest is null) throw new NotFoundException("Interest not found");
        return new MasterInterestResponse
        {
            Interest = masterInterest.Interest,
            MemberId = masterInterest.MemberInterests.Select(interest => new string(interest.MemberId.ToString())).First()
        };
    }

    public async Task<MasterInterestResponse> FindByInterest(string interest)
    {
        var masterInterest = await _repository.FindAsync(i => i.Interest.Equals(interest));
        if (masterInterest is null) throw new NotFoundException("Interest not found");
        return new MasterInterestResponse
        {
            Interest = masterInterest.Interest,
            MemberId = masterInterest.MemberInterests.Select(memberInterest => new string(memberInterest.MemberId.ToString())).First()
        };;
    }
}