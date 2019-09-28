//
// 快速访问各种游戏物件.
//
using UnityEngine;

public class StaffHolder : MonoBehaviour
{
    public static StaffHolder _instance;
    public static StaffHolder Instance()
    {
        return _instance;
    }

    public GameObject mDropCoin;
    public GameObject mGameDrop;
    public RoleRender mRole;
    public MazeRender mMazeRender;
    public GameObject mThunder;
    public GameObject mVfxElectroic;
    public GameObject mVfxIce;
    public GameObject mVfxFire;
    public Maze mMaze;

    void Awake()
    {
        _instance = this;
    }

    public void AddCoinToRole(int coin, Vector3 posInput)
    {
        var staff = Instance();
        while (coin > 0)
        {
            coin--;
            var pos = Unit.RandomAround(posInput, 0.5f);
            pos.y = posInput.y;
            var obj = Instantiate(staff.mDropCoin, staff.mGameDrop.transform);
            obj.transform.position = pos;
            obj.AddComponent<Adsorb>().SetDest(staff.mRole.gameObject,
                                               delegate ()
                                               {
                                                   var roleRender = staff.mRole.GetComponent<RoleRender>();
                                                   roleRender.CoinAdd(1);
                                               });
        }
    }
}

