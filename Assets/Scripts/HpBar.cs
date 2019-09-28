using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HpBar : MonoBehaviour
{
    public GameObject mRedHeart;
    public GameObject mDemo;

    private List<HpBarHeart> mRedHeartList = new List<HpBarHeart>();

    // Use this for initialization
    void Start()
    {
        mDemo.SetActive(false);
    }

    // Update is called once per frame
    public void UpdateRole(Role role)
    {
        // UpdateMaxHp(role.mMaxHP);
        // UpdateCurrentHp(role.mHP);
    }

    void UpdateMaxHp(int maxValue)
    {
        // Debug.Log("count :" +mRedHeartList.Count);
        for (var i = mRedHeartList.Count; i < maxValue; i++)
        {
            var heart = Instantiate(mRedHeart, transform);
            heart.transform.localPosition = new Vector3(i * 30, 0, 0);
            mRedHeartList.Add(heart.GetComponent<HpBarHeart>());
        }
    }

    void UpdateCurrentHp(int value)
    {
        for (var i = 0; i < mRedHeartList.Count; i++)
        {
            // Debug.Log("i:"+i);
            if (i < value)
            {
                mRedHeartList[i].SetState(HpBarHeart.State.Normal);
            }
            else
            {
                mRedHeartList[i].SetState(HpBarHeart.State.Damage);
            }
        }
    }
}
