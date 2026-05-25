using System.Collections.Generic;
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
            return Result(new byte[] { 0x00, 0x00, 0x00, 0x00 });
        }
    }
}