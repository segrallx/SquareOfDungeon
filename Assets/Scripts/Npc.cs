using UnityEngine;

public class Npc : Char
{
    public Npc(Vector3 pos) : base(pos, "npc")
    {
        mState = new NpcState(mCls, pos, 1f);
    }

    // public override void FixedUpdate()
    // {
    //     mState.FixedUpdate();
    // }
}


