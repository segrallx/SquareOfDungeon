using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUI : MonoBehaviour
{

    RoleRender mRole;
    GamePlay mGp;
    // Use this for initialization
    void Start()
    {
        mGp = GameObject.FindGameObjectWithTag("Maze").GetComponent<GamePlay>();
        mRole = GameObject.FindGameObjectWithTag("Role").GetComponent<RoleRender>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 原地复活，花费宝石
    public void Resurgence()
    {
        //mGp.GameRestart(mGp.mCurLevel, true);
        Debug.LogFormat("Resurgence");
        gameObject.SetActive(false);
        mRole.HpReset();
    }

    // 回到主城
    public void GoHome()
    {
        //mGp.GameRestart(0, true);
        Debug.LogFormat("GoHome");
        gameObject.SetActive(false);
        mGp.GameRestart(0, true);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
