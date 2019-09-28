
public class HeroAbilityEnumCsvLine : ICsvLine
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Identity()
    {
        return Id;
    }

    public object Obj()
    {
        return this;
    }
};
