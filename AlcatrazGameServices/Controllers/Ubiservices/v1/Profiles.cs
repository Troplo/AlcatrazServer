using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Alcatraz.Context;
using Alcatraz.GameServices.Services;
using RDVServices;

namespace Alcatraz.GameServices.Controllers.Ubiservices.V1
{
    [ApiController]
    [Route("v1/profiles")]
    public class ProfilesController(IUserService userService, MainDbContext dbContext) : ControllerBase
    {
        [HttpGet("{id}/club/rewards")]
        public IActionResult GetClubRewards(string id)
        {
            return Ok(new
            {
                rewards = Array.Empty<object>()
            });
        }

        [HttpGet("")]
        public async Task<IActionResult> GetProfile([FromQuery] string profileId)
        {
            var user = DBHelper.GetUserByGuid(profileId);
            return Ok(new
            {
                profiles = new[]
                {
                    new
                    {
                        profileId = profileId,
                        userId = profileId,
                        platformType = "uplay",
                        idOnPlatform = profileId,
                        nameOnPlatform = user.PlayerNickName
                    }
                }
            });
        }

        [HttpGet("{id}/rewards")]
        public IActionResult GetRewards(string id)
        {
            return Ok(new
            {
                rewards = new object[]
                {
                    new
                    {
                        id = "WDREWARD04",
                        profileId = "8d51ebd2-9f58-41bf-ae12-84926bf87c3f",
                        value = 0,
                        creationDate = "2012-10-26T18:04:54.273Z",
                        typeId = 2,
                        typeName = "Unlockable",
                        name = "Papavero Stealth Edition",
                        description = "Avoid detection and elude the cops in the Papavero Stealth Edition. ctOS won’t know what hit it.",
                        obj = (object)null,
                        platformShared = false,
                        isOwned = true,
                        quantityPurchased = 1,
                        rewardLocation = (object)null,
                        instruction = "You can access this Reward within the game.",
                        condition = "",
                        spaceId = "c8237ba1-f3a7-4a93-acb6-a23044c4f0cf",
                        images = new[]
                        {
                            new { type = "background", url = "/Games/WD/rewards/reward4.jpg" },
                            new { type = "iPhone", url = "/Games/WD/rewards/reward4_ip.png" },
                            new { type = "mobileThumbnail", url = "/Games/WD/rewards/reward4_mobile.jpg" },
                            new { type = "thumbnail", url = "/Games/WD/rewards/reward4.jpg" },
                            new { type = "thumbnailWebsite", url = "/Games/WD/rewards/reward4.jpg" }
                        },
                        tags = Array.Empty<object>(),
                        purchaseDate = "2026-03-29T09:18:12.8356544Z",
                        xp = 0,
                        consumableTypeId = (object)null,
                        consumableTypeName = (object)null,
                        startDate = (object)null,
                        endDate = (object)null,
                        daysLeft = (object)null,
                        timeFrameLimitInDays = (object)null,
                        quantityLimit = (object)null,
                        consumableConstraintId = (object)null,
                        consumableConstraintName = (object)null,
                        groups = Array.Empty<object>(),
                        quantityUsed = 0,
                        rarity = (object)null
                    },
                    new
                    {
                        id = "WDREWARD02",
                        profileId = "8d51ebd2-9f58-41bf-ae12-84926bf87c3f",
                        value = 0,
                        creationDate = "2013-08-20T17:53:44Z",
                        typeId = 2,
                        typeName = "Unlockable",
                        name = "Online Contract Cash Boost",
                        description = "Boost the cash payout from all online contracts to get an edge on your enemies.",
                        obj = (object)null,
                        platformShared = false,
                        isOwned = true,
                        quantityPurchased = 1,
                        rewardLocation = (object)null,
                        instruction = "You can access this Reward within the game.",
                        condition = "",
                        spaceId = "c8237ba1-f3a7-4a93-acb6-a23044c4f0cf",
                        images = new[]
                        {
                            new { type = "background", url = "/Games/WD/rewards/reward2.jpg" },
                            new { type = "iPhone", url = "/Games/WD/rewards/reward2_ip.png" },
                            new { type = "mobileThumbnail", url = "/Games/WD/rewards/reward2_mobile.jpg" },
                            new { type = "thumbnail", url = "/Games/WD/rewards/reward2.jpg" },
                            new { type = "thumbnailWebsite", url = "/Games/WD/rewards/reward2.jpg" }
                        },
                        tags = Array.Empty<object>(),
                        purchaseDate = "2026-03-29T09:18:12.8356544Z",
                        xp = 0,
                        consumableTypeId = (object)null,
                        consumableTypeName = (object)null,
                        startDate = (object)null,
                        endDate = (object)null,
                        daysLeft = (object)null,
                        timeFrameLimitInDays = (object)null,
                        quantityLimit = (object)null,
                        consumableConstraintId = (object)null,
                        consumableConstraintName = (object)null,
                        groups = Array.Empty<object>(),
                        quantityUsed = 0,
                        rarity = (object)null
                    },
                    new
                    {
                        id = "WDREWARD03",
                        profileId = "8d51ebd2-9f58-41bf-ae12-84926bf87c3f",
                        value = 0,
                        creationDate = "2013-08-20T17:53:44Z",
                        typeId = 2,
                        typeName = "Unlockable",
                        name = "Gold D50",
                        description = "Bringing a new meaning to ‘one-hit wonder’, this 14 karat D50 edition is a jaw-dropper.",
                        obj = (object)null,
                        platformShared = false,
                        isOwned = true,
                        quantityPurchased = 1,
                        rewardLocation = (object)null,
                        instruction = "You can access this Reward within the game.",
                        condition = "",
                        spaceId = "c8237ba1-f3a7-4a93-acb6-a23044c4f0cf",
                        images = new[]
                        {
                            new { type = "background", url = "/Games/WD/rewards/reward3.jpg" },
                            new { type = "iPhone", url = "/Games/WD/rewards/reward3_ip.png" },
                            new { type = "mobileThumbnail", url = "/Games/WD/rewards/reward3_mobile.jpg" },
                            new { type = "thumbnail", url = "/Games/WD/rewards/reward3.jpg" },
                            new { type = "thumbnailWebsite", url = "/Games/WD/rewards/reward3.jpg" }
                        },
                        tags = Array.Empty<object>(),
                        purchaseDate = "2026-03-29T09:18:12.8356544Z",
                        xp = 0,
                        consumableTypeId = (object)null,
                        consumableTypeName = (object)null,
                        startDate = (object)null,
                        endDate = (object)null,
                        daysLeft = (object)null,
                        timeFrameLimitInDays = (object)null,
                        quantityLimit = (object)null,
                        consumableConstraintId = (object)null,
                        consumableConstraintName = (object)null,
                        groups = Array.Empty<object>(),
                        quantityUsed = 0,
                        rarity = (object)null
                    },
                    new
                    {
                        id = "WDREWARD01PC",
                        profileId = "8d51ebd2-9f58-41bf-ae12-84926bf87c3f",
                        value = 0,
                        creationDate = "2013-08-20T17:53:44.007Z",
                        typeId = 1,
                        typeName = "Downloadable",
                        name = "WATCH_DOGS™ Desktop Wallpapers",
                        description = "Download Uplay-exclusive WATCH_DOGS™ desktop wallpapers for your PC.",
                        obj = (object)null,
                        platformShared = false,
                        isOwned = true,
                        quantityPurchased = 1,
                        rewardLocation = "s/Rewards/WDREWARD01PC/WDTheme1.ZIP",
                        instruction = "Go to https://ubisoftconnect.com to download this Reward and start using it.",
                        condition = "",
                        spaceId = "c8237ba1-f3a7-4a93-acb6-a23044c4f0cf",
                        images = new[]
                        {
                            new { type = "background", url = "/Games/WD/rewards/reward1.jpg" },
                            new { type = "iPhone", url = "/Games/WD/rewards/reward1_ip.png" },
                            new { type = "mobileThumbnail", url = "/Games/WD/rewards/reward1_mobile.jpg" },
                            new { type = "thumbnail", url = "/Games/WD/rewards/reward1.jpg" },
                            new { type = "thumbnailWebsite", url = "/Games/WD/rewards/reward1.jpg" }
                        },
                        tags = Array.Empty<object>(),
                        purchaseDate = "2026-03-29T09:18:12.8356544Z",
                        xp = 0,
                        consumableTypeId = (object)null,
                        consumableTypeName = (object)null,
                        startDate = (object)null,
                        endDate = (object)null,
                        daysLeft = (object)null,
                        timeFrameLimitInDays = (object)null,
                        quantityLimit = (object)null,
                        consumableConstraintId = (object)null,
                        consumableConstraintName = (object)null,
                        groups = Array.Empty<object>(),
                        quantityUsed = 0,
                        rarity = (object)null
                    }
                }
            });
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> CreateSession([FromHeader(Name = "Authorization")] string authorization)
        {
            try
            {
                if (string.IsNullOrEmpty(authorization) || !authorization.Contains("t="))
                {
                    return BadRequest();
                }

                string ticket = authorization.Split("t=")[1];
                
                var session = dbContext.SessionTokens
                    .FirstOrDefault(x => ticket == x.Id);
                
                if (session == null)
                {
                    return Unauthorized(new
                    {
                        error = "Invalid ticket"
                    });
                }

                var user =
                    userService.GetById(session.UserId);
                System.Net.IPAddress remoteIp = HttpContext.Connection.RemoteIpAddress;
                
                return Ok(new
                {
                    token = "/AaAaAaAaAaAaAaAaAaAaAa==",
                    ticket = ticket,
                    twoFactorAuthenticationTicket = (object)null,
                    expiration = DateTime.UtcNow.AddMilliseconds(900000000).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    platformType = "uplay",
                    profileId = user.Guid,
                    userId = user.Guid,
                    username = user.PlayerNickName,
                    nameOnPlatform = user.PlayerNickName,
                    initializeUser = true,
                    spaceId = "375124ab-c707-42b3-a229-826184e33d2a",
                    environment = "Prod",
                    hasAcceptedLegalOptins = true,
                    accountIssues = (object)null,
                    sessionId = ticket,
                    clientIp = remoteIp.MapToIPv4().ToString(),
                    clientIpCountry = "US",
                    serverTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    rememberMeTicket = (object)null
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }
        
        [HttpPost("{id}/events")]
        public IActionResult IngestEvents()
        {
            return Ok(new { success = true });
        }
    }
}