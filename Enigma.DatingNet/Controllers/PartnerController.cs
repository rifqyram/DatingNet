using System.Net;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Repositories;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/partners")]
public class PartnerController : BaseController
{
    private readonly IPartnerService _partnerService;
    
    public PartnerController(IPartnerService partnerService)
    {
        _partnerService = partnerService;
    }

    [HttpGet("{memberId}")]
    public async Task<IActionResult> GetPartners(string memberId, [FromQuery] int page = 1)
    {
        var personalInfoResponses = await _partnerService.FindPartners(memberId, page);
        var response = new CommonResponse<List<MemberPersonalInfoResponse>>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully find partner",
            Data = personalInfoResponses
        };
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> ChoosePartner(PartnerRequest request)
    {
        await _partnerService.CreateMemberPartner(request);
        var response = new CommonResponse<string>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create partner",
        };
        return Created("/api/v1/partners", response);
    }

    [HttpGet]
    public async Task<IActionResult> ListPartner([FromQuery] string memberId)
    {
        var partners = await _partnerService.ListPartner(memberId);
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