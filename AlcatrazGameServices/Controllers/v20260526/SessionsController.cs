using Alcatraz.DTO.Models.v20260526;
using Alcatraz.GameServices.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Alcatraz.GameServices.Controllers.v20260526
{
	[ApiController]
	[Route("api/v20260526/sessions")]
	public class SessionsController : ControllerBase
	{
		[Authorize]
		[HttpGet]
		public IActionResult GetSessions()
		{
			var sessions = DSFServices.GameSessions.SessionList.Select(s => new SessionBrowserItem
			{
				Id = s.Id,
				TypeID = s.TypeID,
				HostPID = s.HostPID,
				HostURLs = s.HostURLs.Select(u => u.ToString()).ToList(),
				Attributes = s.Attributes,
				PublicParticipants = s.PublicParticipants.ToList(),
				PrivateParticipants = s.Participants.ToList()
			}).ToList();

			return Ok(sessions);
		}
	}
}
