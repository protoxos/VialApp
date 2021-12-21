using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VialApp.Models;
using VialApp.Services;
using VialApp.Tools;

namespace VialApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthRequest model)
        {
            ApiResponse<string, ApiStatusResponse> res = new ApiResponse<string, ApiStatusResponse>
            {
                Status = ApiStatusResponse.Error,
                Message = "Usuario y/o contraseña incorrectos",
                Data = ""
            };
            string token = _userService.Auth(model);

            if (!string.IsNullOrEmpty(token))
            {
                res.Status = ApiStatusResponse.Success;
                res.Message = "";
                res.Data = token;
            }
            return Ok(res);
        }
    }
}
