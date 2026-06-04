using System;
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
		public RMCResult SetPlayerOpenForSuggestions(bool isOpen, string playerInfoStr, uint arg3, uint arg4, uint arg5)
		{
			var playerInfo = JsonConvert.DeserializeObject<PlayerSuggestionDataPortals>(playerInfoStr);
			var plInfo = Context.Client.PlayerInfo;
			QLog.WriteLine(1, $"Player {plInfo.PID} is now available for invasion, {isOpen}, {playerInfoStr}, {arg3}, {arg4}, {arg5}");

			if (isOpen)
			{
				Random rand = new Random();
				int id = rand.Next(100000, 999999);

				// MatchMakingManager.MatchmakingQueue.Add(new MatchMakingRequest()
				// {
				// 	PID = plInfo.PID,
				// 	Client = plInfo.Client,
				// 	Data = new PlayerSuggestionData()
				// 	{
				// 		game_mode = (int)GameMode.Portal,
				// 		mission_id = 3,
				// 		sub_mission_id = 8,
				// 		nat_type = playerInfo.nat_type,
				// 		roles_bitmask = playerInfo.roles_bitmask,
				// 		game_version = playerInfo.game_version,
				// 		notoriety = playerInfo.notoriety,
				// 		origin = playerInfo.origin,
				// 		xp = playerInfo.xp,
				// 		hack_defense = playerInfo.hack_defense,
				// 		time_available = playerInfo.time_available
				// 	},
				// 	RequestId = id
				// });

				// MatchMakingManager.CheckMatches();
				return Result(new { request_id = (uint)id, interval = 33u });
			}
			else
			{
				// MatchMakingManager.MatchmakingQueue.RemoveAll(x => x.PID == plInfo.PID && x.Data.game_mode == (int)GameMode.Portal);

				var session = GameSessions.SessionList.FirstOrDefault(x => x.HostPID == plInfo.PID && x.GameMode == GameMode.MPHacking);
				if (session != null)
				{
					GameSessions.SessionList.Remove(session);
					plInfo.GameData().CurrentSession = null;
				}

				return Result(new { request_id = 0u, interval = 33u });
			}
		}
	}
}
