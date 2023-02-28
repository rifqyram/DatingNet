using System.Net;
using System.Security.Claims;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/members/preferences")]
public class PreferenceController : BaseController
{
    private readonly IMemberPreferencesService _preferencesService;

    public PreferenceController(IMemberPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePreferences([FromBody] MemberPreferencesRequest request)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();

        request.MemberId = claim.Value;

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

    [HttpGet]
    public async Task<IActionResult> GetPreferenceByMemberId(string? memberId)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();

        var preferencesResponse = await _preferencesService.FindByMemberId(memberId ?? claim.Value);
        var response = new CommonResponse<MemberPreferencesResponse>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully create preferences",
            Data = preferencesResponse
        };
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePreference([FromBody] MemberPreferencesUpdateRequest request)
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        if (claim is null) return Unauthorized();

        request.MemberId = claim.Value;
        
        var mpResponse = await _preferencesService.Update(request);
        var response = new CommonResponse<MemberPreferencesResponse>
        {
            Code = (int)HttpStatusCode.Created,
            Status = HttpStatusCode.Created.ToString(),
            Message = "Successfully update preferences",
            Data = mpResponse
        };

        return Ok(response);
    }
}