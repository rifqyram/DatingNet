using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IPartnerService
{
    Task<PageResponse<MemberPersonalInfoResponse>> FindPartners(string memberId, int page, int size);
    Task CreateMemberPartner(PartnerRequest request);
    Task<List<MemberPersonalInfoResponse>> ListPartner(string memberId);
}