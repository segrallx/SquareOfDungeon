using UnityEngine;
using System.IO;
using CsvHelper;
using System.Collections.Generic;

class CsvAgent : MonoBehaviour
{
    public static CsvAgent _instance;
    public static CsvAgent Instance()
    {
        return _instance;
    }

    // csv file name
    public static string MonsterCsv = "Csv/Monster_Monster";
    public static string HeroExpCsv = "Csv/Hero_Exp";
    public static string HeroAbilityCsv = "Csv/Hero_Ability";
    public static string HeroAbilityEnumCsv = "Csv/Hero_Enum";
    public static string TrapBaseCsv = "Csv/Trap_Base";

    private Dictionary<string, object> mDicts = new Dictionary<string, object>();

    void loadCsvRaw<T>(string path) where T : ICsvLine
    {
        var txt = Resources.Load<TextAsset>(path);

        if (txt != null)
        {
            using (var csv = new CsvReader(new StringReader(txt.text)))
            {
                var dictX = new Dictionary<int, ICsvLine>();
                var records = csv.GetRecords<T>();
                Debug.LogFormat("load csv {0}", path);
                foreach (var r in records)
                {
                    if (r is ICsvLine)
                    {
                        ICsvLine r2 = r;
                        Debug.LogFormat("record {0} ", r2.Identity());
                        dictX.Add(r2.Identity(), r2);
                    }
                }
                mDicts.Add(path, dictX);
            }
        }
    }

    void Awake()
    {
        _instance = this;
        loadCsvRaw<MonsterCsvLine>(MonsterCsv);
        loadCsvRaw<HeroExpCsvLine>(HeroExpCsv);
        loadCsvRaw<HeroAbilityCsvLine>(HeroAbilityCsv);
        loadCsvRaw<HeroAbilityEnumCsvLine>(HeroAbilityEnumCsv);
        loadCsvRaw<TrapBaseCsvLine>(TrapBaseCsv);
    }


    public Dictionary<int, ICsvLine> GetCsvTable(string path)
    {
        object dictObj;
        if (!mDicts.TryGetValue(path, out dictObj))
        {
            return null;
        }

        return dictObj as Dictionary<int, ICsvLine>;
    }

    public bool GetCsvLine(string path, int id, out object ret)
    {
        object dictObj;
        ret = null;
        if (!mDicts.TryGetValue(path, out dictObj))
        {
            return false;
        }

        if (dictObj != null)
        {
            Debug.Log("find dictobj");
        }

        var dict = dictObj as Dictionary<int, ICsvLine>;
        ICsvLine v;
        if (!dict.TryGetValue(id, out v))
        {
            return false;
        }

        ret = v.Obj();
        return true;
    }


    // 根据怪物ID获取其战斗属性
    public MonsterCsvLine GetMonsterCsvByMonsterId(int id)
    {
        object ret;
        if (GetCsvLine(CsvAgent.MonsterCsv, id, out ret))
        {
            var ml = ret as MonsterCsvLine;
            return ml;
        }

        return null;
    }

    // 根据怪物ID获取其战斗属性
    public Fight GetFightByTrapId(int id)
    {
        object ret;
        Fight fight = null;
        //id = id * 100 + cls;
        if (GetCsvLine(CsvAgent.TrapBaseCsv, id, out ret))
        {
            var ml = ret as TrapBaseCsvLine;
            //Debug.LogFormat("get monstr cool {0}", ml.Id);
            fight = ml.GetFight();
        }

        return fight;
    }


    // 根据等级获取其等级属性
    public HeroExpCsvLine GetHeroExpByLevel(int lv)
    {
        object ret;
        HeroExpCsvLine heroExp = null;
        if (GetCsvLine(CsvAgent.HeroExpCsv, lv, out ret))
        {
            return ret as HeroExpCsvLine;
        }

        return heroExp;
    }

    // 根据Key获取能力属性
    public HeroAbilityCsvLine GetHeroAbilityByIdAndLevel(int id, int level)
    {
        object ret;
        HeroAbilityCsvLine heroAbililty = null;
        var key = id * 100 + level;
        if (GetCsvLine(CsvAgent.HeroAbilityCsv, key, out ret))
        {
            return ret as HeroAbilityCsvLine;
        }

        return heroAbililty;
    }




};
