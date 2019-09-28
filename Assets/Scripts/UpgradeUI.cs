using UnityEngine;
//using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
    public D3UI mD3UI;
    public UpgradeUIChoose mChoose1;
    public UpgradeUIChoose mChoose2;
    public UpgradeUIChoose mChoose3;
    public RoleRender mRole;
    private List<HeroAbilityEnumCsvLine> mList = new List<HeroAbilityEnumCsvLine>();

    // 初始化
    public void Awake()
    {
        Debug.LogFormat("upgrade test");
        var table = CsvAgent.Instance().GetCsvTable(CsvAgent.HeroAbilityEnumCsv);
        foreach (KeyValuePair<int, ICsvLine> kv in table)
        {
            Debug.LogFormat("upgrade k {0}", kv.Key);
            if (kv.Value is HeroAbilityEnumCsvLine)
            {
                mList.Add(kv.Value as HeroAbilityEnumCsvLine);
            }
        }
    }

    // 随机洗牌
    private System.Random rng = new System.Random();
    public void Shuffle()
    {
        var list = mList;
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            HeroAbilityEnumCsvLine value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // 展示
    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);

        Shuffle();
        var s1 = mList[0];
        var s2 = mList[1];
        var s3 = mList[2];

        mChoose1.SetAbility(s1);
        mChoose2.SetAbility(s2);
        mChoose3.SetAbility(s3);
        mD3UI.HiddenAll();
    }


    // 选择
    public void Choose(int id)
    {
        Debug.LogFormat("I Choose {0}", id);
        gameObject.SetActive(false);
        var s = mList[id - 1];
        mRole.AbilityUpgrade(s.Id);
        mD3UI.ShowAll();
        Time.timeScale = 1f;
    }

}
