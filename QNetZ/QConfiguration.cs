using System.Linq;
using System.Text;

namespace QNetZ
{
	public class UbiservicesMicroservice
	{
		public string Token { get; set; }
	}

	public enum Environment
	{
		Prod,
		Dev,
		Staging,
		Test
	}

	public class MaintenanceConfig
	{
		public string title { get; set; }
		public string message { get; set; }
		public bool enabled { get; set; }
	}
	
	public class LegalPolicies
	{
		public string Terms { get; set; }
		public string Privacy { get; set; }
	}
	
	public class QConfiguration
	{
		public string ServerBindAddress { get; set; }
		public ushort RDVServerPort { get; set; }
		public ushort BackendServiceServerPort { get; set; }
		public string ServerFilesPath { get; set; }
		public string ServiceURLHostName { get; set; }          // Affects Alcatraz.json file downloaded from server in GetAlcatrazConfig
		public string SandboxAccessKey { get; set; }            // Server access key. Affects packet checksum;
		public string OnlineConfigKey { get; set; }
		public string DbConnectionString { get; set; }
		public int DbType { get; set; }
		public int LogLevel { get; set; }						// not used in debug server. See Services/RendezVousServer.cs
		public UbiservicesMicroservice Ubiservices { get; set; }
		public LegalPolicies LegalPolicies { get; set; }
		public Environment Environment { get; set; }
		public MaintenanceConfig MaintenanceConfig { get; set; }
		public string ServerBaseUrl { get; set; }
		public string RdvConnectionUrl { get; set; }
		public bool AllowRegistrations { get; set; }
		public byte SandboxAccessKeyCheckSum
		{
			get
			{
				if (SandboxAccessKey == null)
					return 0;

				return (byte)Encoding.ASCII.GetBytes(SandboxAccessKey).Sum(b => b);
			}
		}

		public uint SandboxAccessKeyWordSum
		{
			get
			{
				if (SandboxAccessKey == null)
					return 0;

				var bytes = Encoding.ASCII.GetBytes(SandboxAccessKey);
				uint sum = 0;
				int wordCount = (bytes.Length + 3) / 4;
				for (int i = 0; i < wordCount; i++)
				{
					uint word = 0;
					for (int b = 0; b < 4; b++)
					{
						int idx = i * 4 + b;
						if (idx < bytes.Length)
							word |= (uint)bytes[idx] << (b * 8);
					}
					sum += word;
				}
				return sum;
			}
		}


		// -------------------------------------------------------- static shit
		static QConfiguration _instance;
		public static QConfiguration Instance {
			get {
				return _instance;
			}
			set {
				_instance = value;
			}
		}

		public static QConfiguration MakeDevelopmentConfiguration(string serverBindAddress)
		{
			var cfg = new QConfiguration();

			cfg.ServerBindAddress = serverBindAddress ?? "127.0.0.1";
			cfg.RDVServerPort = 21001; // for Watch Dogs
			//cfg.RDVServerPort = 21005;
			cfg.BackendServiceServerPort = 21006;

			cfg.ServerFilesPath = "../../../../data/serverFiles/";

			cfg.SandboxAccessKey = "p4BkcPBh"; // for WD1
          //cfg.SandboxAccessKey = "w6kAtr3T";            // Server access key. Affects packet checksum;

            cfg.DbType = 0;
			cfg.DbConnectionString = "Data Source=database.sqlite";

			return cfg;
		}
	}
}
