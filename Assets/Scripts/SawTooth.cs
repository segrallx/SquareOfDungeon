using UnityEngine;

public class SawTooth : MonoBehaviour
{
    //长度
    private float mSpeed = 0.2f;
    private int mLength = 3;
    private float mLengthUnit = 2f;
	public Direction mDirection;
    private bool mSetInit = false;
	private Vector3 mVelocity ;

    Transform mHead;
    Transform mTail;
    Transform mRail;
    Transform mSaw;

    enum Status
    {
        None = 0,
        ToHead = 1,
        ToTail = 2,
    };

    public enum Direction
    {
        Horizontal = 0,
        Vertical = 1,
    };


    Status mSawRolling = Status.None;
    Vector3 mRotate = new Vector3(0, 15, 0);

    void Start()
    {
        mHead = transform.Find("Head");
        mTail = transform.Find("Tail");
        mRail = transform.Find("Rail");
        mSaw = transform.Find("Saw");
    }

    public void SetInit(Vector3 pos, int length, float unit, Direction direction)
    {
        Vector3 headPos = Vector3.zero;
        Quaternion headRotaion = Quaternion.identity;

        Vector3 tailPos = Vector3.zero; ;
        Quaternion tailRotaion = Quaternion.identity;

		Quaternion railRotation = Quaternion.identity;
		Vector3 railScale = new Vector3(length*unit,1,1) ;

		Quaternion sawRotaion = Quaternion.identity;

        switch (direction)
        {
            case Direction.Horizontal:
				headPos = pos - new Vector3(0, 0, 1) * unit * length;
                tailPos = pos + new Vector3(0, 0, 1) * unit * length;

				headRotaion = Quaternion.Euler(-90, 90, 0);
				tailRotaion = Quaternion.Euler(-90, 270, 0);

				railRotation = Quaternion.Euler(-90, -90, 0);
				sawRotaion =  Quaternion.Euler(-90, 0, 90);
                break;
            case Direction.Vertical:
				headPos = pos - new Vector3(1, 0, 0) * unit * length;
                tailPos = pos + new Vector3(1, 0, 0) * unit * length;

				headRotaion = Quaternion.Euler(-90, 180, 0);
				tailRotaion = Quaternion.Euler(-90, 0, 0);

				railRotation = Quaternion.Euler(-90, 90, -90);
				sawRotaion =  Quaternion.Euler(-90, 0, 0);
                break;
        }

        mHead.localPosition = headPos;
        mHead.localRotation = headRotaion;

        mTail.localPosition = tailPos;
        mTail.localRotation = tailRotaion;

        mSaw.localPosition = mHead.localPosition;
		mSaw.localRotation = sawRotaion;

        mRail.localPosition = Vector3.zero;
		mRail.localRotation = railRotation;
		mRail.localScale = railScale;


        Invoke("BeginRoll", 1);
    }

	Vector3 HeadTailVelocity(Vector3 head, Vector3 tail)
	{
		return (tail - head)/ ((2*mLength+1)*mLengthUnit) *mSpeed ;
	}


    // Use this for initialization
    void BeginRoll()
    {
        mSawRolling = Status.ToTail;
		mSaw.localPosition = mHead.localPosition;
		mVelocity = HeadTailVelocity(mHead.localPosition, mTail.localPosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 如果没有初始化，默认给一个。
        if (!mSetInit)
        {
            mSetInit = true;
            SetInit(Vector3.zero, 3, 2, mDirection);
        }

        if (mSawRolling != Status.None)
        {
            mSaw.Rotate(mRotate);
        }

        switch (mSawRolling)
        {
            case Status.ToHead:
				mSaw.localPosition = mSaw.localPosition + mVelocity;
				if ((mSaw.localPosition - mHead.localPosition).magnitude<0.4)
                {
					mSawRolling = Status.ToTail;
					mSaw.localPosition = mHead.localPosition;
					mVelocity = HeadTailVelocity(mHead.localPosition, mTail.localPosition);
                }
                break;
            case Status.ToTail:
                mSaw.localPosition = mSaw.localPosition + mVelocity;
                if ((mSaw.localPosition - mTail.localPosition).magnitude<0.4)
                {
					mSaw.localPosition = mTail.localPosition;
                    mSawRolling = Status.ToHead;
					mVelocity = HeadTailVelocity(mTail.localPosition, mHead.localPosition);
                }
                break;
        }
    }
}
