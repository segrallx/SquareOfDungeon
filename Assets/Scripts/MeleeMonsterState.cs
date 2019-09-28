using UnityEngine;

public class MeleeMonsterState : MonsterState
{
    public MeleeMonsterState(string cls, Vector3 position, float moveSpeed) : base(cls, position, moveSpeed)
    {
    }

    // 近距离攻击和远程攻击的达成条件不同
    protected override bool CheckCanAtk()
    {
        return base.CheckCanAtk();
    }

}
