public class CsvConst
{
    static public readonly int TrapCfgTypeTrap = 1;
    static public readonly int TrapCfgTypeDefence = 2;

    public static int ToTrapCfgId(int typ, int id)
    {
        return typ * 100 + id;
    }
}
