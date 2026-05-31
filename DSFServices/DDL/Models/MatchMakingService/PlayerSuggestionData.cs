namespace DSFServices.DDL.Models;

// JSON
// {"mission_id":3,"sub_mission_id":8,"nat_type":1,"game_mode":4,"roles_bitmask":2,"game_version":3053253107,"notoriety":0,"origin":4,"xp":26785,"hack_defense":100,"time_available":4190651861}
public class PlayerSuggestionData
{
    public uint mission_id { get; set; }
    public int sub_mission_id { get; set; }
    public int nat_type { get; set; }
    public int game_mode { get; set; }
    public uint roles_bitmask { get; set; }
    public uint game_version { get; set; }
    public int notoriety { get; set; }
    public int origin { get; set; }
    public int xp { get; set; }
    public int hack_defense { get; set; }
    public ulong time_available { get; set; }
    bool allow_direct_invasion { get; set; }
}

//{"mission_id":4294967280,"nat_type":1,"roles_bitmask":3,"game_version":3053253107,"portals_mode":6017,"sp_act":2,"notoriety":0,"origin":10,"xp":26785,"hack_defense":100,"time_available":1914485040}
public class PlayerSuggestionDataPortals
{
    public uint mission_id { get; set; }
    public int nat_type { get; set; }
    public uint roles_bitmask { get; set; }
    public uint game_version { get; set; }
    public uint portals_mode { get; set; }
    public int sp_act { get; set; }
    public int notoriety { get; set; }
    public int origin { get; set; }
    public int xp { get; set; }
    public int hack_defense { get; set; }
    public ulong time_available { get; set; }
}