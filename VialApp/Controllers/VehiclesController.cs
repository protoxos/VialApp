using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VialApp.Infrastructure.Permission;
using VialApp.Models;
using VialApp.Services;
using VialApp.Tools;

namespace VialApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiclesController: ControllerBase
    {
        private readonly ILogger<VehicleModel> _logger;
        private readonly UserModel _user;
        private readonly VehicleService vehicleService;

        public VehiclesController(IUserService user, ILogger<VehicleModel> logger)
        {
            _user = user.GetUser(User);
            _logger = logger;
            vehicleService = new VehicleService();
        }

        [HttpGet()]
        public IActionResult Get()
        {
            ApiResponse<List<VehicleModel>, ApiStatusResponse> apiResponse = 
                new ApiResponse<List<VehicleModel>, ApiStatusResponse> { 
                    Status = ApiStatusResponse.Success, 
                    Data = vehicleService.GetAll(_user.Id) 
                };
            return Ok(apiResponse);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            VehicleModel? v = vehicleService.Get(_user.Id, id);
            ApiResponse<VehicleModel, ApiStatusResponse>  apiResponse = 
                new ApiResponse<VehicleModel, ApiStatusResponse> { 
                    Status = v == null ? ApiStatusResponse.Error : ApiStatusResponse.Success, 
                    Message = "No se encontró el vehiculo",
                    Data = v
                };
            return Ok(apiResponse);
        }

        [HttpPost()]
        public IActionResult Post(VehicleModel? vehicle)
        {
            if (vehicle != null)
            {
                VehicleModel? v = vehicleService.Create(_user.Id, vehicle);

                ApiResponse<VehicleModel, ApiStatusResponse> apiResponse =
                    new ApiResponse<VehicleModel, ApiStatusResponse>
                    {
                        Status = v == null ? ApiStatusResponse.Error : ApiStatusResponse.Success,
                        Message = "No pudo crear el vehiculo",
                        Data = v
                    };
                return Ok(apiResponse);
            }
            return BadRequest();
        }
        [HttpPut()]
        public IActionResult Put(VehicleModel? vehicle)
        {
            if (vehicle != null)
            {
                VehicleModel? v = vehicleService.Update(_user.Id, vehicle);

                ApiResponse<VehicleModel, ApiStatusResponse> apiResponse =
                    new ApiResponse<VehicleModel, ApiStatusResponse>
                    {
                        Status = v == null ? ApiStatusResponse.Error : ApiStatusResponse.Success,
                        Message = "No pudo actualizar el vehiculo",
                        Data = v
                    };
                return Ok(apiResponse);
            }
            return BadRequest();
        }
        [HttpDelete()]
        public IActionResult Delete(int VehicleId)
        {
            bool deleted = vehicleService.Delete(_user.Id, VehicleId);

            ApiResponse<VehicleModel, ApiStatusResponse> apiResponse =
                new ApiResponse<VehicleModel, ApiStatusResponse>
                {
                    Status = deleted ? ApiStatusResponse.Error : ApiStatusResponse.Success,
                    Message = "No pudo borrar el vehiculo"
                };
            return Ok(apiResponse);
        }

    }
}