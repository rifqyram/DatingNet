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
        var mp = await _repository.FindAsync(preferences => preferences.MemberId.Equals(memberId));

        if (mp is not null) return ConvertMemberPreferencesToMemberPreferencesResponse(mp);

        var memberPreferences = await _repository.SaveAsync(new MemberPreferences
        {
            LookingForGender = request.LookingForGender,
            LookingForDomicile = request.LookingForDomicile,
            LookingForStartAge = request.LookingForStartAge,
            LookingForEndAge = request.LookingForEndAge,
            MemberId = memberId,
        });
        await _persistence.SaveChangesAsync();
        return ConvertMemberPreferencesToMemberPreferencesResponse(memberPreferences);
    }

    public async Task<MemberPreferencesResponse> FindByMemberId(string memberId)
    {
        var memberPreferences = await FindByMemberIdOrThrowNotFound(memberId);
        return ConvertMemberPreferencesToMemberPreferencesResponse(memberPreferences);
    }

    public async Task<MemberPreferencesResponse> Update(MemberPreferencesUpdateRequest request)
    {
        var mp = await FindByIdOrThrowNotFound(request.PreferenceId);

        mp.LookingForGender = request.LookingForGender;
        mp.LookingForStartAge = request.LookingForStartAge;
        mp.LookingForEndAge = request.LookingForEndAge;

        var updateMp = _repository.Update(mp);
        await _persistence.SaveChangesAsync();

        return ConvertMemberPreferencesToMemberPreferencesResponse(updateMp);
    }

    private async Task<MemberPreferences> FindByIdOrThrowNotFound(string preferenceId)
    {
        if (!Guid.TryParse(preferenceId, out var id)) throw new NotFoundException("Member not found");
        var memberPreferences = await _repository.FindAsync(preferences => preferences.PreferenceId.Equals(id));
        if (memberPreferences is null) throw new NotFoundException("Preferences not found");
        return memberPreferences;

    }

    private async Task<MemberPreferences> FindByMemberIdOrThrowNotFound(string memberId)
    {
        if (!Guid.TryParse(memberId, out var id)) throw new NotFoundException("Member not found");
        var memberPreferences = await _repository.FindAsync(preferences => preferences.MemberId.Equals(id));
        if (memberPreferences is null) throw new NotFoundException("Preferences not found");
        return memberPreferences;
    }

    private static MemberPreferencesResponse ConvertMemberPreferencesToMemberPreferencesResponse(
        MemberPreferences memberPreferences)
    {
        return new MemberPreferencesResponse
        {
            PreferenceId = memberPreferences.PreferenceId.ToString(),
            LookingForGender = memberPreferences.LookingForGender,
            LookingForDomicile = memberPreferences.LookingForDomicile,
            LookingForStartAge = memberPreferences.LookingForStartAge,
            LookingForEndAge = memberPreferences.LookingForEndAge,
            MemberId = memberPreferences.MemberId.ToString()
        };
    }
}