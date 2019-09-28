
class HeroExpCsvLine : ICsvLine
{
    public int Level { get; set; }
    public int Exp { get; set; }

    public int Identity()
    {
        return Level;
    }

    public object Obj()
    {
        return this;
    }
};
