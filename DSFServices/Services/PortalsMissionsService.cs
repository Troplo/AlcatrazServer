using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using QNetZ.Connection;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.NexusOnly, Name = "PortalsMissionsProtocol")]
	class PortalsMissionsService : RMCServiceBase
	{
		static uint GatheringIdCounter = 39000;
		static List<SentInvitation> InvitationList = new List<SentInvitation>();

		[RMCMethod(0, "SetPlayerOpenForSuggestions_V6")]
		public RMCResult SetPlayerAvailableForMatchMaking(string playerInfoStr)
		{
			var playerInfo = JsonConvert.DeserializeObject<PlayerSuggestionDataPortals>(playerInfoStr);
			var plInfo = Context.Client.PlayerInfo;
			QLog.WriteLine(1, $"Player {plInfo.PID} is now available for invasion, {playerInfoStr}");
			return Result(new { retVal = true });
		}
	}
}
