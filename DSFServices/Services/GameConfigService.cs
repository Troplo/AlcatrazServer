using System.Collections.Generic;
using DSFServices.DDL.Models.GameConfigService;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace DSFServices.Services
{
    [RMCService(RMCProtocolId.NexusOnly, Name = "GameConfigProtocol")]
    public class GameConfigService : RMCServiceBase
    {
        [RMCMethod(0, Name = "GetConfig_V2")]
        public RMCResult GetConfigV2()
        {
            var response = new GetConfigV2Response();
            response.ConfigMap = new Dictionary<string, uint>
            {
                { "Protocol", 786688u },
                { "VersionOverride.sub-cg-jpn-package-122.MASTER : sub-cg-jpn-package-122", 8u },
                { "VersionOverride.watchdogs-cg-36.MASTER : watchdogs-cg-36", 4u },
                { "RemoteLog.Level.Party", 2u },
                { "Matchmaking.PartyPolling.UpdateCooldown", 60u },
                { "PortalAvailability.IsCloseToRailwayMaxSearchDistance", 5u },
                { "Companion.MinPercentGatesPassedToIgnoreDisconnect", 50u },
                { "PlayerProgression.NotorietyDecryptionTeamWin", 250u },
                { "Matchmaking.Timeout.Search.Race", 300u },
                { "PortalAvailability.FirewallSlowDecreasePercentage", 20u },
                { "MPModeKillSwitch.IsDecryptionDisabled", 0u },
                { "PortalRace.SelectionTime", 5u },
                { "Companion.QuickMatchConnectionTimeout", 30u },
                { "Matchmaking.Timeout.Search.Hacking", 300u },
                { "RemoteLog.Level.GameConfig", 4u },
                { "PortalsMissions.RandomEvents.EnableAdvancedTracking", 1u },
                { "PlayerProgression.NotorietyTailingInvaderPotentialGain", 250u },
                { "PDAGridApplication.PortalOpportunityRequestThrottleDelay", 30u },
                { "StormCore.NetworkUpdateMaxInterval", 200u },
                { "RemoteLog.Level.HostMigration", 4u },
                { "Matchmaking.Timeout.Search.FreeRoaming", 300u },
                { "Tracking.Level", 2u },
                { "PlayerProgression.NotorietyCompanionCheckpointCross", 100u },
                { "VersionOverride.1stsub-cg-package-1014.MASTER : 1stsub-cg-package-1014", 4u },
                { "VersionOverride.patchdayoneubi-22.MASTER : patchdayoneubi-22", 8u },
                { "PortalsMissions.BountyDuration", 900u },
                { "RemoteLog.Level.PortalsMissions", 4u },
                { "VersionOverride.dlc-package-968.MASTER : dlc-package-968", 10u },
                { "VersionOverride.dlc-jpn-package-31.MASTER : dlc-jpn-package-31", 10u },
                { "RemoteLog.Level.RendezVous", 4u },
                { "VersionOverride.dlc-mena-package-8.MASTER : dlc-mena-package-38", 10u },
                { "StormCore.NetworkUpdateMinInterval", 15u },
                { "NexusWalla.EcoMaxClaimGifts", 10u },
                { "RemoteLog.Level.UGC", 4u },
                { "PortalAvailability.MaxOpportunitiesBatchSize", 10u },
                { "VersionOverride.dlc-cg-jpn-package-28.MASTER : dlc-cg-jpn-package-28", 10u },
                { "VersionOverride.dlc-package-798.MASTER : dlc-package-798", 10u },
                { "PortalAvailability.MinTimeBetweenContracts", 180u },
                { "VersionOverride.sub-cg-jpn-package-127.MASTER : sub-cg-jpn-package-127", 8u },
                { "Matchmaking.Timeout.Search.Tailing", 300u },
                { "VersionOverride.dlc-mena-package-39.MASTER : dlc-mena-package-39", 10u },
                { "RespawnService.MPInitTeleportAlgoDelayDelta", 1000u },
                { "VersionOverride.dlc-cg-jpn-package-32.MASTER : dlc-cg-jpn-package-32", 10u },
                { "DrinkingGame.Friction", 120u },
                { "DrinkingGame.ForceMin", 17u },
                { "VersionOverride.dlc-cg-jpn-package-31.MASTER : dlc-cg-jpn-package-31", 10u },
                { "VersionOverride.patchdayoneubi-21.MASTER : patchdayoneubi-21", 8u },
                { "VersionOverride.dlc-package-970.MASTER : dlc-package-970", 10u },
                { "PortalRace.MaxCountDownLength", 60u },
                { "VersionOverride.watchdogs-cg-46.MASTER : watchdogs-cg-46", 4u },
                { "DrinkingGame.ForceMax", 29u },
                { "Storm.StartupParams.pingNbQosProbes", 1u },
                { "PortalAvailability.OptionsMenuOpenDuration", 10u },
                { "PortalRace.VoteTime", 45u },
                { "VersionOverride.sub-cg-jpn-package-123.MASTER : sub-cg-jpn-package-123", 8u },
                { "GroupContractDirector.QuitToMatchMakingTime", 300u },
                { "NexusWalla.EcoMaxRetryCount", 3u },
                { "PlayerProgression.NotorietyHackingVictimPotentialGain", 250u },
                { "GroupContractDirector.Races_MatchMakingUpdateTime", 60u },
                { "VoiceChat.MaxPeerCount", 7u },
                { "PortalAvailability.AbandonPenaltyDuration", 300000u },
                { "VersionOverride.dlc-cg-mena-package-26.MASTER : dlc-cg-mena-package-26", 10u },
                { "PortalRace.MinPlayerCountForStart", 2u },
                { "PortalAvailability.MinTimeBetweenInvasionAttempts", 300u },
                { "DrinkingGame.ReticleCenteringSpeedFactor", 200u },
                { "NexusWalla.EcoMaxLootDrops", 10u },
                { "RemoteLog.Level.Session", 4u },
                { "VersionOverride.sub-cg-jpn-package-145.MASTER : sub-cg-jpn-package-145", 9u },
                { "GroupContractDirector.DecryptionCombatIdleDuration", 180u },
                { "VersionOverride.dlc-package-972.MASTER : dlc-package-972", 10u },
                { "VersionOverride.dlc-cg-jpn-package-27.MASTER : dlc-cg-jpn-package-27", 10u },
                { "VersionOverride.dlc-mena-package-37.MASTER : dlc-mena-package-37", 10u },
                { "VersionOverride.dlc-mena-package-34.MASTER : dlc-mena-package-34", 10u },
                { "Matchmaking.Timeout.Search.DecryptionCombat", 300u },
                { "PortalAvailability.CancelMatchMakingCooldownDuration", 300u },
                { "VersionOverride.dlc-cg-jpn-package-30.MASTER : dlc-cg-jpn-package-30", 10u },
                { "VersionOverride.dlc-mena-package-35.MASTER : dlc-mena-package-35", 10u },
                { "VersionOverride.dlc-package-969.MASTER : dlc-package-969", 10u },
                { "RemoteLog.Level.Matchmaking", 4u },
                { "PortalAvailability.IgnoreFirewallCooldown", 0u },
                { "VersionOverride.sub-cg-jpn-package-126.MASTER : sub-cg-jpn-package-126", 8u },
                { "RemoteLog.Level.Localization", 4u },
                { "VersionOverride.chs-7.MASTER : chs-7", 8u },
                { "VersionOverride.sub-cg-jpn-package-125.MASTER : sub-cg-jpn-package-125", 8u },
                { "VersionOverride.chs-6.MASTER : chs-6", 8u },
                { "VersionOverride.dlc-cg-jpn-package-25.MASTER : dlc-cg-jpn-package-25", 10u },
                { "PortalAvailability.PDAMapAppOpenDuration", 0u },
                // { "PlayerProgression.NotorietyRaceAheadOf", 20u },
                // { "PlayerProgression.NotorietyRaceBehind", 10u },
                // { "PlayerProgression.NotorietyDecryptionTeamLoss", 5u },
                // { "PlayerProgression.NotorietyDecryptionPlayerHigherXP", 10u },
                // { "PlayerProgression.NotorietyDecryptionPlayerLowerXP", 2u },
                // { "PlayerProgression.NotorietyCompanionPenalty", 5u },
            };
            
            response.ServerTime = (uint)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            response.PrincipalID = Context.Client.PlayerInfo != null ? Context.Client.PlayerInfo.PID : 4154;
            response.TitleID = 564276;
            response.PlatformContext = "WDOGS_PC_LNCH";
            QLog.WriteLine(1, $"[GameConfigService] GetConfig_V2: ServerTime={response.ServerTime}, PrincipalID={response.PrincipalID}, TitleID={response.TitleID}, PlatformContext={response.PlatformContext}");

            return Result(response);
        }
    }
}