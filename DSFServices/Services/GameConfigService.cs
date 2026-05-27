using System.Collections.Generic;
using DSFServices.DDL.Models.GameConfigService;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
    [RMCService(RMCProtocolId.NexusOnly, Name = "GameConfigProtocol")]
    public class GameConfigService : RMCServiceBase
    {
        [RMCMethod(0, Name = "GetConfig_V2")]
        public RMCResult GetConfigV2()
        {
            var response = new GetConfigV2Response();
            response.ConfigMap = new Dictionary<string, uint>();
            
            response.ServerTime = (uint)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            response.PrincipalID = Context.Client.PlayerInfo != null ? Context.Client.PlayerInfo.PID : 4154;
            response.TitleID = 564276;
            response.PlatformContext = "WDOGS_PC_LNCH";
            QLog.WriteLine(1, $"[GameConfigService] GetConfig_V2: ServerTime={response.ServerTime}, PrincipalID={response.PrincipalID}, TitleID={response.TitleID}, PlatformContext={response.PlatformContext}");

            return Result(response);
        }
    }
}