using QNetZ.DDL;
using System.Collections.Generic;

namespace DSFServices.DDL.Models
{
    public class ClientVersionInfo
    {
        public ushort m_v1 { get; set; }
        public ushort m_v2 { get; set; }
        public ushort m_v3 { get; set; }
        public uint m_v4 { get; set; }

        public object m_customVersion { get; set; }
    }
    
    public class RVConnectionData
    {
        public StationURL m_urlRegularProtocols { get; set; }

        public List<byte> m_lstSpecialProtocols { get; set; }

        public StationURL m_urlSpecialProtocols { get; set; }

        public uint nid { get; set; }
    }
}