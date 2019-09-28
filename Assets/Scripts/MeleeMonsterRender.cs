using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeMonsterRender : MonsterRender {

	public override void AtkBegin()
    {
		base.AtkBegin();
        //Debug.LogFormat("melee monster {0} atk begin", mChar.mCls);
	}

    public  override void AtkEnd()
    {
		base.AtkEnd();
        //Debug.LogFormat("melee monster {0} atk end", mChar.mCls);
    }


}
