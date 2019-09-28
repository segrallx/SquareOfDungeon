using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private BoxCollider mCollider;

    void Start()
    {
        mCollider = GetComponent<BoxCollider>();
    }

    public bool mIsAtking
    {
        set
        {
            if (value == true)
            {
                mCollider.enabled = true;
            }
            else
            {
                mCollider.enabled = false;
            }
        }
    }
    public RoleRender Owner;
}
