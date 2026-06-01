using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Connection;
using QNetZ.DDL;

namespace DSFServices.Services
{
	public class MatchMakingRequest
	{
		public uint PID { get; set; }
		public QClient Client { get; set; }
		public PlayerSuggestionData Data { get; set; }
		public int RequestId { get; set; }
	}

	public static class MatchMakingManager
	{
		public static List<MatchMakingRequest> MatchmakingQueue = new List<MatchMakingRequest>();
		private static object _matchmakingLock = new object();

		public static void CheckMatches()
		{
			lock (_matchmakingLock)
			{
				var queue = MatchmakingQueue.ToList();
				QLog.WriteLine(1, $"Matchmaking: Checking for matches. Queue size: {queue.Count}");

				foreach (var p1 in queue)
				{
					if (!MatchmakingQueue.Contains(p1)) continue;

					foreach (var p2 in queue)
					{
						if (p1 == p2) continue;
						if (!MatchmakingQueue.Contains(p2)) continue;

						if (p1.Data.game_mode == p2.Data.game_mode)
						{
							var p1Session = GameSessions.SessionList.FirstOrDefault(x => x.HostPID == p1.PID);
							var p1Info = NetworkPlayers.GetPlayerInfoByPID(p1.PID);
							var p2Info = NetworkPlayers.GetPlayerInfoByPID(p2.PID);

							if (p1Info?.PlayerURLs.Count > 0)
							{
								DoMatch(p1, p2, p1Session, p1Info, p2Info);
								// MatchmakingQueue.Remove(p1);
								// MatchmakingQueue.Remove(p2);
								break;
							}
							// else
							// {
							// 	var p2Session = GameSessions.SessionList.FirstOrDefault(x => x.HostPID == p2.PID);
							// 	if (p2Session != null && p2Info?.PlayerURLs.Count > 0)
							// 	{
							// 		DoMatch(p2, p1, p2Session, p2Info, p1Info);
							// 		MatchmakingQueue.Remove(p1);
							// 		MatchmakingQueue.Remove(p2);
							// 		break;
							// 	}
							// }
						}
					}
				}
			}
		}

		private static void DoMatch(MatchMakingRequest host, MatchMakingRequest guest, GameSessionData hostSession, PlayerInfo hostInfo, PlayerInfo guestInfo)
		{
			QLog.WriteLine(1, $"Matchmaking: Found match! Host PID: {host.PID}, Guest PID: {guest.PID}, GameMode: {host.Data.game_mode}");

			var qosResult = new
			{
				qos_target = hostInfo.PlayerURLs.Count > 0 ? hostInfo.PlayerURLs[0].urlString : "",
				qos_id = host.RequestId,
				qos_target_profile_id = qUUID.FromPID(host.PID).ToString(),
				qos_target_2 = hostInfo.PlayerURLs.Count > 1 ? hostInfo.PlayerURLs[1].urlString : "",
				request_id = guest.RequestId
			};

			var matchResult = new
			{
				mission_id = host.Data.mission_id,
				bounty_level = host.Data.bounty_level,
				xp = host.Data.xp,
				host_request_id = host.RequestId,
				assignation_id = host.RequestId,
				sub_mission_id = host.Data.sub_mission_id,
				session_id = hostSession?.Id.ToString() ?? "0",
				host_profile_id = qUUID.FromPID(host.PID).ToString(),
				notoriety = host.Data.notoriety,
				role = 1,
				request_id = guest.RequestId,
				group_id = 250231, // Default group ID
				hack_defense = host.Data.hack_defense
			};

			var qosParam = new
			{
				code = 1,
				@params = new { json_result = JsonConvert.SerializeObject(qosResult) },
				facility = "ServerMatchMaking"
			};

			var matchParam = new
			{
				code = 0,
				@params = new { json_result = JsonConvert.SerializeObject(matchResult) },
				facility = "ServerMatchMaking"
			};

			var qosNotification = new NotificationEvent()
			{
				m_pidSource = 0,
				m_uiType = 0,
				m_uiParam1 = 0,
				m_uiParam2 = 0x00020000,
				m_strParam = JsonConvert.SerializeObject(qosParam)
			};

			var matchNotification = new NotificationEvent()
			{
				m_pidSource = 0,
				m_uiType = 0,
				m_uiParam1 = 0,
				m_uiParam2 = 0x00020000,
				m_strParam = JsonConvert.SerializeObject(matchParam)
			};

			NotificationQueue.AddNotification(qosNotification, guest.Client, 4500);
			NotificationQueue.AddNotification(matchNotification, guest.Client, 5000);
		}
	}
}
