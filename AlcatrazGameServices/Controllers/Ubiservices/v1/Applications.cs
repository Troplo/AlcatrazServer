using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QNetZ;

namespace Alcatraz.GameServices.Controllers.Ubiservices.V1
{
    [ApiController]
    [Route("v{version}/applications")]
    public class ApplicationsController : ControllerBase
    {
        [HttpGet("{id}/parameters")]
        public IActionResult GetApplicationParameters(string id)
        {
            return Ok(new
            {
                parameters = new Dictionary<string, object>
                {
                    {
                        "us-staging", new
                        {
                            fields = new { spaceId = "c619c78d-1c6b-47cf-9b36-80160d5c0e8f" },
                            relatedPopulation = (object)null
                        }
                    },
                    {
                        "us-sdkClientClub", new
                        {
                            fields = new
                            {
                                ps4ClubWebsite = "https://static8.cdn.ubi.com/u/sites/uplay/index.html",
                                xoneLaunchPath = "Ms-xbl-55C5EE27://default?url=/games/{deepLink}&context={context}&titleid={gameTitleId}&env={clubEnvName}&genomeid={applicationId}&actioncompleted={actionCompletedList}&debug={debug}&spaceId={{spaceId}}&applicationId={applicationId}",
                                dynamicPanelUrl = "https://{env}public-ubiservices.ubi.com/v1/profiles/{{profileId}}/club/dynamicPanel/MyProfile.png?spaceId={{spaceId}}&noRedirect=true",
                                psApiClubWebsite = "https://static8.cdn.ubi.com/u/sites/uplay/index.html",
                                ps4PsnoLaunchPath = "psno://localhost/?response_type=code&client_id=171ca84a-371e-412b-a807-6531f3f1e454&scope=psn%3As2s&redirect_uri={ps4ClubWebsiteUrlEncoded}&state={ps4ClubWebsiteArgumentsPsnoEncoded}",
                                psApiPsnoLaunchPath = "psno://localhost/?response_type=code&client_id=171ca84a-371e-412b-a807-6531f3f1e454&scope=psn%3As2s&redirect_uri={psApiClubWebsiteUrlEncoded}&state={psApiClubWebsiteArgumentsPsnoEncoded}",
                                stadiaClubWebsiteUrl = "https://static8.cdn.ubi.com/u/sites/UbisoftClub/Stadia/index.html?url=/games/{deepLink}&spaceId={{spaceId}}&applicationId={applicationId}&profileId={{profileId}}&env={env}&displayMode={displayMode}&locale={locale}&actioncompleted={actionCompletedList}&jotunVersion={jotunVersion}&jotunBinaryVersion={jotunBinaryVersion}&ticket={ticket}",
                                switchClubWebsiteUrl = "https://static8.cdn.ubi.com/u/sites/UbisoftClub/NintendoSwitch/index.html?url=/games/{deepLink}&ussdkversion={usSdkVersionName}&env={clubEnvName}&actioncompleted={actionCompletedList}&forceLang={forceLang}&profileId={{profileId}}&ticket={ticket}&context={context}&debug={debug}&spaceId={{spaceId}}&applicationId={applicationId}",
                                ps4ClubWebsiteArguments = "url=/games/{deepLink}&spaceId={{spaceId}}&applicationId={applicationId}&context={context}&env={clubEnvName}&genomeid={applicationId}&actioncompleted={actionCompletedList}&ussdkversion={usSdkVersionName}&debug={debug}{geometry}&ticket={ticket}&profileid={{profileId}}",
                                psApiClubWebsiteArguments = "url=/games/{deepLink}&spaceId={{spaceId}}&applicationId={applicationId}&context={context}&env={clubEnvName}&genomeid={applicationId}&actioncompleted={actionCompletedList}&ussdkversion={usSdkVersionName}&debug={debug}{geometry}&ticket={ticket}&profileId={{profileId}}&psEnterButton={psEnterButton}"
                            },
                            relatedPopulation = (object)null
                        }
                    },
                    {
                        "us-sdkClientUrlsPlaceholders", new
                        {
                            fields = new
                            {
                                baseurl_ws = new
                                {
                                    China = "wss://public-ws-ubiservices.ubisoft.cn/",
                                    Standard = "wss://{env}public-ws-ubiservices.ubi.com",
                                    China_GAAP = "wss://gaap.ubiservices.ubi.com:16000"
                                },
                                baseurl_aws = new
                                {
                                    China = "https://public-ubiservices.ubisoft.cn",
                                    Standard = "https://{env}public-ubiservices.ubi.com",
                                    China_GAAP = "https://gaap.ubiservices.ubi.com:12000"
                                },
                                baseurl_msr = new
                                {
                                    China = "https://public-ubiservices.ubisoft.cn",
                                    Standard = "https://msr-{env}public-ubiservices.ubi.com",
                                    China_GAAP = "https://gaap.ubiservices.ubi.com:12000"
                                }
                            },
                            relatedPopulation = (object)null
                        }
                    },
                    {
                        "us-sdkClientLogin", new
                        {
                            fields = new
                            {
                                populationHttpRequestOptimizationEnabled = true,
                                populationHttpRequestOptimizationRetryCount = 0,
                                populationHttpRequestOptimizationRetryTimeoutIntervalMsec = 5000,
                                populationHttpRequestOptimizationRetryTimeoutIncrementMsec = 1000
                            },
                            relatedPopulation = (object)null
                        }
                    },
                    {
                        "us-sdkClientFeaturesSwitches", new
                        {
                            fields = new
                            {
                                populationsAutomaticUpdate = true,
                                forcePrimarySecondaryStoreSyncOnReturnFromBackground = true
                            },
                            relatedPopulation = (object)null
                        }
                    },
                    {
                        "us-sdkClientChina", new
                        {
                            fields = new
                            {
                                websocketHost = "public-ws-ubiservices.ubi.com",
                                tLogApplicationId = "",
                                tLogApplicationKey = "",
                                tLogApplicationName = ""
                            },
                            relatedPopulation = (object)null
                        }
                    },
                    {
                        "us-sdkClientUrls", new
                        {
                            fields = new
                            {
                                populations = "{baseurl_aws}/{{version}}/profiles/me/populations",
                                profilesToken = "{baseurl_aws}/{{version}}/profiles/{{profileId}}/tokens/{token}"
                            },
                            relatedPopulation = (object)null
                        }
                    }
                }
            });
        }

