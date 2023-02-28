using System.Net;
using System.Security.Claims;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/members/contact-info")]
public class ContactInfoController : BaseController
{
    private readonly IMemberContactInfoService _memberContactInfoService;

    public ContactInfoController(IMemberContactInfoService memberContactInfoService)
    {
        _memberContactInfoService = memberContactInfoService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContactInfo([FromBody] MemberContactInfoRequest request)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();

        request.MemberId = claim.Value;

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

    [HttpGet]
    public async Task<IActionResult> GetContactInfo(string? memberId)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        var id = claim.Value;
        var mciResponse = await _memberContactInfoService.GetByMemberId(memberId ?? id);
        var response = new CommonResponse<MemberContactInfoResponse>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully get contact info",
            Data = mciResponse
        };
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContactInfo([FromBody] MemberContactInfoUpdateRequest request)
    {
        var mciResponse = await _memberContactInfoService.Update(request);
        var response = new CommonResponse<MemberContactInfoResponse>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully update contact info",
            Data = mciResponse
        };
        return Ok(response);
    }
}