
using System.Collections.Generic;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
    [RMCService(RMCProtocolId.NexusOnly, Name = "LeaderboardsProtocol")]
    public class LeaderboardsService : RMCServiceBase
    {
        [RMCMethod(0, Name = "GetLeaderboardOverviewWithEstimatedUserPositionAndDefaultSorting_V2")]
        public RMCResult GetLeaderboardOverviewWithEstimatedUserPositionAndDefaultSorting_V2()
        {
            return Result(new {result = true});
        }   
    }
}