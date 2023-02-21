using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Enigma.DatingNet.Repositories.Impls;

public class PartnerRepository : IPartnerRepository
{
    private readonly AppDbContext _context;

    public PartnerRepository(AppDbContext context) => _context = context;

    public async Task<List<MemberPersonalInformation>> FindPartners(
        Guid memberId,
        string byGender,
        int byStartAge,
        int byEndAge,
        Guid[] interestId,
        int page, int size)
    {
        var info = await _context.MemberPersonalInformation
            .Join(_context.MemberPreferences,
                mpi => mpi.MemberId,
                mp => mp.MemberId,
                (mpi, mp) => new { mpi, mp })
            .Join(_context.MemberInterest,
                arg => arg.mpi.MemberId,
                mi => mi.MemberId,
                (arg, mi) => new { arg, mi })
            .Where(arg => !arg.arg.mpi.MemberId.Equals(memberId)
                          && arg.arg.mpi.Gender.Equals(byGender)
                          && DateTime.Now.Year - arg.arg.mpi.Bod.Year >= byStartAge
                          && DateTime.Now.Year - arg.arg.mpi.Bod.Year <= byEndAge
                          && interestId.Any(guid => arg.mi.InterestId.Equals(guid))
            )
            .OrderBy(arg => arg.arg.mpi.MemberId)
            .Select(arg => arg.arg.mpi)
            .Skip((page - 1) * size)
            .Take(size).ToListAsync();

        return info;
    }

    public async Task CreatePartner(Guid memberId, Guid partnerId)
    {
        await _context.MemberPartners.AddAsync(new MemberPartner
        {
            MemberId = memberId,
            PartnerId = partnerId,
        });
    }

    public async Task<List<MemberPersonalInformation>> ListPartner(Guid memberId)
    {
        var memberPersonalInfo = await _context.MemberPersonalInformation
            .Join(
                _context.MemberPartners.Where(partner => partner.MemberId.Equals(memberId)),
                information => information.MemberId,
                partner => partner.MemberId,
                (information, partner) => information
            ).ToListAsync();
        return memberPersonalInfo;
    }
}