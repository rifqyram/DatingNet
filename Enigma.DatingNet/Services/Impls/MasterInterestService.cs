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

    public async Task<List<MasterInterestResponse>> Create(MasterInterestRequest interests)
    {
        var masterInterests = new List<MasterInterestResponse>();

        foreach (var interestReq in interests.Interests)
        {
            if (!Guid.TryParse(interests.MemberId, out var memberId)) throw new NotFoundException("Member not found");

            var currentMasterInterest =
                await _repository.FindAsync(interest => interest.Interest.ToLower().Equals(interestReq.ToLower()));

            if (currentMasterInterest is not null)
            {
                await _persistence.ExecuteTransactionAsync(async () =>
                {
                    var memberInterest =
                        await _memberInterestService.Create(memberId, currentMasterInterest.InterestId);
                    currentMasterInterest.MemberInterests.Add(memberInterest);
                    masterInterests.Add(new MasterInterestResponse
                    {
                        Interest = currentMasterInterest.Interest,
                        InterestId = interests.MemberId
                    });
                    await _persistence.SaveChangesAsync();
                    return true;
                });
                continue;
            }

            await _persistence.ExecuteTransactionAsync(async () =>
            {
                var masterInterest = await _repository.SaveAsync(new MasterInterest { Interest = interestReq });
                await _memberInterestService.Create(memberId, masterInterest.InterestId);
                await _persistence.SaveChangesAsync();
                masterInterests.Add(new MasterInterestResponse
                {
                    Interest = masterInterest.Interest,
                    InterestId = interests.MemberId
                });
                return true;
            });
        }

        return masterInterests;
    }

    public async Task<MasterInterestResponse> FindById(string id)
    {
        if (!Guid.TryParse(id, out var guid)) throw new NotFoundException("Interest not found");
        var masterInterest = await _repository.FindAsync(interest => interest.InterestId.Equals(guid));
        if (masterInterest is null) throw new NotFoundException("Interest not found");
        return new MasterInterestResponse
        {
            InterestId = masterInterest.InterestId.ToString(),
            Interest = masterInterest.Interest,
        };
    }

    public async Task<MasterInterestResponse> FindByInterest(string interest)
    {
        var masterInterest = await _repository.FindAsync(i => i.Interest.ToLower().Equals(interest.ToLower()));
        if (masterInterest is null) throw new NotFoundException("Interest not found");
        return new MasterInterestResponse
        {
            InterestId = masterInterest.InterestId.ToString(),
            Interest = masterInterest.Interest,
        };
    }

    public async Task<List<MasterInterestResponse>> GetAll()
    {
        var masterInterests = await _repository.FindAllAsync();
        return masterInterests.Select(interest => new MasterInterestResponse
        {
            InterestId = interest.InterestId.ToString(),
            Interest = interest.Interest,
        }).ToList();
    }

    public async Task<List<MemberInterestResponse>> Update(MasterInterestRequest requests)
    {
        if (requests.MemberId is null) throw new NotFoundException("Member not found");
        await _memberInterestService.DeleteAllByMemberId(requests.MemberId);
        var masterInterestResponses = await Create(requests);
        return masterInterestResponses.Select(response => new MemberInterestResponse
        {
            MemberId = requests.MemberId,
            InterestId = response.InterestId,
            Interest = response.Interest
        }).ToList();
    }
}