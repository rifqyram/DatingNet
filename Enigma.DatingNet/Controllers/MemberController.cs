using System.Net;
using System.Security.Claims;
using Enigma.DatingNet.Models.Requests;
using Enigma.DatingNet.Models.Responses;
using Enigma.DatingNet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[Route("api/v1/members")]
public class MemberController : BaseController
{
    private readonly IMemberInfoService _memberInfoService;
    private readonly IFileService _fileService;

    public MemberController(IMemberInfoService memberInfoService, IFileService fileService)
    {
        _memberInfoService = memberInfoService;
        _fileService = fileService;
    }

    [HttpPut("{memberId}")]
    public async Task<IActionResult> UpdateVerification(string memberId)
    {
        await _memberInfoService.VerificationUpdate(memberId);
        var response = new CommonResponse<string>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully update verification",
        };
        return Ok(response);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMySelf()
    {
        var claim = User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name));
        if (claim is null) return Unauthorized();

        var username = claim.Value;
        var member = await _memberInfoService.GetByUsername(username);

        var response = new CommonResponse<MemberInfoResponse>
        {
            Code = (int)HttpStatusCode.OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully get self info",
            Data = member
        };
        return Ok(response);
    }

    [HttpGet("profile-picture")]
    public async Task<IActionResult> DownloadImage([FromQuery] string path)
    {
        var file = await _fileService.DownloadFile(path);
        return File(file.MemoryStream, file.ContentType, file.Filename);
    }
}