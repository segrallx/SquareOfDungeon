using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlyObject : MonoBehaviour
{
    private Direction mDirection = Direction.None;
    private float mSpeed;
    private float mActiveTime;
    public GameObject mOwner;

    // Use this for initialization
    void Start()
    {
        mActiveTime = 18.0f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetDirection(Direction dir, float speed)
    {
        mDirection = dir;
        mSpeed = speed;
        if (speed > 0)
        {
            mActiveTime = 4.0f;
        }
    }

    public void SetSpeed(float speed)
    {
        mSpeed = speed;
        mActiveTime = 4.0f;
    }


    void FixedUpdate()
    {

        mActiveTime -= Time.deltaTime;
        if (mActiveTime < 0)
        {
            Destroy(gameObject);
        }
        if (mDirection == Direction.None)
        {
            return;
        }

        var moveVec = Unit.MoveVector3(mDirection);
        transform.position = transform.position + (mSpeed * Time.deltaTime * moveVec);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject == mOwner)
        {
            return;
        }

        Debug.Log("flyObject trigger enter " + coll.ToString() + " " + coll.tag);
        switch (coll.tag)
        {
            case "Shield":
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;
            default:
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;
        }

    }

}
