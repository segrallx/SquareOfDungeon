using System;

class MonsterCsvLine : ICsvLine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Speed { get; set; }
    public int Hp { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int Exp { get; set; }
    public int Coin { get; set; }

    public int Identity()
    {
        return Id;
    }

    public object Obj()
    {
        return this;
    }

    public override string ToString()
    {
        return String.Format("Id {0}", Id);
    }

    public Fight GetFight()
    {
        var fight = new Fight();
        //fight.Id = id;
        fight.Atk = Atk;
        fight.Def = Def;
        fight.Hp = Hp;
        fight.MaxHp = Hp;
        //fight.Exp = Exp;
        return fight;
    }

};
