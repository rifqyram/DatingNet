using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;

namespace Enigma.DatingNet.Services;

public interface IMasterInterestService
{
    Task<List<MasterInterestResponse>> Create(MasterInterestRequest interests);
    Task<MasterInterestResponse> FindById(string id);
    Task<MasterInterestResponse> FindByInterest(string interest);
    Task<List<MasterInterestResponse>> GetAll();
    Task<List<MemberInterestResponse>> Update(MasterInterestRequest request);
}