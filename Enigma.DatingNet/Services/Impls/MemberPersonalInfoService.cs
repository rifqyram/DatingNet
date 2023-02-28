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
            var currentMpi = await _repository.FindAsync(information => information.MemberId.Equals(memberId));
            if (currentMpi is not null) return ConvertMemberInfoToMemberInfoResponse(currentMpi);
            
            var profilePicturePath = await _fileService.SaveFile(request.ProfilePicture);
            var memberPersonalInformation = await _repository.SaveAsync(new MemberPersonalInformation
            {
                Name = request.Name,
                Bod = DateOnly.FromDateTime(DateTime.Parse(request.Bod)),
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
        var mpi = await FindByMemberIdOrThrowNotFound(memberId);
        return ConvertMemberInfoToMemberInfoResponse(mpi);
    }

    public async Task<MemberPersonalInfoResponse> Update(MemberPersonalInfoUpdateRequest request)
    {
        var mpi = await FindByIdOrThrowNotFound(request.PersonalInformationId);

        var mpiUpdate = await _persistence.ExecuteTransactionAsync(async () =>
        {
            if (request.MemberId != mpi.MemberId.ToString()) throw new UnauthorizedException("Unauthorized");

            string? newFilePath = null;
            
            if (request.ProfilePicture != null)
            {
                newFilePath = await _fileService.SaveFile(request.ProfilePicture);
                _fileService.RemoveFile(mpi.RecentPhotoPath);
            }

            mpi.Name = request.Name;
            mpi.Bod = DateOnly.FromDateTime(DateTime.Parse(request.Bod));
            mpi.RecentPhotoPath = newFilePath ?? mpi.RecentPhotoPath;
            mpi.SelfDescription = request.SelfDescription;

            var mpiUpdate = _repository.Update(mpi);
            await _persistence.SaveChangesAsync();
            return mpiUpdate;
        });

        return ConvertMemberInfoToMemberInfoResponse(mpiUpdate);
    }

    private async Task<MemberPersonalInformation> FindByIdOrThrowNotFound(string id)
    {
        if (!Guid.TryParse(id, out var guid)) throw new NotFoundException("Personal Info not found");
        var mpi = await _repository.FindAsync(mpi => mpi.PersonalInformationId.Equals(guid));
        if (mpi is null) throw new NotFoundException("Member Personal Info not found");
        return mpi;
    }

    private async Task<MemberPersonalInformation> FindByMemberIdOrThrowNotFound(string memberId)
    {
        if (!Guid.TryParse(memberId, out var guid)) throw new NotFoundException("Member not found");
        var mpi = await _repository.FindAsync(mpi => mpi.MemberId.Equals(guid));
        if (mpi is null) throw new NotFoundException("Member Personal Info not found");
        return mpi;
    }

    private static MemberPersonalInfoResponse ConvertMemberInfoToMemberInfoResponse(
        MemberPersonalInformation memberPersonalInformation)
    {
        return new MemberPersonalInfoResponse
        {
            PersonalInformationId = memberPersonalInformation.PersonalInformationId.ToString(),
            SelfDescription = memberPersonalInformation.SelfDescription,
            Name = memberPersonalInformation.Name,
            Bod = memberPersonalInformation.Bod,
            Gender = memberPersonalInformation.Gender,
            ProfilePicture = memberPersonalInformation.RecentPhotoPath,
            City = memberPersonalInformation.City
        };
    }
}