using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.LocalizationService, Name = "LocalizationProtocol")]
	public class LocalizationService : RMCServiceBase
	{
		[RMCMethod(1, "GetLocaleCode_V1")]
		public RMCResult GetLocaleCode()
		{
			return Result("en-US");
		}

		[RMCMethod(2, "SetLocaleCode_V1")]
		public RMCResult SetLocaleCode(string localeCode)
		{
			return Error(0);
		}
	}
}
