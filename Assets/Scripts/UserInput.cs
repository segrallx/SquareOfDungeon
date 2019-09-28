using UnityEngine;

public class UserInput : MonoBehaviour
{
    public RoleRender mRole;

    // 命令模式处理输入
    public void KeyBoardInputCmd()
    {
        if (mRole.IsDead())
        {
            return;
        }

        Cmd cmd = null;
        if (Input.GetKeyDown(KeyCode.W))
        {
            cmd = new CmdMove(Direction.Up);
            object ret;
            if (CsvAgent.Instance().GetCsvLine(CsvAgent.MonsterCsv, 1, out ret))
            {
                var ml = ret as MonsterCsvLine;
                Debug.LogFormat("get monstr cool {0} {1} {2} {3} {4}",
                                ml.Id, ml.Name, ml.Hp, ml.Atk, ml.Def);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            cmd = new CmdMove(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            cmd = new CmdMove(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            cmd = new CmdMove(Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            cmd = new CmdAtk(CmdAtk.Idx.Primary);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            cmd = new CmdAtk(CmdAtk.Idx.Secondary);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            var screenPos = Camera.main.WorldToScreenPoint(mRole.transform.position);
            Debug.LogFormat("pos {0} screenpos {1}", mRole.transform.position, screenPos);
        }

        if (cmd != null)
        {
            cmd.Do(mRole);
        }

    }


    void Update()
    {
        KeyBoardInputCmd();
    }
}
