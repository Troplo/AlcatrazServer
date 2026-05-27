using System.Collections.Generic;

namespace DSFServices.DDL.Models.GameConfigService
{
    public class GetConfigV2Response
    {
        public Dictionary<string, uint> ConfigMap { get; set; } = new Dictionary<string, uint>();
        public uint ServerTime { get; set; }
        public uint PrincipalID { get; set; }
        public uint TitleID { get; set; }
        public string PlatformContext { get; set; } = "";
    }
}
