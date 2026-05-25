using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
    [RMCService(RMCProtocolId.NexusOnly, Name = "Tracking2Protocol")]
    public class Tracking2Service : RMCServiceBase
    {
        [RMCMethod(1, "GetStartupStats_V1")]
        public RMCResult GetStartupStats()
        { 
            return Result(new { result = true });
        }
    }
}