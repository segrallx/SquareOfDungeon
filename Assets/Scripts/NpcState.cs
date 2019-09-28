using UnityEngine;

public class NpcState : CharState
{
    public NpcState(string cls, Vector3 position, float moveSpeed) : base(cls, position, moveSpeed)
    {
        //UpdateIdxRaw();
    }

    // public override bool FixedUpdate()
    // {
    //     UpdateIdx();
    //     return false;
    // }
}
