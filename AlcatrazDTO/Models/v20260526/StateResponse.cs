namespace Alcatraz.DTO.Models.v20260526
{
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
    
    public class StateResponse
    {
        public bool allowRegistrations { get; set; }
        public string websiteBaseUrl { get; set; }
        public Environment environment { get; set; }
        public MaintenanceConfig maintenanceConfig;
    }
}