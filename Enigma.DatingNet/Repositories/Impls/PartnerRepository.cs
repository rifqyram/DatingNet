using System.Diagnostics.CodeAnalysis;
using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Enigma.DatingNet.Repositories.Impls;

public class PartnerRepository : IPartnerRepository
{
    private readonly AppDbContext _context;

    public PartnerRepository(AppDbContext context) => _context = context;

    [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
    public async Task<List<MemberPersonalInformation>> FindPartners(
        Guid memberId,
        string byGender,
        int byStartAge,
        int byEndAge,
        Guid[] interestId,
        int page, int size)
    {
        // var query = await (
        //         from mpi in _context.MemberPersonalInformation
        //         join mp in _context.MemberPreferences on mpi.MemberId equals mp.MemberId
        //         where !mpi.MemberId.Equals(memberId)
        //               && mpi.Gender.ToLower().Equals(byGender.ToLower())
        //               && DateTime.Now.Year - mpi.Bod.Year >= byStartAge
        //               && DateTime.Now.Year - mpi.Bod.Year <= byEndAge
        //               && !_context.MemberPartners
        //                   .Where(mp => mp.MemberId == "a1ebdeec-75fe-415c-8d0b-ef239b101ca8")
        //                   .Select(mp => mp.PartnerId)
        //                   .Contains(mpi.MemberId)
        //         select mpi)
        //     .Skip((page - 1) * size)
        //     .Take(size).ToListAsync();

        var info = await _context.MemberPersonalInformation
            .Join(_context.MemberPreferences,
                mpi => mpi.MemberId,
                mp => mp.MemberId,
                (mpi, mp) => new { mpi, mp })
            .Where(arg => !arg.mpi.MemberId.Equals(memberId)
                          && arg.mpi.Gender.ToLower().Equals(byGender.ToLower())
                          && DateTime.Now.Year - arg.mpi.Bod.Year >= byStartAge
                          && DateTime.Now.Year - arg.mpi.Bod.Year <= byEndAge
                          && !_context.MemberPartners
                              .Where(mp => mp.MemberId.Equals(memberId))
                              .Select(mp => mp.PartnerId)
                              .Contains(arg.mpi.MemberId)
            )
            .Select(arg => arg.mpi)
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

    public async Task<MemberPartner?> FindByMemberIdAndPartnerId(Guid memberId, Guid partnerId)
    {
        return await _context.MemberPartners.FirstOrDefaultAsync(partner =>
            partner.MemberId.Equals(memberId) && partner.PartnerId.Equals(partnerId));
    }

    public async Task<List<MemberPersonalInformation>> ListPartner(Guid memberId)
    {
        var memberPersonalInfo = await _context.MemberPersonalInformation
            .Join(
                _context.MemberPartners.Where(partner => partner.MemberId.Equals(memberId)),
                information => information.MemberId,
                partner => partner.PartnerId,
                (information, partner) => information
            ).ToListAsync();
        return memberPersonalInfo;
    }

    public async Task<int> CountAsync(Guid memberId,
        string byGender,
        int byStartAge,
        int byEndAge)
    {
        return await _context.MemberPersonalInformation
            .Join(_context.MemberPreferences,
                mpi => mpi.MemberId,
                mp => mp.MemberId,
                (mpi, mp) => new { mpi, mp })
            .Where(arg => !arg.mpi.MemberId.Equals(memberId)
                          && arg.mpi.Gender.ToLower().Equals(byGender.ToLower())
                          && DateTime.Now.Year - arg.mpi.Bod.Year >= byStartAge
                          && DateTime.Now.Year - arg.mpi.Bod.Year <= byEndAge
                          && !_context.MemberPartners
                              .Where(mp => mp.MemberId.Equals(memberId))
                              .Select(mp => mp.PartnerId)
                              .Contains(arg.mpi.MemberId)
            )
            .Select(arg => arg.mpi)
            .CountAsync();
    }
}