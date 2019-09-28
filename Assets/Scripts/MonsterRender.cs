using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRender : CharRender
{
    public override void FixedUpdate()
    {
        if (mChar == null)
        {
            return;
        }

        base.FixedUpdate();

        // 设置所有的怪物的动画状态.
        var stateChange = mChar.FixedUpdate();
        if (stateChange)
        {
            switch (mChar.mState.mState)
            {
                case CharState.State.Move:
                    mAnimator.SetInteger("State", 1);
                    break;
                case CharState.State.Chase:
                    mAnimator.SetInteger("State", 1);
                    break;
                case CharState.State.Atk:
                    mAnimator.SetInteger("State", 2);
                    break;
                case CharState.State.Dizzy:
                    mAnimator.SetInteger("State", 3);
                    break;
                default:
                    //idle
                    mAnimator.SetInteger("State", 0);
                    break;
            }
        }
    }

    public override void OnTriggerEnter(Collider coll)
    {

        base.OnTriggerEnter(coll);

        switch (coll.tag)
        {
            case "Sword":
                var c = coll.GetComponent<Sword>().Owner.GetComponent<CharRender>().mChar;
                //EffectSet(CharEffect.Electro, c, EffectArg.InfectAdj(), EffectArg.SetSource());
                EffectSet(CharEffect.Fire, c);
                break;
        }

    }

    public override void Dead()
    {
        var role = StaffHolder.Instance().mRole;
        role.ExpAdd(mChar.GetExp());
        base.Dead();

    }



    internal MonsterCsvLine GetCsv()
    {
        var mm = mChar as Monster;
        return mm.mCsv;
    }


    void DestorySelf()
    {
        Destory();
        Destroy(gameObject);
        var staff = StaffHolder.Instance();
        var csv = GetCsv();
        staff.AddCoinToRole(csv.Coin, transform.position);
    }



}
