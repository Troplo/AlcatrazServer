using Alcatraz.DTO.Versioning;

namespace Alcatraz.DTO.Models.v20260526
{
    public class MarketplaceTokenResponse
    {
		[ApiVersionSince(20260701)]
        public bool enabled { get; set; }
		[ApiVersionSince(20260701)]
        public string token { get; set; }
        [ApiVersionSince(20260701)]
        public string baseUrl { get; set; }
		[ApiVersionSince(20260701)]
        public int expiresIn { get; set; }
    }
}