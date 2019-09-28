using UnityEngine;
using System;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text mLevellbl;
    //public Text mCoinLbl;
    public Slider mLevelSlider;
    public ValueUI mValueCoinUI;

    public void SetLevelBar(int lv, float expRate)
    {
        mLevellbl.text = String.Format("Lv.{0}", lv);
        mLevelSlider.value = expRate;
    }

    // public void SetCoin(int coin)
    // {
    //     mCoinLbl.text = String.Format("{0}", coin);
    // }

    public void SetRole(Role role)
    {
        mValueCoinUI.SetValue((int)role.mCoin);
    }

    public void ShowCoin(Role role)
    {
        mValueCoinUI.ShowValue((int)role.mCoin);
    }




}
