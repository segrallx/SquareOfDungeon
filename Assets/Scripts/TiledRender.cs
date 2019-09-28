using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TiledRender : MonoBehaviour
{
    private Color mOldColor = Color.black;
    public Tiled mTiled;
    public MazeRender mMazeRender;

    public bool SetMId(int id)
    {
        if (mTiled.mId == id)
        {
            return false;
        }

        Debug.LogFormat("SetMId {0}", id);
        mTiled.mId = id;
        reRender();
        return true;
    }


    public bool SetMCls(Tiled.Cls cls)
    {
        if (mTiled.mCls == cls)
        {
            return false;
        }

        Debug.LogFormat("SetCls {0}", cls);
        mTiled.mCls = cls;
        reRender();
        return true;
    }

    void reRender()
    {
        var pos = new Vector3(mTiled.mX * 2, 0, mTiled.mY * 2);
        mMazeRender.RenderTile(mTiled, pos);
        Destroy(gameObject);

    }


}
