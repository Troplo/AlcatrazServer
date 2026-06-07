using DSFServices.DDL.Models.Leaderboard;
using System.Collections.Generic;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using QNetZ.DDL;

namespace DSFServices.Services
{
    [RMCService(RMCProtocolId.NexusOnly, Name = "LeaderboardsProtocol")]
    public class LeaderboardsService : RMCServiceBase
    {
        [RMCMethod(0, Name = "GetLeaderboardOverviewWithEstimatedUserPositionAndDefaultSorting_V2")]
        public RMCResult GetLeaderboardOverviewWithEstimatedUserPositionAndDefaultSorting_V2(
            string statName,
            uint unk1,
            uint unk2,
            List<qUUID> userIds)
        {
            QLog.WriteLine(1, $"Received GetLeaderboardOverviewWithEstimatedUserPositionAndDefaultSorting_V2 request for stat '{statName}' with {userIds.Count} user IDs.");
            var result = new LeaderboardOverviewResult
            {
                TopEntries = new List<LeaderboardEntry>(),
                EstimatedUserPosition = new LeaderboardEntry { Rank = 0, PID = new qUUID(), Score = 0 },
                UserPositions = new List<LeaderboardEntry>(),
                TotalPlayers = 0
            };

            // For now, return mock data for the requested users so the client doesn't freeze or crash.
            uint dummyRank = 1000;
            foreach (var id in userIds)
            {
                result.UserPositions.Add(new LeaderboardEntry
                {
                    Rank = dummyRank++,
                    PID = id,
                    Score = 1500
                });
            }

            return Result(result);
        }

        [RMCMethod(1, Name = "ScoringSessionStart_V1")]
        public RMCResult ScoringSessionStart_V1(string sessionId, List<qUUID> participants)
        {
            QLog.WriteLine(1, $"[Leaderboards] ScoringSessionStart_V1 called for session '{sessionId}' with {participants.Count} participants.");
            return Error(0);
        }

        [RMCMethod(2, Name = "ScoringSessionFinish_V1")]
        public RMCResult ScoringSessionFinish_V1(PlayerSessionInformation sessionInformation)
        {
            if (sessionInformation != null)
            {
                QLog.WriteLine(1, $"[Leaderboards] Session '{sessionInformation.SessionId}' finished. Recorded {sessionInformation.Scores?.Count ?? 0} scores.");
            }

            return Error(0);
        }
    }
}