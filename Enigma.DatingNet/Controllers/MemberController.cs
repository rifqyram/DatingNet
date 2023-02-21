using System.Net;
using Enigma.DatingNet.Entities;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;
using MasterInterestRequest = Enigma.DatingNet.Models.Requests.MasterInterestRequest;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/members")]
public class MemberController : BaseController
{
    private readonly IMemberPersonalInfoService _memberPersonalInfoService;
    private readonly IMemberContactInfoService _memberContactInfoService;
    private readonly IMemberPreferencesService _preferencesService;
    private readonly IMasterInterestService _interestService;

    public MemberController(IMemberPersonalInfoService memberPersonalInfoService,
        IMemberContactInfoService memberContactInfoService, IMemberPreferencesService preferencesService, IMasterInterestService interestService)
    {
        _memberPersonalInfoService = memberPersonalInfoService;
        _memberContactInfoService = memberContactInfoService;
        _preferencesService = preferencesService;
        _interestService = interestService;
    }

    [HttpPost("personal-info")]
    public async Task<IActionResult> CreatePersonalInfo([FromForm] MemberPersonalInfoRequest request)
    {
        var memberPersonalInfoResponse = await _memberPersonalInfoService.Create(request);
        var response = new CommonResponse<MemberPersonalInfoResponse>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create personal info",
            Data = memberPersonalInfoResponse
        };
        return Created("/api/v1/members/personal-info", response);
    }

    [HttpPost("contact-info")]
    public async Task<IActionResult> CreateContactInfo([FromBody] MemberContactInfoRequest request)
    {
        var memberContactInfoResponse = await _memberContactInfoService.Create(request);
        var response = new CommonResponse<MemberContactInfoResponse>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create contact info",
            Data = memberContactInfoResponse
        };
        return Created("/api/v1/members/contact-info", response);
    }


    [HttpPost("preferences")]
    public async Task<IActionResult> CreatePreferences([FromBody] MemberPreferencesRequest request)
    {
        var memberPreferencesResponse = await _preferencesService.Create(request);
        var response = new CommonResponse<MemberPreferencesResponse>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create preferences",
            Data = memberPreferencesResponse
        };
        return Created("/api/v1/members/preferences", response);
    }

    [HttpPost("interests")]
    public async Task<IActionResult> CreateInterests([FromBody] List<MasterInterestRequest> requests)
    {
        var masterInterestsResponses = await _interestService.Create(requests);
        var response = new CommonResponse<List<MasterInterestResponse>>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create interests",
            Data = masterInterestsResponses
        };
        return Created("/api/v1/members/interests", response);
    } 
}