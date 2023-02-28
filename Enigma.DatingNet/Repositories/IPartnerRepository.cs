using Enigma.DatingNet.Entities;

namespace Enigma.DatingNet.Repositories;

public interface IPartnerRepository
{
    Task<List<MemberPersonalInformation>> FindPartners(Guid memberId, string byGender, int byStartAge, int byEndAge,
        Guid[] interestId, int page = 1, int size = 1);

    Task CreatePartner(Guid memberId, Guid partnerId);
    Task<MemberPartner?> FindByMemberIdAndPartnerId(Guid memberId, Guid partnerId);

    Task<List<MemberPersonalInformation>> ListPartner(Guid memberId);

    Task<int> CountAsync(Guid memberId,
        string byGender,
        int byStartAge,
        int byEndAge);
}