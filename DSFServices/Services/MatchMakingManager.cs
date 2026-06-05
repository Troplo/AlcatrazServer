using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Connection;
using QNetZ.DDL;
using RDVServices;

namespace DSFServices.Services
{
	public class MatchMakingRequest
	{
		public uint PID { get; set; }
		public QClient Client { get; set; }
		public PlayerSuggestionData Data { get; set; }
		public int RequestId { get; set; }
	}

	public class MatchResultData
	{
		public uint mission_id { get; set; }
		public int bounty_level { get; set; }
		public int xp { get; set; }
		public uint host_request_id { get; set; }
		public uint assignation_id { get; set; }
		public int sub_mission_id { get; set; }
		public string session_id { get; set; }
		public string host_profile_id { get; set; }
		public int notoriety { get; set; }
		public int role { get; set; }
		public uint request_id { get; set; }
		public int group_id { get; set; }
		public int hack_defense { get; set; }
	}

	public class PendingMatch
	{
		public MatchResultData MatchResult { get; set; }
		public MatchResultData HostMatchResult { get; set; }
		public QClient GuestClient { get; set; }
		public QClient HostClient { get; set; }
		public uint QosId { get; set; }
		public bool IsImplicitHost { get; set; }
	}

	public static class MatchMakingManager
	{
		public static List<MatchMakingRequest> MatchmakingQueue = new List<MatchMakingRequest>();
		public static List<PendingMatch> PendingMatches = new List<PendingMatch>();
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

					bool joinedExisting = false;
					foreach (var session in GameSessions.SessionList)
					{
						// session.GameMode = GameMode.Freeroam;
						QLog.WriteLine(1, "Matchmaking queue" + p1.PID + " " + session.HostPID + " " + p1.Data.game_mode + " " + session.GameMode);
						if (session.HostPID == p1.PID) continue;

						int sessionGameMode = -1;
						if (session.Attributes.TryGetValue((uint)GameSessionAttributeType.GameMode, out uint gmVal))
						{
							sessionGameMode = (int)gmVal;
						}
						else
						{
							sessionGameMode = (int)session.GameMode;
						}

						uint value = 0;
					
// #if DEBUG
						// bool tntPrecheck = true;
// #else
						bool TryMatch(GameSessionAttributeType key, uint expected)
						{
							return session.Attributes.TryGetValue((uint)key, out var value) && value == expected;
						}

						
						bool hasIgnoreSessionSettings =
							(p1.Data.tnt_userSettings & UserSetting.AllowMatchingDifferentSessionSettings)
							== UserSetting.AllowMatchingDifferentSessionSettings;
						
						bool hasIgnoreGraphicsMods =
							(p1.Data.tnt_sessionSettings & SessionSetting.AllowModsMarkedAsGraphics)
							== SessionSetting.AllowModsMarkedAsGraphics;

						bool tntPrecheck =
							TryMatch(GameSessionAttributeType.TNT_Version, p1.Data.tnt_version) &&
							TryMatch(GameSessionAttributeType.TNT_ModsHashStd, p1.Data.tnt_modsHashStd) &&
							TryMatch(GameSessionAttributeType.TNT_ModsHashStd, p1.Data.tnt_modsHashStd) &&
							(hasIgnoreGraphicsMods || TryMatch(GameSessionAttributeType.TNT_ModsHashGfx, p1.Data.tnt_modsHashGfx)) &&
							 (TryMatch(GameSessionAttributeType.TNT_SessionSettings, (uint)p1.Data.tnt_sessionSettings) || hasIgnoreSessionSettings);
// #endif
						
						bool isSessionExactMatch = sessionGameMode != (int)GameMode.SinglePlayer && sessionGameMode == p1.Data.game_mode;
						bool isSessionInvasionMatch = (p1.Data.game_mode == (int)GameMode.MPHacking || p1.Data.game_mode == (int)GameMode.MPTailing) && sessionGameMode == 0 && p1.Data.allow_direct_invasion;

						if (!tntPrecheck) continue;
						
						if ((isSessionExactMatch || isSessionInvasionMatch))
						{
							var hostInfo = NetworkPlayers.GetPlayerInfoByPID(session.HostPID);
							if (hostInfo != null && hostInfo.PlayerURLs.Count > 0)
							{
								var hostRequest = queue.FirstOrDefault(x => x.PID == session.HostPID);
								var dummyHost = new MatchMakingRequest()
								{
									PID = session.HostPID,
									Client = hostInfo.Client,
									Data = p1.Data, // Use guest's data for the match result
									RequestId = hostRequest != null ? hostRequest.RequestId : new Random().Next(999999)
								};

								DoMatch(dummyHost, p1, session, hostInfo, NetworkPlayers.GetPlayerInfoByPID(p1.PID), isSessionInvasionMatch);
								MatchmakingQueue.Remove(p1);
								joinedExisting = true;
								break;
							}
						}
					}

