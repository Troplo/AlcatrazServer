using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DSFServices.Services
{
	/// <summary>
	/// Game session 
	///		Implements the sessions responsible for the gameplay process
	/// </summary>
	[RMCService(RMCProtocolId.GameSessionService, Name = "GameSessionProtocol")]
	public class GameSessionService : RMCServiceBase
	{
		static uint GameSessionCounter = 22110;

		[RMCMethod(1, "CreateSession_V1")]
		public RMCResult CreateSession(GameSession gameSession)
		{
			QLog.WriteLine(1, "GameSessionService.CreateSession called with typeID=" + gameSession.m_typeID);
			var plInfo = Context.Client.PlayerInfo;
			var newSession = new GameSessionData();
			GameSessions.SessionList.Add(newSession);

			newSession.Id = ++GameSessionCounter;
			newSession.HostPID = plInfo.PID;
			newSession.TypeID = gameSession.m_typeID;

			foreach (var attr in gameSession.m_attributes)
				newSession.Attributes[attr.ID] = attr.Value;

			uint temp;
			if(!newSession.Attributes.TryGetValue((uint)GameSessionAttributeType.PublicSlots, out temp))
				newSession.Attributes[(uint)GameSessionAttributeType.PublicSlots] = 0;

			if (!newSession.Attributes.TryGetValue((uint)GameSessionAttributeType.PrivateSlots, out temp))
				newSession.Attributes[(uint)GameSessionAttributeType.PrivateSlots] = 8;

			if (!newSession.Attributes.TryGetValue((uint)GameSessionAttributeType.GameType, out temp))
				newSession.Attributes[(uint)GameSessionAttributeType.GameType] = (uint)GameType.FreeForAll;

			newSession.Attributes[(uint)GameSessionAttributeType.FilledPublicSlots] = (uint)newSession.PublicParticipants.Count;
			newSession.Attributes[(uint)GameSessionAttributeType.FilledPrivateSlots] = (uint)newSession.Participants.Count;

			// TODO: give names to attributes
			if (!newSession.Attributes.TryGetValue(100, out temp))
				newSession.Attributes[100] = 0;

			if (!newSession.Attributes.TryGetValue(101, out temp))
				newSession.Attributes[101] = 0;

			if (!newSession.Attributes.TryGetValue(104, out temp))
				newSession.Attributes[104] = 0;

			if (!newSession.Attributes.TryGetValue(113, out temp))
				newSession.Attributes[113] = 0;

			// return key
			var result = new GameSessionKey();
			result.m_sessionID = newSession.Id;
			result.m_typeID = newSession.TypeID;

			return Result(result);
		}

		[RMCMethod(2, "UpdateSession_V1")]
		public RMCResult UpdateSession(GameSessionUpdate gameSessionUpdate)
		{
			var session = GameSessions.SessionList
				.FirstOrDefault(x => x.Id == gameSessionUpdate.m_sessionKey.m_sessionID && 
									 x.TypeID == gameSessionUpdate.m_sessionKey.m_typeID);

			if(session != null)
			{
				// update or add attributes
				foreach (var attr in gameSessionUpdate.m_attributes)
				{
					session.Attributes[attr.ID] = attr.Value;
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.UpdateSession - no session with id={gameSessionUpdate.m_sessionKey.m_sessionID}");
			}

			return Error(0);
		}


		[RMCMethod(3)]
		public RMCResult DeleteSession(GameSessionKey gameSessionKey)
		{
			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(4)]
		public RMCResult MigrateSession(GameSessionKey gameSessionKey)
		{
			var oldSession = GameSessions.SessionList
				.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID &&
									 x.TypeID == gameSessionKey.m_typeID);
			if (oldSession == null)
			{
				QLog.WriteLine(1, $"Error : GameSessionService.MigrateSession - no session with id={gameSessionKey.m_sessionID}");
				return Result(new GameSessionKey());
			}

			// ????
			// "notification": {
			// 	"m_pidSource": 539625,
			// 	"m_uiType": 7001,
			// 	"m_uiParam1": 31,
			// 	"m_uiParam2": 30,
			// 	"m_strParam": "",
			// 	"m_uiParam3": 1
			//   }

			var newSession = new GameSessionData();
			GameSessions.SessionList.Add(newSession);

			newSession.Id = ++GameSessionCounter;
			newSession.HostPID =  Context.Client.PlayerInfo.PID;
			newSession.TypeID = oldSession.TypeID;
			newSession.Participants = oldSession.Participants;
			newSession.PublicParticipants = oldSession.PublicParticipants;

			foreach (var attr in oldSession.Attributes)
				newSession.Attributes[attr.Key] = attr.Value;

			var newSessionKey = new GameSessionKey();
			newSessionKey.m_sessionID = newSession.Id;
			newSessionKey.m_typeID = newSession.TypeID;

			// move all participants (change session key)
			foreach (var pid in oldSession.PublicParticipants)
			{
				var participantPlayerInfo = NetworkPlayers.GetPlayerInfoByPID(pid);

				if (participantPlayerInfo != null)
					participantPlayerInfo.GameData().CurrentSession = newSessionKey;
			}

			foreach (var pid in oldSession.Participants)
			{
				var participantPlayerInfo = NetworkPlayers.GetPlayerInfoByPID(pid);

				if (participantPlayerInfo != null)
					participantPlayerInfo.GameData().CurrentSession = newSessionKey;
			}

			// drop old session
			QLog.WriteLine(1, $"MigrateSession - Auto-deleted session {oldSession.Id}");
			GameSessions.SessionList.Remove(oldSession);

			return Result(newSessionKey);
		}


		[RMCMethod(5, "LeaveSession_V1")]
		public RMCResult LeaveSession(GameSessionKey gameSessionKey)
		{
			// Same as AbandonSession
			var playerInfo = Context.Client.PlayerInfo;
			var myPlayerId = playerInfo.PID;
			var session = GameSessions.SessionList
				.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID && 
									 x.TypeID == gameSessionKey.m_typeID);

			if(session != null)
			{
				// send - could be invalid!!!
				//{
				//  "notification": {
				//	"m_pidSource": 25447,	// ???
				//	"m_uiType": 7004,		// GameSessionEvent
				//	"m_uiParam1": 539625,	// participantID
				//	"m_uiParam2": 27,		// gameSessionKey.m_sessionID
				//	"m_strParam": "",
				//	"m_uiParam3": 1			// gameSessionKey.m_typeID ??? not sure...
				//  }
				//}

				// send to all session members
				foreach (var pid in session.Participants)
				{
					var qclient = Context.Handler.GetQClientByClientPID(pid);

					if (qclient != null)
					{
						var leaveNotification = new NotificationEvent(NotificationEventsType.GameSessionEvent, 4)
						{
							m_pidSource = playerInfo.PID,
							m_uiParam1 = playerInfo.PID,
							m_uiParam2 = session.Id,
							m_strParam = "",
							m_uiParam3 = session.TypeID
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, leaveNotification);
					}
				}

				GameSessions.UpdateSessionParticipation(playerInfo, null, false);
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.LeaveSession - no session with id={gameSessionKey.m_sessionID}");
			}

			return Error(0);
		}


		[RMCMethod(6, "GetSession_V1")]
		public RMCResult GetSession(GameSessionKey gameSessionKey)
		{
			var searchResult = new GameSessionSearchResult();
			QLog.WriteLine(1, $"GetSession_V1: KeySid={gameSessionKey.m_sessionID}, KeyType={gameSessionKey.m_typeID} player={Context.Client.PlayerInfo?.Name}");

			var session = GameSessions.SessionList.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID && x.TypeID == gameSessionKey.m_typeID);

			if (session != null)
			{
				var hostPlayer = NetworkPlayers.GetPlayerInfoByPID(session.HostPID);

				searchResult = new GameSessionSearchResult()
				{
					m_hostPID = qUUID.FromPID(session.HostPID),
					m_hostURLs = hostPlayer?.PlayerURLs ?? new List<StationURL>(),
					m_attributes = session.Attributes.Select(x => new GameSessionProperty { ID = x.Key, Value = x.Value }).ToArray(),
					m_sessionKey = new GameSessionKey()
					{
						m_sessionID = session.Id,
						m_typeID = session.TypeID
					}
				};
				QLog.WriteLine(1, $"GetSession_V1: session={session.Id} player={Context.Client.PlayerInfo?.Name}");
			}
			else
			{
				QLog.WriteLine(1, $"GetSession_V1: session=None");
			}

			return Result(searchResult);
		}


		[RMCMethod(7, "SearchSessions_V1")]
		public RMCResult SearchSessions(uint m_typeID, uint m_queryID, IEnumerable<GameSessionProperty> m_parameters)
		{
			var sessions = GameSessions.SessionList.Where(x => x.TypeID == m_typeID).ToArray();

			var resultList = new List<GameSessionSearchResult>();

			foreach (var ses in sessions)
			{
				uint value;

				// cut out *private* sessions completely
				if (ses.Attributes.TryGetValue((uint)GameSessionAttributeType.FreePrivateSlots, out value) && value > 0 ||
					ses.Attributes.TryGetValue((uint)GameSessionAttributeType.PrivateSlots, out value) && value > 0 ||
					ses.Attributes.TryGetValue((uint)GameSessionAttributeType.FilledPrivateSlots, out value) && value > 0)
					continue;

				var gameTypeMinParam = m_parameters.FirstOrDefault(x => x.ID == (uint)GameSessionAttributeType.GameTypeMin);
				var gameTypeMaxParam = m_parameters.FirstOrDefault(x => x.ID == (uint)GameSessionAttributeType.GameTypeMax);
				var totalPublicSlotsParam = m_parameters.FirstOrDefault(x => x.ID == (uint)GameSessionAttributeType.PublicSlots);

				uint sessionGameType = ses.Attributes[(uint)GameSessionAttributeType.GameType];

				// check game mode matches criteria
				// and if there are free slots
				if(sessionGameType >= gameTypeMinParam.Value && sessionGameType <= gameTypeMaxParam.Value &&
					ses.PublicParticipants.Count < totalPublicSlotsParam.Value)
				{
					var hostPlayer = NetworkPlayers.GetPlayerInfoByPID(ses.HostPID);

					resultList.Add(new GameSessionSearchResult()
					{
						m_hostPID = qUUID.FromPID(ses.HostPID),
						m_hostURLs = hostPlayer?.PlayerURLs ?? new List<StationURL>(),
						m_attributes = ses.Attributes.Select(x => new GameSessionProperty { ID = x.Key, Value = x.Value }).ToArray(),
						m_sessionKey = new GameSessionKey()
						{
							m_sessionID = ses.Id,
							m_typeID = ses.TypeID
						},
					});
				}
			}

			return Result(resultList);
		}


		[RMCMethod(8, "AddParticipants_V1")]
		public RMCResult AddParticipants(GameSessionKey gameSessionKey, IEnumerable<uint> publicParticipantIDs, IEnumerable<uint> privateParticipantIDs)
		{
			var session = GameSessions.SessionList.FirstOrDefault(x => x.IsMatchingKey(gameSessionKey));

			if(session != null)
			{
				foreach (var pid in publicParticipantIDs)
				{
					session.PublicParticipants.Add(pid);

					var player = NetworkPlayers.GetPlayerInfoByPID(pid);
					if (player != null)
					{
						GameSessions.UpdateSessionParticipation(player, gameSessionKey, false);
					}
				}

				foreach (var pid in privateParticipantIDs)
				{
					session.Participants.Add(pid);

					var player = NetworkPlayers.GetPlayerInfoByPID(pid);
					if (player != null)
					{
						GameSessions.UpdateSessionParticipation(player, gameSessionKey, true);
					}
				}

				session.Attributes[(uint)GameSessionAttributeType.FilledPublicSlots] = (uint)session.PublicParticipants.Count;
				session.Attributes[(uint)GameSessionAttributeType.FilledPrivateSlots] = (uint)session.Participants.Count;
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.AddParticipants - no session with id={gameSessionKey.m_sessionID}");
			}

			return Error(0);
		}

		[RMCMethod(9, "RemoveParticipants_V1")]
		public RMCResult RemoveParticipants(GameSessionKey gameSessionKey, IEnumerable<uint> participantIDs)
		{
			var session = GameSessions.SessionList.FirstOrDefault(x => x.IsMatchingKey(gameSessionKey));

			if (session != null)
			{
				// TODO: send
				//{
				//  "notification": {
				//	"m_pidSource": 25447,	// ???
				//	"m_uiType": 7004,		// GameSessionEvent
				//	"m_uiParam1": 539625,	// participantID
				//	"m_uiParam2": 27,		// gameSessionKey.m_sessionID
				//	"m_strParam": "",
				//	"m_uiParam3": 1			// gameSessionKey.m_typeID
				//  }
				//}

				foreach (var pid in participantIDs)
				{
					var player = NetworkPlayers.GetPlayerInfoByPID(pid);
					if (player != null)
					{
						GameSessions.UpdateSessionParticipation(player, null, false);
					}
					else if (GameSessions.RemovePlayerFromSession(session, pid))
					{
						QLog.WriteLine(1, $"RemoveParticipants - Auto-deleted session {session.Id}");
						GameSessions.SessionList.Remove(session);
					}
				}

				foreach (var pid in participantIDs)
				{
					var player = NetworkPlayers.GetPlayerInfoByPID(pid);
					if (player != null)
					{
						GameSessions.UpdateSessionParticipation(player, null, false);
					}
					else if (GameSessions.RemovePlayerFromSession(session, pid))
					{
						QLog.WriteLine(1, $"RemoveParticipants - Auto-deleted session {session.Id}");
						GameSessions.SessionList.Remove(session);
					}
				}

				session.Attributes[(uint)GameSessionAttributeType.FilledPublicSlots] = (uint)session.PublicParticipants.Count;
				session.Attributes[(uint)GameSessionAttributeType.FilledPrivateSlots] = (uint)session.Participants.Count;
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.RemoveParticipants - no session with id={gameSessionKey.m_sessionID}");
			}

			return Error(0);
		}


		[RMCMethod(10, "GetParticipantCount_V1")]
		public RMCResult GetParticipantCount(GameSessionKey gameSessionKey, IEnumerable<uint> participantIDs)
		{
			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(11, "GetParticipants_V1")]
		public void GetParticipants()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(12)]
		public void SendInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(13)]
		public void GetInvitationReceivedCount()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(14)]
		public void GetInvitationsReceived()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(15)]
		public void GetInvitationSentCount()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(16)]
		public void GetInvitationsSent()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(17)]
		public void AcceptInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(18)]
		public void DeclineInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(19)]
		public void CancelInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(20)]
		public void SendTextMessage()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(21, Name = "RegisterURLs_V1")]
		public RMCResult RegisterURLs(IEnumerable<StationURL> stationURLs)
		{
			var plInfo = Context.Client.PlayerInfo;
			var myPlayerId = plInfo.PID;
			var session = GameSessions.SessionList.FirstOrDefault(x => x.HostPID == myPlayerId);
			
			plInfo.PlayerURLs.Clear();
			if (stationURLs != null)
			{
				plInfo.PlayerURLs.AddRange(stationURLs);
			}

			QLog.WriteLine(1, $"Session hosted by pid={myPlayerId}. Session={session?.Id}, URLs: {string.Join(", ", stationURLs.Select(x => x.ToString()))}");

			return Result(new { retVal = true });
		}


		[RMCMethod(22, "JoinSession_V1")]
		public void JoinSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(23, Name = "AbandonSession_V1")]
		public RMCResult AbandonSession(GameSessionKey gameSessionKey)
		{
			return LeaveSession(gameSessionKey);
		}


		[RMCMethod(24)]
		public void SearchSessionsWithParticipants()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(25, "GetSessions_V1")]
		public void GetSessions()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(26)]
		public void GetParticipantsURLs()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(27)]
		public void MigrateSessionHost()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(28, "SplitSession_V1")]
		public RMCResult SplitSession(GameSessionKey gameSessionKey)
		{
			var session = GameSessions.SessionList.FirstOrDefault(x =>
				x.Id == gameSessionKey.m_sessionID &&
				x.TypeID == gameSessionKey.m_typeID);

			if (session == null)
			{
				QLog.WriteLine(1,
					$"Error : GameSessionService.SplitSession - no session with id={gameSessionKey.m_sessionID}");
				return Error(0);
			}

			var newHostPid = Context.Client.PlayerInfo.PID;

			session.HostPID = newHostPid;

			QLog.WriteLine(1,
				$"SplitSession - host migrated to PID={newHostPid} for session={session.Id}");

			return Result(gameSessionKey);
		}

		[RMCMethod(29)]
		public void SearchSocialSessions()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(30)]
		public void ReportUnsuccessfulJoinSessions()
		{
			UNIMPLEMENTED();
		}


	}
}
