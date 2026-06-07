using System.Collections.Generic;
using QNetZ.DDL;

namespace DSFServices.DDL.Models.Leaderboard;

public class LeaderboardSortProp
{
    public string Name { get; set; }
    public ushort Type { get; set; }
}

public class LeaderboardEntry
{
    public uint Rank { get; set; }
    public qUUID PID { get; set; }
    public uint Score { get; set; }
}

public class LeaderboardOverviewResult
{
    public List<LeaderboardEntry> TopEntries { get; set; }
    public LeaderboardEntry EstimatedUserPosition { get; set; }
    public List<LeaderboardEntry> UserPositions { get; set; }
    public uint TotalPlayers { get; set; }
}

public class PlayerSessionScore
{
    public qUUID PlayerId { get; set; }
    public ulong Score { get; set; }
    public uint Unk1 { get; set; }
    public uint Unk2 { get; set; }
}

public class PlayerSessionInformation
{
    public string SessionId { get; set; }
    public List<qUUID> Participants { get; set; }
    public uint Unk1 { get; set; }
    public uint Unk2 { get; set; }
    public byte Unk3 { get; set; }
    public uint Unk4 { get; set; }
    public List<PlayerSessionScore> Scores { get; set; }
}