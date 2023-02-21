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
        var memberContactInformation = await _repository.SaveAsync(new MemberContactInformation
        {
            MobilePhoneNumber = request.MobilePhoneNumber,
            InstagramId = request.InstagramId,
            TwitterId = request.TwitterId,
            Email = request.Email,
            MemberId = memberId,
        });
        await _persistence.SaveChangesAsync();
        return new MemberContactInfoResponse
        {
            ContactInfoId = memberContactInformation.MemberId.ToString(),
            MobilePhoneNumber = memberContactInformation.MobilePhoneNumber,
            Email = memberContactInformation.Email,
            InstagramId = memberContactInformation.InstagramId,
            TwitterId = memberContactInformation.TwitterId,
        };
    }
}