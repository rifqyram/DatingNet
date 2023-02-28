using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/partners")]
public class PartnerController : BaseController
{
    private readonly IPartnerService _partnerService;

    public PartnerController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    [HttpGet]
    [SuppressMessage("ReSharper.DPA", "DPA0000: DPA issues")]
    public async Task<IActionResult> GetPartners([FromQuery] int page = 1, int size = 5)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        var personalInfoResponses = await _partnerService.FindPartners(claim.Value, page, size);
        var response = new CommonResponse<PageResponse<MemberPersonalInfoResponse>>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully find partner",
            Data = personalInfoResponses
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> ChoosePartner([FromBody] PartnerRequest request)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        request.MemberId = claim.Value;
        await _partnerService.CreateMemberPartner(request);
        var response = new CommonResponse<string>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create partner",
        };
        return Created("/api/v1/partners", response);
    }

    [HttpGet("my-matches")]
    public async Task<IActionResult> ListPartner()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();
        var partners = await _partnerService.ListPartner(claim.Value);
        var response = new CommonResponse<List<MemberPersonalInfoResponse>>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully fetch partner",
            Data = partners
        };
        return Ok(response);
    }
}