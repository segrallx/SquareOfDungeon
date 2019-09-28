// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIChoose : MonoBehaviour
{
    public GameObject mUpgradeUi;
    public Text mText;

    //  设置能力
    public void SetAbility(HeroAbilityEnumCsvLine csv)
    {
        mText.text = csv.Name;
    }

}
