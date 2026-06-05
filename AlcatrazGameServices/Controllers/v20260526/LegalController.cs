using Alcatraz.DTO.Models.v20260526;
using Microsoft.AspNetCore.Mvc;
using QNetZ;

namespace Alcatraz.GameServices.Controllers.v20260526
{
    [ApiController]
    [Route("api/v20260526/legal")]
    public class LegalController : ControllerBase
    {
        [HttpGet("policies")]
        public IActionResult Policies()
        {
            return Ok(new LegalResponse
            {
                privacy = QConfiguration.Instance.LegalPolicies.Privacy,
                terms = QConfiguration.Instance.LegalPolicies.Terms
            });
        }
    }
}