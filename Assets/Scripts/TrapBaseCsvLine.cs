class TrapBaseCsvLine : ICsvLine
{
    public int Id { get; set; }
    public int Type { get; set; }
    public int Atk { get; set; }

    public int Identity()
    {
        return Id * 100 + Type;
    }

    public object Obj()
    {
        return this;
    }

    public Fight GetFight()
    {
        var fight = new Fight();
        //fight.Id = id;
        fight.Atk = Atk;
        return fight;
    }
};
