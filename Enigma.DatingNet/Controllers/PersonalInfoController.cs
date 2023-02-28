using System.Net;
using System.Security.Claims;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/members/personal-info")]
public class PersonalInfoController : BaseController
{
    private readonly IMemberPersonalInfoService _memberPersonalInfoService;

    public PersonalInfoController(IMemberPersonalInfoService memberPersonalInfoService)
    {
        _memberPersonalInfoService = memberPersonalInfoService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePersonalInfo([FromForm] MemberPersonalInfoRequest request)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();

        request.MemberId = claim.Value;

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

    [HttpGet]
    public async Task<IActionResult> GetPersonalInfoByMemberId(string? memberId)
    {
        var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        var memberPersonalInfo = await _memberPersonalInfoService.GetByMemberId(memberId ?? claim.Value);
        var response = new CommonResponse<MemberPersonalInfoResponse>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully get personal info",
            Data = memberPersonalInfo
        };
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePersonalInfo([FromForm] MemberPersonalInfoUpdateRequest request)
    {
        var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        request.MemberId = claim.Value;
        var mpiResponse = await _memberPersonalInfoService.Update(request);
        var response = new CommonResponse<MemberPersonalInfoResponse>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully update personal info",
            Data = mpiResponse
        };
        return Ok(response);
    }
}