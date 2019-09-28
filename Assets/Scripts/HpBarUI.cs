using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    private Slider mSlider;
    public Text mCurHp;

    void Awake()
    {
        mSlider = GetComponent<Slider>();
    }

    public void SetPostion(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetColor(Color c)
    {
        mSlider.fillRect.GetComponent<Image>().color = c;
    }

    public void SetCurHp(int hp, int maxHp)
    {

        mSlider.value = (float)hp / (float)maxHp * 100;
        mCurHp.text = string.Format("{0}", hp);
    }


    public void Destory()
    {
        Destroy(gameObject);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void HiddenHpText()
    {
        mCurHp.gameObject.SetActive(false);
    }
}
