using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class MemberContactInfoService : IMemberContactInfoService
{
    private readonly IRepository<MemberContactInformation> _repository;
    private readonly IPersistence _persistence;

    public MemberContactInfoService(IRepository<MemberContactInformation> repository, IPersistence persistence)
    {
        _repository = repository;
        _persistence = persistence;
    }

    public async Task<MemberContactInfoResponse> Create(MemberContactInfoRequest request)
    {
        if (!Guid.TryParse(request.MemberId, out var memberId)) throw new NotFoundException("Member not found");

        var currentMci = await _repository.FindAsync(information => information.MemberId.Equals(memberId));

        if (currentMci is not null) return ConvertMemberContactInfoToMemberContactInfoResponse(currentMci);

        var memberContactInformation = await _repository.SaveAsync(new MemberContactInformation
        {
            MobilePhoneNumber = request.MobilePhoneNumber,
            InstagramId = request.InstagramId,
            TwitterId = request.TwitterId,
            Email = request.Email,
            MemberId = memberId,
        });
        await _persistence.SaveChangesAsync();
        return ConvertMemberContactInfoToMemberContactInfoResponse(memberContactInformation);
    }

    public async Task<MemberContactInfoResponse> GetByMemberId(string memberId)
    {
        var mci = await FindMemberIdByOrThrowNotFound(memberId);
        return ConvertMemberContactInfoToMemberContactInfoResponse(mci);
    }

    public async Task<MemberContactInfoResponse> Update(MemberContactInfoUpdateRequest request)
    {
        var mci = await FindIdByOrThrowNotFound(request.MemberContactId);

        if (request.MemberId is not null && !request.MemberId.Equals(mci.MemberId.ToString()))
            throw new UnauthorizedException("Unauthorized");

        mci.InstagramId = request.InstagramId;
        mci.TwitterId = request.TwitterId;

        var mciUpdate = _repository.Update(mci);
        await _persistence.SaveChangesAsync();
        return ConvertMemberContactInfoToMemberContactInfoResponse(mciUpdate);
    }

    private async Task<MemberContactInformation> FindMemberIdByOrThrowNotFound(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var mci = await _repository.FindAsync(information => information.MemberId.Equals(guid));
        if (mci is null) throw new NotFoundException("Contact info not found");
        return mci;
    }

    private async Task<MemberContactInformation> FindIdByOrThrowNotFound(string contactInfoId)
    {
        if (!Guid.TryParse(contactInfoId, out var guid)) throw new NotFoundException("Contact Info not found");
        var mci = await _repository.FindAsync(information => information.MemberContactId.Equals(guid));
        if (mci is null) throw new NotFoundException("Contact info not found");
        return mci;
    }

    private static MemberContactInfoResponse ConvertMemberContactInfoToMemberContactInfoResponse(
        MemberContactInformation memberContactInformation)
    {
        return new MemberContactInfoResponse
        {
            MemberContactId = memberContactInformation.MemberContactId.ToString(),
            MobilePhoneNumber = memberContactInformation.MobilePhoneNumber,
            Email = memberContactInformation.Email,
            InstagramId = memberContactInformation.InstagramId,
            TwitterId = memberContactInformation.TwitterId,
            MemberId = memberContactInformation.MemberId.ToString()
        };
    }
}