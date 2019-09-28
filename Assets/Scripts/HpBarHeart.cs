using UnityEngine;
using UnityEngine.UI;

public class HpBarHeart : MonoBehaviour
{

    private State mState = State.Normal;
    private int mTwinkTime ;

    public GameObject mBase;
    public GameObject mHeart;


    public enum State
		{
        Normal = 0,
        Damage = 1,
    };

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mTwinkTime > 0)
        {
            mTwinkTime -= 2;
            switch (Random.Range(0, 2))
            {
                case 0:
                    mBase.GetComponent<Image>().color = Color.white;
                    mHeart.GetComponent<Image>().color = Color.white;
                    break;
                case 1:
                    mBase.GetComponent<Image>().color = Color.black;
                    mHeart.GetComponent<Image>().color = Color.black;
                    break;
            }
        }
        else if (mTwinkTime < 0)
        {
            mBase.GetComponent<Image>().color = Color.white;
            mHeart.GetComponent<Image>().color = Color.white;
			mTwinkTime=0;
        }
    }

    // Update is called once per frame
    public void SetState(State state)
    {
        if (mState == state)
        {
            return;
        }

        if (mState == State.Normal && state == State.Damage)
        {
            mTwinkTime = 61;
            mHeart.SetActive(false);
        }
        else if (mState == State.Damage && state == State.Normal)
        {
            mTwinkTime = 61;
            mHeart.SetActive(true);
        }

        mState = state;
    }
}
