using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Mvc;
using QNetZ;

namespace Alcatraz.GameServices.Controllers.v20260526
{
    [ApiController]
    [Route("api/v{version}/state")]
    public class StateController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult State()
        {
            return Ok(new StateResponse
            {
                environment = (Alcatraz.DTO.Models.v20260526.Environment)QConfiguration.Instance.Environment,
                maintenanceConfig = new Alcatraz.DTO.Models.v20260526.MaintenanceConfig
                {
                    title = QConfiguration.Instance.MaintenanceConfig.title,
                    message = QConfiguration.Instance.MaintenanceConfig.message,
                    enabled = QConfiguration.Instance.MaintenanceConfig.enabled
                },
                websiteBaseUrl = QConfiguration.Instance.ServerBaseUrl,
                allowRegistrations = QConfiguration.Instance.AllowRegistrations
            });
        }
    }
}