        [HttpGet("{id}/configuration")]
        public IActionResult GetApplicationConfiguration(string id)
        {
            return Ok(new
            {
                configuration = new
                {
                    custom = new { resources = Array.Empty<object>(), featuresSwitches = Array.Empty<object>() },
                    featuresSwitches = new[]
                    {
                        new { name = "Connection", value = true },
                        new { name = "ContentFiltering", value = true },
                        new { name = "Entities", value = true },
                        new { name = "Event", value = true },
                        new { name = "Everything", value = true },
                        new { name = "ExtendSession", value = true },
                        new { name = "FriendsLookup", value = true },
                        new { name = "FriendsProposal", value = true },
                        new { name = "FriendsRequest", value = true },
                        new { name = "FriendsSuggestions", value = true },
                        new { name = "KeepAlive", value = true },
                        new { name = "Localization", value = true },
                        new { name = "Messaging", value = true },
                        new { name = "Presence", value = true },
                        new { name = "Profiles", value = true },
                        new { name = "SocialfeedRead", value = true },
                        new { name = "SocialfeedWrite", value = true },
                        new { name = "Uplay", value = true },
                        new { name = "UplayFriends", value = true },
                        new { name = "UplayPassport", value = true },
                        new { name = "UplayWin", value = true },
                        new { name = "UseNonBlockingHttpRequestsPS4", value = true },
                        new { name = "Users", value = true }
                    },
                    gatewayResources = new[]
                    {
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/entities", name = "all_profiles/entities", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/spaces/entities", name = "all_spaces/entities", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/certificates", name = "certificates", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/connections", name = "connections", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/events", name = "events", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/me/friends", name = "friends", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles", name = "profiles", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/actions", name = "profiles/actions", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/entities", name = "profiles/entities", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/rewards", name = "profiles/rewards", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/spaces/{{spaceId}}/actions", name = "spaces/actions", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/spaces/{{spaceId}}/entities", name = "spaces/entities", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/spaces/{{spaceId}}/rewards", name = "spaces/rewards", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/wall", name = "wall", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/wall/{{postId}}/comments", name = "wall/comments", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/{{profileId}}/wall/{{postId}}/likes", name = "wall/likes", version = 1 },
                        new { url = $"wss://{QConfiguration.Instance.ServerBaseUrl}", name = "websocket/server", version = 1 },
                        new { url = $"https://{QConfiguration.Instance.ServerBaseUrl}/{{version}}/profiles/connections", name = "all_connections", version = 1 }
                    },
                    storm = new
                    {
                        detectUrl1 = "lb-ne1z-prod-mpe-detect01.ubisoft.com:11000",
                        detectUrl2 = "lb-ne1z-prod-mpe-detect02.ubisoft.com:11000",
                        traversalUrl = "lb-ne1z-prod-mpe-traversal.ubisoft.com:11005",
                        detectUrl1_v2 = "lb-ne1z-prod-mpe-detect01.ubisoft.com:11000",
                        detectUrl2_v2 = "lb-ne1z-prod-mpe-detect02.ubisoft.com:11000",
                        traversalUrl_v2 = "lb-ne1z-prod-mpe-traversal.ubisoft.com:11005"
                    },
                    sdkConfig = new
                    {
                        httpSafetySleepTime = 0,
                        keepAliveTimeoutMin = 10,
                        timeoutSec = 30,
                        httpParam = new { timeoutParam = new { initialDelayMsec = 30000 } },
                        websocketParam = new { timeoutParam = new { initialDelayMsec = 30000 } }
                    },
                    clubServices = new[]
                    {
                        new { url = "https://secure.ubi.com/UplayServices/UplayFacade/DownloadServicesRESTXML.svc/REST/XML/?url", name = "DownloadServiceUrl" },
                        new { url = "http://static8.ubi.com/u/Uplay/", name = "DynContentBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/", name = "DynContentSecureBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/u/Uplay/Packages/linkapp/1.1/", name = "LinkappBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/u/Uplay/", name = "MovieBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/u/Uplay/Packages/1.5-Share-rc/", name = "PackageBaseUrl" },
                        new { url = "https://{env}uplay.ubi.com/UplayServices/UplayFacade/ProfileServicesFacadeRESTXML.svc/REST/", name = "WebServiceBaseUrl" }
                    },
                    platformConfig = new
                    {
                        uplayGameCode = "WD",
                        applicationId = "9c4a1757-422b-458f-b4d2-5e623c911ba6",
                        spaceId = "c8237ba1-f3a7-4a93-acb6-a23044c4f0cf",
                        platform = "PC",
                        environment = "prod"
                    },
                    legacyUrls = new[]
                    {
                        new { url = "https://{env}friendsservice.ubi.com/friendsservice.svc/REST/", name = "friends" },
                        new { url = "https://{env}signupservice.ubi.com/PublicSignupService.svc/REST/", name = "signup" },
                        new { url = "https://{env}wspuplay-ext.ubi.com/UplayServices/ShareServices/GameClientServices.svc/REST/JSON/", name = "uplayShare" },
                        new { url = "https://{env}wspuplay-ext.ubi.com/UplayServices/WinServices/GameClientServices.svc/REST/JSON/", name = "uplayWin" },
                        new { url = "https://{env}wspuplay.ubi.com/UplayServices/ShareServices/ClientServices.svc/REST/JSON/", name = "uplayWinPlural" }
                    },
                    sandboxes = new[]
                    {
                        new { name = "WDOGS_PC_LNCH_A_DLC", friendlyName = "main_dlc", url = QConfiguration.Instance.RdvConnectionUrl },
                        new { name = "WDOGS_PC_LNCH_A_MASTER", friendlyName = "watch_dogs_final_1", url = QConfiguration.Instance.RdvConnectionUrl },
                        new { name = "WDOGS_PC_LNCH_A_PATCH_1", friendlyName = "watch_dogs_final_2", url = QConfiguration.Instance.RdvConnectionUrl },
                        new { name = "wdogs_pc_lnch_a_ws", friendlyName = "wdogs_pc_lnch_a_ws", url = QConfiguration.Instance.RdvConnectionUrl },
                        new { name = "ws_wdogs_companion_lnch_a", friendlyName = "ws_wdogs_comp_master", url = "https://127.0.0.1/WDOGS_COMPANION_LNCH_A" },
                        new { name = "ws_wdogs_pc_lnch_a_keys", friendlyName = "keys_ws", url = "https://127.0.0.1/WDOGS_PC_LNCH_A" },
                        new { name = "WDOGS_PC_LNCH_A", friendlyName = "WDOGS_PC_LNCH_A", url = QConfiguration.Instance.RdvConnectionUrl }
                    },
                    events = new
                    {
                        queues = new[]
                        {
                            new { name = "A", sendPeriodMilliseconds = 30000 },
                            new { name = "B", sendPeriodMilliseconds = 30000 },
                            new { name = "C", sendPeriodMilliseconds = 30000 }
                        },
                        tags = new[]
                        {
                            new { name = "achievement.unlock", value = true },
                            new { name = "context.start", value = true },
                            new { name = "context.stop", value = true },
                            new { name = "game.localization", value = true },
                            new { name = "game.progression", value = true },
                            new { name = "game.stats", value = true },
                            new { name = "NextMatch_Vote", value = true },
                            new { name = "NextMatch_VoteResult", value = true },
                            new { name = "player.achievement", value = true },
                            new { name = "player.dlc", value = true },
                            new { name = "player.progression", value = true },
                            new { name = "Stat_Combat_Kill_Enemies", value = true },
                            new { name = "Stat_Combat_Kill_EnemyAI", value = true },
                            new { name = "Stat_Combat_Kill_EnemyPlayer", value = true },
                            new { name = "Stat_CompanionVersus_Notoriety", value = true },
                            new { name = "Stat_Driving_Job_CountDown", value = true },
                            new { name = "Stat_Driving_Job_Damage", value = true },
                            new { name = "Stat_Driving_Job_Time", value = true },
                            new { name = "Stat_Invasion_HackingTutorialDone", value = true },
                            new { name = "Stat_Invasion_OptionEnabled", value = true },
                            new { name = "Stat_Minigame_CashRun_Time", value = true },
                            new { name = "Stat_Minigame_NVZN_Highscore", value = true },
                            new { name = "Stat_Minigame_NVZN_TotalScore", value = true },
                            new { name = "Stat_Minigame_NVZN_WaveReached", value = true },
                            new { name = "Stat_Minigame_Poker_TotalBenefits", value = true },
                            new { name = "Stat_Pills_Alone_HighScore", value = true },
                            new { name = "Stat_Pills_Bouncing_HighScore", value = true },
                            new { name = "Stat_Pills_Madness_HighScore", value = true },
                            new { name = "Stat_Pills_People_HighScore", value = true },
                            new { name = "Stat_Pills_Spider_HighScore", value = true },
                            new { name = "Stat_Player_Grid_Race_Time", value = true },
                            new { name = "Stat_Player_Hacking_InvasionCooldown", value = true },
                            new { name = "Stat_Player_Nexuswalla_Badge_Progress", value = true },
                            new { name = "Stat_Player_Nexuswalla_Mayor_Generator", value = true },
                            new { name = "Stat_Player_Nexuswalla_TotalCheckins", value = true },
                            new { name = "Stat_Player_Notoriety", value = true },
                            new { name = "Stat_Player_Notoriety_Decryption", value = true },
                            new { name = "Stat_Player_Notoriety_Hacking", value = true },
                            new { name = "Stat_Player_Notoriety_Races", value = true },
                            new { name = "Stat_Player_Notoriety_Tailing", value = true },
                            new { name = "Stat_Player_UGC_Time", value = true },
                            new { name = "Stat_User_PC_Specs", value = true },
                            new { name = "Tag_AI_Hacking", value = true },
                            new { name = "Tag_AI_Hacking_Profiled", value = true },
                            new { name = "Tag_DenunciatorOnAiGunshot", value = true },
                            new { name = "Tag_DTripComplete", value = true },
                            new { name = "Tag_DtripPerkAction", value = true },
                            new { name = "Tag_DTripStop", value = true },
                            new { name = "Tag_FelonyStop", value = true },
                            new { name = "Tag_GWar_Stop", value = true },
                            new { name = "Tag_Invasion_IncompatibleContentAttempt", value = true },
                            new { name = "Tag_InvasionSetting_Switch", value = true },
                            new { name = "Tag_MoneyEarn", value = true },
                            new { name = "Tag_MoneySpend", value = true },
                            new { name = "Tag_PlayerCraft", value = true },
                            new { name = "Tag_PlayerDeath", value = true },
                            new { name = "Tag_PlayerKill", value = true },
                            new { name = "Tag_PlayerReward", value = true },
                            new { name = "Tag_PlayerTool", value = true },
                            new { name = "Tag_QoS", value = true },
                            new { name = "Tag_RandomEventInteracted", value = true },
                            new { name = "Tag_SkillpointEarn", value = true },
                            new { name = "Tag_SkillpointSpend", value = true },
                            new { name = "Tag_UplayAccountStatus", value = true },
                            new { name = "Tag_UplayMenuAction", value = true },
                            new { name = "Tag_WeaponSwitch", value = true },
                            new { name = "Tag_XpEarn", value = true },
                            new { name = "XDPEvent", value = true }
                        }
                    },
                    punch = new
                    {
                        detectUrl1 = "lb-ne1z-prod-mpe-detect01.ubisoft.com:11000",
                        detectUrl2 = "lb-ne1z-prod-mpe-detect02.ubisoft.com:11000",
                        traversalUrl = "lb-ne1z-prod-mpe-traversal.ubisoft.com:11005",
                        detectUrl1_v2 = "lb-ne1z-prod-mpe-detect01.ubisoft.com:11000",
                        detectUrl2_v2 = "lb-ne1z-prod-mpe-detect02.ubisoft.com:11000",
                        traversalUrl_v2 = "lb-ne1z-prod-mpe-traversal.ubisoft.com:11005"
                    },
                    uplayServices = new[]
                    {
                        new { url = "https://secure.ubi.com/UplayServices/UplayFacade/DownloadServicesRESTXML.svc/REST/XML/?url", name = "DownloadServiceUrl" },
                        new { url = "http://static8.ubi.com/u/Uplay/", name = "DynContentBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/", name = "DynContentSecureBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/u/Uplay/Packages/linkapp/1.1/", name = "LinkappBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/u/Uplay/", name = "MovieBaseUrl" },
                        new { url = "http://static8.ubi.com/private/{env}/u/Uplay/Packages/1.5-Share-rc/", name = "PackageBaseUrl" },
                        new { url = "https://{env}uplay.ubi.com/UplayServices/UplayFacade/ProfileServicesFacadeRESTXML.svc/REST/", name = "WebServiceBaseUrl" }
                    }
                }
            });
        }
    }
}