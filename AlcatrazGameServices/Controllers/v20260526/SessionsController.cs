using System.Diagnostics;
using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using QNetZ;
using Environment = QNetZ.Environment;

namespace Alcatraz.GameServices.Controllers.v20260526
{
	[ApiController]
	[Route("api/v{version}/sessions")]
	public class SessionsController : ControllerBase
	{
		[Authorize]
		[HttpGet]
		public IActionResult GetSessions()
		{
			if(QConfiguration.Instance.Environment == Environment.Prod) 
			{
				return ApiErrorHelper.CreateErrorResult(HttpContext, TNTMPErrorCode.GenericValidationError, "Session browsing not available in prod.", (int)HttpStatusCode.Unauthorized);
			}
			var sessions = DSFServices.GameSessions.SessionList.Select(s => {
				var hostPlayer = QNetZ.NetworkPlayers.GetPlayerInfoByPID(s.HostPID);
				return new SessionBrowserItem
				{
					id = s.Id,
					typeID = s.TypeID,
					hostPID = s.HostPID,
					hostURLs = (hostPlayer?.PlayerURLs ?? new System.Collections.Generic.List<QNetZ.DDL.StationURL>()).Select(u => u.ToString()).ToList(),
					attributes = s.Attributes,
					publicParticipants = s.PublicParticipants.ToList(),
					privateParticipants = s.Participants.ToList(),
					ownerNickname = hostPlayer?.Name ?? "Unknown"
				};
			}).ToList();

			return Ok(sessions);
		}
	}
}
