using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Exceptions;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;

namespace Enigma.DatingNet.Services.Impls;

public class MemberPersonalInfoService : IMemberPersonalInfoService
{
    private readonly IRepository<MemberPersonalInformation> _repository;
    private readonly IPersistence _persistence;

    private readonly IFileService _fileService;

    public MemberPersonalInfoService(IRepository<MemberPersonalInformation> repository, IPersistence persistence,
        IFileService fileService)
    {
        _repository = repository;
        _persistence = persistence;
        _fileService = fileService;
    }

    public async Task<MemberPersonalInfoResponse> Create(MemberPersonalInfoRequest request)
    {
        if (!Guid.TryParse(request.MemberId, out var memberId)) throw new NotFoundException("Member not found");
        return await _persistence.ExecuteTransactionAsync(async () =>
        {
            var profilePicturePath = await _fileService.SaveFile(request.ProfilePicture);
            var memberPersonalInformation = await _repository.SaveAsync(new MemberPersonalInformation
            {
                Name = request.Name,
                Bod = request.Bod,
                Gender = request.Gender,
                SelfDescription = request.SelfDescription,
                RecentPhotoPath = profilePicturePath,
                City = request.City,
                MemberId = memberId,
            });
            await _persistence.SaveChangesAsync();
            return ConvertMemberInfoToMemberInfoResponse(memberPersonalInformation);
        });
    }

    public async Task<MemberPersonalInfoResponse> GetByMemberId(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var mpi = await _repository.FindAsync(mpi => mpi.MemberId.Equals(guid));
        if (mpi is null) throw new NotFoundException("Member Personal Info not found");
        return ConvertMemberInfoToMemberInfoResponse(mpi);
    }

    private static MemberPersonalInfoResponse ConvertMemberInfoToMemberInfoResponse(
        MemberPersonalInformation memberPersonalInformation)
    {
        return new MemberPersonalInfoResponse
        {
            MemberPersonalInfoId = memberPersonalInformation.PersonalInformationId.ToString(),
            SelfDescription = memberPersonalInformation.SelfDescription,
            Name = memberPersonalInformation.Name,
            Bod = memberPersonalInformation.Bod,
            Gender = memberPersonalInformation.Gender, 
            RecentPhotoPath = memberPersonalInformation.RecentPhotoPath,
            City = memberPersonalInformation.City
        };
    }
}