using UnityEngine;
using System.Collections.Generic;

public class RoleState : CharState
{
    public RoleState(string cls, Vector3 position, float moveSpeed) : base(cls, position, moveSpeed)
    {
    }

    public override bool Moveable(Tiled tile)
    {
        if (!base.Moveable(tile))
        {
            return false;
        }

        return true;
    }


}
