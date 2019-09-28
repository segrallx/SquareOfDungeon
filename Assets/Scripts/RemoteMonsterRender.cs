using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RemoteMonsterRender : MonsterRender
{
    public GameObject mBullet;
    public Transform mBulletPos;

    public override void AtkBegin()
    {
        base.AtkBegin();
        Debug.LogFormat("remote monster {0} atk begin", mChar.mCls);
        var bullet = Instantiate(mBullet, mBulletPos.position, mBulletPos.rotation, transform);
        bullet.SetActive(true);

        var arrow = bullet.AddComponent<FlyObject>();
        arrow.SetDirection(mChar.DirectionGet(), 0);
        arrow.mOwner = gameObject;
        arrow.SetSpeed(16);
    }

    public override void AtkEnd()
    {
        base.AtkEnd();
        Debug.LogFormat("remote monster {0} atk end", mChar.mCls);
    }
}
