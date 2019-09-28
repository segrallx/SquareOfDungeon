using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBody : MonoBehaviour
{

    public CharRender mRole;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    void OnTriggerEnter(Collider coll)
    {
        mRole.OnTriggerEnter(coll);

        //     if (!mRole.CharGet().MazeActive())
        //     {
        //         return;
        //     }

        //     switch (coll.tag)
        //     {
        //         case "Block":
        //             return;
        //         case "Shield":
        //             return;
        //         case "Sword":
        //             return;
        //     }

        //     Debug.Log("role trigger enter " + coll.ToString() + " " + coll.tag);

        //     switch (coll.tag)
        //     {
        //         case "Thorn":
        //             ITrapCfg itrap = coll.GetComponent<LandThorn>();
        //             var fight = CsvAgent.Instance().GetFightByTrapId(itrap.CfgId());
        //             mRole.BeAttacked(fight);
        //             break;
        //         case "Axe":
        //             mRole.BeAttacked(CsvAgent.Instance().GetFightByTrapId(1));
        //             break;
        //         case "Arrow":
        //             if (coll.gameObject.activeSelf)
        //             {
        //                 mRole.BeAttacked(CsvAgent.Instance().GetFightByTrapId(1));
        //             }
        //             break;
        //         case "FlyObject":
        //             //mRole.Damaged(1);
        //             //@fix
        //             mRole.BeAttacked(CsvAgent.Instance().GetFightByTrapId(1));
        //             coll.gameObject.SetActive(false);
        //             //Destroy(coll.gameObject);
        //             break;
        //         case "Slime":
        //             var owner = coll.gameObject.GetComponent<FightOwner>().mOwner;
        //             mRole.BeAttacked(owner.mChar.mFight);
        //             break;
        //         case "Coin":
        //             coll.gameObject.GetComponent<Coin>().CoinGet(); ;
        //             mRole.GetCoin(1);
        //             break;
        //         case "Chest":
        //             coll.gameObject.GetComponent<Chest>().ChestOpen(); ;
        //             break;
        //     }

    }

}
