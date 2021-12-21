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

        [HttpPost("create")]
        public IActionResult Create([FromBody] UserModel model)
        {
            // Preparamos la respuesta base
            ApiResponse<UserModel, ApiStatusResponse> res = new ApiResponse<UserModel, ApiStatusResponse>
            {
                Status = ApiStatusResponse.Error,
                Message = "No se ha podido crear el usuario",
                Data = null
            };

            // Validamos que no exista
            UserModel? userReg = _userService.GetUserByEmail(model.Email);
            if (userReg != null)
            {
                res.Status = ApiStatusResponse.Error;
                res.Message = "Ya existe un usuario registrado con ese email";
                res.Data = null;
                return Ok(res);
            }

            userReg = _userService.Create(model);
            if(userReg != null)
            {
                res.Status = ApiStatusResponse.Success;
                res.Message = $"Se ha enviado un email de verificación a {userReg.Email}";
                res.Data = userReg;
                return Ok(res);
            }


            return Ok(res);
        }
    }
}
