using System.Net;
using System.Security.Claims;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/members/interests")]
public class InterestController : BaseController
{
    private readonly IMasterInterestService _interestService;
    private readonly IMemberInterestService _memberInterestService;

    public InterestController(IMasterInterestService interestService, IMemberInterestService memberInterestService)
    {
        _interestService = interestService;
        _memberInterestService = memberInterestService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateInterests([FromBody] MasterInterestRequest request)
    {
        var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        request.MemberId = claim.Value;
        var masterInterestsResponses = await _interestService.Create(request);
        var response = new CommonResponse<List<MasterInterestResponse>>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create interests",
            Data = masterInterestsResponses
        };
        return Created("/api/v1/members/interests", response);
    }

    [HttpGet]
    public async Task<IActionResult> GetInterests()
    {
        var interests = await _interestService.GetAll();
        var response = new CommonResponse<List<MasterInterestResponse>>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully get interests",
            Data = interests
        };
        return Ok(response);
    }

    [HttpGet("my-interests")]
    public async Task<IActionResult> GetMemberInterest()
    {
        var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        var miResponses = await _memberInterestService.FindListByMemberId(claim.Value);
        var response = new CommonResponse<List<MemberInterestResponse>>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully get member interests",
            Data = miResponses
        };

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMemberInterest([FromBody] MasterInterestRequest request)
    {
        var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        request.MemberId = claim.Value;
        var miResponses = await _interestService.Update(request);
        var response = new CommonResponse<List<MemberInterestResponse>>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully update member interests",
            Data = miResponses
        };
        return Ok(response);
    }
}