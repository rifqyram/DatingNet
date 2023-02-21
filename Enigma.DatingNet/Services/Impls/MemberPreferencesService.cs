using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class MemberPreferencesService : IMemberPreferencesService
{
    private readonly IRepository<MemberPreferences> _repository;
    private readonly IPersistence _persistence;

    public MemberPreferencesService(IRepository<MemberPreferences> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    public async Task<MemberPreferencesResponse> Create(MemberPreferencesRequest request)
    {
        if (!Guid.TryParse(request.MemberId, out var memberId)) throw new NotFoundException("Member not found");
        var memberPreferences = await _repository.SaveAsync(new MemberPreferences
        {
            LookingForGender = request.LookingForGender,
            LookingForDomicile = request.LookingForDomicile,
            LookingForStartAge = request.LookingForStartAge,
            LookingForEndAge = request.LookingForEndAge,
            MemberId = memberId,
        });
        await _persistence.SaveChangesAsync();
        return new MemberPreferencesResponse
        {
            MemberPreferenceId = memberPreferences.PreferenceId.ToString(),
            LookingForGender = memberPreferences.LookingForGender,
            LookingForDomicile = memberPreferences.LookingForDomicile,
            LookingForStartAge = memberPreferences.LookingForStartAge,
            LookingForEndAge = memberPreferences.LookingForEndAge,
            MemberId = memberPreferences.MemberId.ToString()
        };
    }

    public async Task<MemberPreferencesResponse> FindByMemberId(string memberId)
    {
        if (!Guid.TryParse(memberId, out var id)) throw new NotFoundException("Member not found");
        var memberPreferences = await _repository.FindAsync(preferences => preferences.MemberId.Equals(id));
        if (memberPreferences is null) throw new NotFoundException("Preferences not found");
        return new MemberPreferencesResponse
        {
            MemberPreferenceId = memberPreferences.PreferenceId.ToString(),
            LookingForGender = memberPreferences.LookingForGender,
            LookingForDomicile = memberPreferences.LookingForDomicile,
            LookingForStartAge = memberPreferences.LookingForStartAge,
            LookingForEndAge = memberPreferences.LookingForEndAge,
            MemberId = memberPreferences.MemberId.ToString()
        };
    }
}