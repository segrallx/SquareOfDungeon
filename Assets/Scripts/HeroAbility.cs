//using System;
using System.Collections.Generic;

// 管理玩家的RogueLike能力
public class HeroAbility
{
    private Dictionary<int, int> mTable;

    // 常量
    public const int TypeHp = 1;
    public const int TypeAtk = 2;
    public const int TypeDef = 3;
    public const int TypeExp = 4;
    public const int TypeCoin = 5;
    public const int TypeCrit = 6; //暴击概率，暴击伤害X2.
    public const int TypeMoveSpeed = 7; //玩家移动速度
    public const int TypeAtkSpeed = 8; //玩家攻击速度


    internal float expRate;   // 经验获取比例
    internal float coinRate;  // 金币获取比例

    public HeroAbility()
    {
        mTable = new Dictionary<int, int>();
    }

    public void Reset()
    {
        mTable.Clear();
        // 初始属性为1级。
        Upgrade(HeroAbility.TypeHp);
        Upgrade(HeroAbility.TypeAtk);
        Upgrade(HeroAbility.TypeDef);
        Upgrade(HeroAbility.TypeExp);
        Upgrade(HeroAbility.TypeCoin);
        Upgrade(HeroAbility.TypeCrit);
        Upgrade(HeroAbility.TypeMoveSpeed);

        expRate = 1.0f;
        coinRate = 1.0f;
    }

    // 升级某项能力
    public int Upgrade(int at)
    {
        int value;
        if (mTable.TryGetValue(at, out value))
        {
            mTable[at] = value + 1;
            return value + 1;
        }
        else
        {
            mTable[at] = 1;
            return 1;
        }
    }

    // 检查某项能力的等级
    public int Level(int at)
    {
        int value;
        if (mTable.TryGetValue(at, out value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

}
