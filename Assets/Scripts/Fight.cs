using UnityEngine;

public class Fight
{
    public float Atk;
    public float Def;
    public int Hp;
    public int MaxHp;
    public float Crit; //暴击

    public Fight()
    {
        Hp = 1;
        MaxHp = 1;
        Crit = 1.0f;
    }

    public void Reset()
    {
        Hp = 1;
        MaxHp = 1;
        Crit = 1.0f;
    }

    public void CopyAttr(Fight f)
    {
        //Id = f.Id;
        Atk = f.Atk;
        Def = f.Def;
        Hp = f.Hp;
        MaxHp = f.MaxHp;
        Crit = f.Crit;
        //Exp = f.Exp;
    }

    // 被攻击.
    public bool BeAttacked(Fight f)
    {
        var atk = f.Atk * f.Crit;
        var hp = atk * (atk / (atk + Def));
        Debug.LogFormat("BeAttacked hp {0} atk {1}", hp, atk);
        return Damage((int)hp);
    }


    public bool BeEffected(Fight f)
    {
        var hp = 10;
        Debug.LogFormat("BeEffected hp {0}", hp);
        return Damage((int)hp);
    }


    private bool Damage(int hp)
    {
        Hp -= hp;
        if (Hp <= 0)
        {
            Hp = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public float HpRate()
    {
        return (float)Hp / (float)MaxHp * 100;
    }

    public void HpReset()
    {
        Hp = MaxHp;
    }

    public bool IsDead()
    {
        return Hp <= 0;
    }

};
