using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class PartnerService : IPartnerService
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPersistence _persistence;
    private readonly IMemberPreferencesService _preferencesService;
    private readonly IMemberInterestService _memberInterestService;

    public PartnerService(IPartnerRepository partnerRepository, IPersistence persistence,
        IMemberPreferencesService preferencesService,
        IMemberInterestService memberInterestService)
    {
        _partnerRepository = partnerRepository;
        _persistence = persistence;
        _preferencesService = preferencesService;
        _memberInterestService = memberInterestService;
    }

    public async Task<PageResponse<MemberPersonalInfoResponse>> FindPartners(string memberId, int page, int size)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");

        var memberPreference = await _preferencesService.FindByMemberId(memberId);
        var findListMemberInterestByMemberId = await _memberInterestService.FindListByMemberId(memberId);
        var memberPersonalInfos = await _partnerRepository.FindPartners(
            guid,
            memberPreference.LookingForGender,
            memberPreference.LookingForStartAge,
            memberPreference.LookingForEndAge,
            findListMemberInterestByMemberId.Select(interest => Guid.Parse(interest.InterestId)).ToArray(), page, size);

        var memberPersonalInfoResponse = memberPersonalInfos.Select(information => new MemberPersonalInfoResponse
        {
            PersonalInformationId = information.PersonalInformationId.ToString(),
            SelfDescription = information.SelfDescription,
            Bod = information.Bod,
            Gender = information.Gender,
            Name = information.Name,
            ProfilePicture = information.RecentPhotoPath,
            City = information.City,
            MemberId = information.MemberId.ToString()
        }).ToList();

        var totalPages = (int)Math.Ceiling(await _partnerRepository.CountAsync(
            guid,
            memberPreference.LookingForGender,
            memberPreference.LookingForStartAge,
            memberPreference.LookingForEndAge) / (decimal)size);

        return new PageResponse<MemberPersonalInfoResponse>
        {
            Content = memberPersonalInfoResponse,
            TotalPages = totalPages,
            TotalElement = memberPersonalInfoResponse.Count
        };
    }

    public async Task CreateMemberPartner(PartnerRequest request)
    {
        if (!Guid.TryParse(request.MemberId, out var memberGuid)) throw new NotFoundException("Member not found");
        if (!Guid.TryParse(request.PartnerId, out var partnerGuid)) throw new NotFoundException("Partner not found");

        var memberPartner = await _partnerRepository.FindByMemberIdAndPartnerId(memberGuid, partnerGuid);
        if (memberPartner is not null) throw new DuplicateDataException("you already match this partner");

        await _partnerRepository.CreatePartner(memberGuid, partnerGuid);
        await _persistence.SaveChangesAsync();
    }

    public async Task<List<MemberPersonalInfoResponse>> ListPartner(string memberId)
    {
        if (!Guid.TryParse(memberId, out var memberGuid)) throw new NotFoundException("Member not found");
        var partners = await _partnerRepository.ListPartner(memberGuid);
        return partners.Select(information => new MemberPersonalInfoResponse
        {
            PersonalInformationId = information.PersonalInformationId.ToString(),
            SelfDescription = information.SelfDescription,
            Bod = information.Bod,
            Gender = information.Gender,
            Name = information.Name,
            ProfilePicture = information.RecentPhotoPath,
            City = information.City
        }).ToList();
    }
}