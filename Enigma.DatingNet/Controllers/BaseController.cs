using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enigma.DatingNet.Controllers;

[ApiController]
[Authorize]
public class BaseController : ControllerBase
{
    
}