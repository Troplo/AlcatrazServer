using System;

namespace QNetZ.Attributes
{
	/// <summary>
	/// RMC class attribute identifying class as a service/protocol hanlder
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class RMCServiceAttribute : Attribute
	{
		public readonly RMCProtocolId ProtocolId;
		public string Name { get; set; }

		public RMCServiceAttribute(RMCProtocolId protocolId)
		{
			ProtocolId = protocolId;
		}
	}
}