					if (joinedExisting) continue;

					foreach (var p2 in queue)
					{
						if (p1 == p2) continue;
						if (!MatchmakingQueue.Contains(p2)) continue;

						QLog.WriteLine(1, "Matchmaking queue" + p1.PID + " " + p2.PID + " " + p1.Data.game_mode + " " + p2.Data.game_mode);
						bool isExactMatch = p1.Data.game_mode != (int)GameMode.SinglePlayer && p1.Data.game_mode == p2.Data.game_mode;
						bool isInvasionMatch = ((p1.Data.game_mode == (int)GameMode.MPHacking || p1.Data.game_mode == (int)GameMode.MPTailing) && p2.Data.game_mode == (int)GameMode.SinglePlayer && p2.Data.allow_direct_invasion) ||
											   ((p2.Data.game_mode == (int)GameMode.MPHacking || p2.Data.game_mode == (int)GameMode.MPTailing) && p1.Data.game_mode == (int)GameMode.SinglePlayer && p1.Data.allow_direct_invasion);

						if (isExactMatch || isInvasionMatch)
						{
							var p1Session = GameSessions.SessionList.FirstOrDefault(x => x.HostPID == p1.PID);
							var p1Info = NetworkPlayers.GetPlayerInfoByPID(p1.PID);
							var p2Session = GameSessions.SessionList.FirstOrDefault(x => x.HostPID == p2.PID);
							var p2Info = NetworkPlayers.GetPlayerInfoByPID(p2.PID);

							if (isInvasionMatch)
							{
								// The host MUST be the player in SinglePlayer (game_mode == 0)
								if (p1.Data.game_mode == (int)GameMode.SinglePlayer && p1Info?.PlayerURLs.Count > 0)
								{
									DoMatch(p1, p2, p1Session, p1Info, p2Info, true);
									MatchmakingQueue.Remove(p1);
									MatchmakingQueue.Remove(p2);
									break;
								}
								else if (p2.Data.game_mode == (int)GameMode.SinglePlayer && p2Info?.PlayerURLs.Count > 0)
								{
									DoMatch(p2, p1, p2Session, p2Info, p1Info, true);
									MatchmakingQueue.Remove(p1);
									MatchmakingQueue.Remove(p2);
									break;
								}
							}
							else
							{
								// For exact matches, anyone can be host
								if (p1Info?.PlayerURLs.Count > 0)
								{
									DoMatch(p1, p2, p1Session, p1Info, p2Info, false);
									MatchmakingQueue.Remove(p1);
									MatchmakingQueue.Remove(p2);
									break;
								}
								else if (p2Info?.PlayerURLs.Count > 0)
								{
									DoMatch(p2, p1, p2Session, p2Info, p1Info, false);
									MatchmakingQueue.Remove(p1);
									MatchmakingQueue.Remove(p2);
									break;
								}
							}
						}
					}
				}
			}
		}

		private static void DoMatch(MatchMakingRequest host, MatchMakingRequest guest, GameSessionData hostSession, PlayerInfo hostInfo, PlayerInfo guestInfo, bool isInvasionMatch = false)
		{
			QLog.WriteLine(1, $"Matchmaking: Found match! Host PID: {host.PID}, Guest PID: {guest.PID}, GameMode: {host.Data.game_mode}");

			bool isImplicitHost = isInvasionMatch || hostSession == null;

			if (hostSession == null)
			{
				hostSession = new GameSessionData();
				hostSession.Id = hostSession.GenerateId();
				hostSession.HostPID = host.PID;
				hostSession.TypeID = 1;
				hostSession.GameMode = (GameMode)host.Data.game_mode;

				hostSession.Attributes[(uint)GameSessionAttributeType.PublicSlots] = 8;
				hostSession.Attributes[(uint)GameSessionAttributeType.PrivateSlots] = 8;
				hostSession.Attributes[(uint)GameSessionAttributeType.FilledPublicSlots] = 1;
				hostSession.Attributes[(uint)GameSessionAttributeType.FilledPrivateSlots] = 0;
				hostSession.Attributes[(uint)GameSessionAttributeType.GameMode] = (uint)hostSession.GameMode; // MM_SESSION_GAME_MODE
				
				hostSession.PublicParticipants.Add(host.PID);

				GameSessions.SessionList.Add(hostSession);
				QLog.WriteLine(1, $"Matchmaking: Implicitly created session {hostSession.Id} for Host PID {host.PID}");
			}
			else
			{
				hostSession.GameMode = (GameMode)host.Data.game_mode;
			}

			var qosResult = new
			{
				qos_target = hostInfo.PlayerURLs.Count > 0 ? hostInfo.PlayerURLs[0].urlString + "," : "",
				qos_id = host.RequestId,
				qos_target_profile_id = qUUID.FromPID(host.PID).ToString() + ",",
				qos_target_2 = hostInfo.PlayerURLs.Count > 1 ? hostInfo.PlayerURLs[1].urlString + "," : "",
				request_id = guest.RequestId
			};

			var matchResult = new MatchResultData
			{
				mission_id = host.Data.mission_id,
				bounty_level = host.Data.bounty_level,
				xp = host.Data.xp,
				host_request_id = (uint)host.RequestId,
				assignation_id = (uint)host.RequestId,
				sub_mission_id = host.Data.sub_mission_id,
				session_id = hostSession?.Id.ToString() ?? "0",
				host_profile_id = DBHelper.GetUserByPID(host.PID).Guid.ToString(),
				notoriety = host.Data.notoriety,
				role = guest.Data.game_mode == (int)GameMode.MPTailing ? 2 : 1,
				request_id = (uint)guest.RequestId,
				group_id = 250231,
				hack_defense = host.Data.hack_defense
			};

			var hostMatchResult = new MatchResultData
			{
				mission_id = host.Data.mission_id,
				bounty_level = guest.Data.bounty_level,
				xp = guest.Data.xp,
				host_request_id = (uint)host.RequestId,
				assignation_id = (uint)host.RequestId,
				sub_mission_id = host.Data.sub_mission_id,
				session_id = hostSession?.Id.ToString() ?? "0",
				host_profile_id = DBHelper.GetUserByPID(guest.PID).Guid.ToString(),
				notoriety = guest.Data.notoriety,
				role = 0,
				request_id = (uint)host.RequestId,
				group_id = 250231,
				hack_defense = guest.Data.hack_defense
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

			var hostMatchParam = new
			{
				code = 0,
				@params = new { json_result = JsonConvert.SerializeObject(hostMatchResult) },
				facility = "ServerMatchMaking"
			};

			var qosNotification = new NotificationEvent()
			{
				m_pidSource = 0,
				m_uiType = 0,
				m_uiParam1 = 0,
				m_uiParam2 = 0,
				m_strParam = JsonConvert.SerializeObject(qosParam)
			};

			//0x00020000 for m_uiParam2 does force join
			var matchNotification = new NotificationEvent()
			{
				m_pidSource = 0,
				m_uiType = 0,
				m_uiParam1 = 0,
				m_uiParam2 = 0,
				m_strParam = JsonConvert.SerializeObject(matchParam)
			};

			var hostMatchNotification = new NotificationEvent()
			{
				m_pidSource = 0,
				m_uiType = 0,
				m_uiParam1 = 0,
				m_uiParam2 = 0,
				m_strParam = JsonConvert.SerializeObject(hostMatchParam)
			};

			NotificationQueue.AddNotification(qosNotification, guest.Client, 4500);
			// NotificationQueue.AddNotification(matchNotification, guest.Client, 5000);
			
			lock (_matchmakingLock)
			{
				PendingMatches.Add(new PendingMatch()
				{
					MatchResult = matchResult,
					HostMatchResult = hostMatchResult,
					GuestClient = guest.Client,
					HostClient = hostInfo.Client,
					QosId = (uint)host.RequestId,
					IsImplicitHost = isImplicitHost
				});
			}
		}

		public static void OnQoSResult(uint qosId, int qosResult)
		{
			lock (_matchmakingLock)
			{
				var match = PendingMatches.FirstOrDefault(x => x.QosId == qosId);
				if (match != null)
				{
					if (match.IsImplicitHost)
					{
						QLog.WriteLine(1, $"Matchmaking: Received QoS result {qosResult} for QosId {qosId}. Sending proposition to host (Implicit Session).");
						if (match.HostClient != null)
						{
							var hostMatchParam = new
							{
								code = 0,
								@params = new { json_result = JsonConvert.SerializeObject(match.HostMatchResult) },
								facility = "ServerMatchMaking"
							};

							var hostMatchNotification = new NotificationEvent()
							{
								m_pidSource = 0,
								m_uiType = 0,
								m_uiParam1 = 0,
								m_uiParam2 = 0,
								m_strParam = JsonConvert.SerializeObject(hostMatchParam)
							};

							NotificationQueue.AddNotification(hostMatchNotification, match.HostClient, 500);
						}
						// host will create the session and then we'll emit to client in OnHostCreatedSession
					}
					else
					{
						QLog.WriteLine(1, $"Matchmaking: Received QoS result {qosResult} for QosId {qosId}. Sending proposition to guest (Explicit Session).");
						var matchParam = new
						{
							code = 0,
							@params = new { json_result = JsonConvert.SerializeObject(match.MatchResult) },
							facility = "ServerMatchMaking"
						};

						var matchNotification = new NotificationEvent()
						{
							m_pidSource = 0,
							m_uiType = 0,
							m_uiParam1 = 0,
							m_uiParam2 = 0,
							m_strParam = JsonConvert.SerializeObject(matchParam)
						};

						NotificationQueue.AddNotification(matchNotification, match.GuestClient, 500);
						PendingMatches.Remove(match);
					}
				}
				else
				{
					QLog.WriteLine(1, $"Matchmaking: Received QoS result {qosResult} for QosId {qosId} but no pending match found.");
				}
			}
		}

		public static void OnHostCreatedSession(QClient hostClient, uint sessionId)
		{
			lock (_matchmakingLock)
			{
				var match = PendingMatches.FirstOrDefault(x => x.HostClient == hostClient);
				if (match != null)
				{
					match.MatchResult.session_id = sessionId.ToString();
					var matchParam = new
					{
						code = 0,
						@params = new { json_result = JsonConvert.SerializeObject(match.MatchResult) },
						facility = "ServerMatchMaking"
					};

					var matchNotification = new NotificationEvent()
					{
						m_pidSource = 0,
						m_uiType = 0,
						m_uiParam1 = 0,
						m_uiParam2 = 0,
						m_strParam = JsonConvert.SerializeObject(matchParam)
					};

					QLog.WriteLine(1, $"Matchmaking: Host created session {sessionId}. Sending proposition to guest.");
					NotificationQueue.AddNotification(matchNotification, match.GuestClient, 500);
					PendingMatches.Remove(match);
				}
			}
		}
	}
}
