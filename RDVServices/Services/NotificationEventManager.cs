using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace RDVServices.Services
{
	[RMCService(RMCProtocolId.NotificationEventManager, Name = "NotificationProtocol")]
	public class NotificationEventManager : RMCServiceBase
	{
		[RMCMethod(1, Name = "ProcessNotificationEvent_V1")]
		public void Notify(NotificationEvent notification)
		{
			// Dummy event

		}
	}
}
