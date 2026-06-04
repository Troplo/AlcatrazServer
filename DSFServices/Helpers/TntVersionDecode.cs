namespace DSFServices.Helpers;

public static class TntVersion
{
    public static string Decode(uint encoded)
    {
        uint a = (encoded >> 24) & 0xFF;
        uint b = (encoded >> 16) & 0xFF;
        uint c = (encoded >> 8) & 0xFF;
        uint d = encoded & 0xFF;

        if (d == 0)
        {
            return $"{a}.{b}.{c}";
        }

        return $"{a}.{b}.{c}.{d}";
    }
}