
class HeroAbilityCsvLine : ICsvLine
{
    public int Id { get; set; }
    public int Level { get; set; }
    public int Value { get; set; }

    public int Identity()
    {
        return Id * 100 + Level;
    }

    public object Obj()
    {
        return this;
    }
};
