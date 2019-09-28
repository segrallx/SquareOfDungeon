using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RssEffect : MonoBehaviour
{
    /// 生成所有数量的资源图标 间隔总时间
    public float mGeneratetime = 1f;
    /// 扩散半径-生成资源图标后.扩散.形成不一样的轨迹，值的意义是起点到终点距离的百分比
    public float mExplosionRadius = 20;
    /// 飞行速度
    public float mMoveSpeed = 20;
    /// 旋转速度
	public float mRotateSpeed = 3;
    /// 资源图标 prefab
    public GameObject[] mRssPrefab;
    /// 资源图标对象池gameObject
    //private GameObject mGameObjectPool;
    /// 资源图标对象池
    private Dictionary<int, List<Rss>> mObjectPool;

    ////////////////////////////////////////
    // rss effect
    public GameObject mRssGameObjectPool;
    public GameObject mRssStart;
    public GameObject mRssEnd;
    Canvas mCanvas;

    public CoinUI mCoinUI;


    // 资源
    private class Rss
    {
        public int type;
        public float mMoveTime;
        public Transform mTransform;
        public Vector3 rotateSpeed;
    }

    // Use this for initialization
    void Start()
    {
        mRssGameObjectPool.SetActive(false);
        foreach (var pre in mRssPrefab)
        {
            pre.SetActive(false);
        }

        // if (mGameObjectPool != null)
        // {
        //     mGameObjectPool.SetActive(false);
        // }

        mObjectPool = new Dictionary<int, List<Rss>>();
        mCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.LogFormat("Player X Pos {0} {1} ", mRssStart.transform.position, mRssEnd.transform.position);
            Play(0, mRssStart.transform.position, mRssEnd.transform.position, 1, null);
        }
    }

    void Destroy()
    {
        if (mObjectPool == null)
        {
            return;
        }

        foreach (var rssListInfo in mObjectPool)
        {
            for (int i = rssListInfo.Value.Count - 1; i >= 0; i--)
            {
                Destroy(rssListInfo.Value[i].mTransform.gameObject);
                rssListInfo.Value[i].mTransform = null;
            }
        }

        mObjectPool = null;
        mRssPrefab = null;
    }

    private void ReleaseRss(Rss rss)
    {
        if (rss == null)
        {
            return;
        }
        rss.mTransform.localPosition = Vector3.zero;
        rss.mMoveTime = 0f;
        rss.mTransform.SetParent(mRssGameObjectPool.transform);
        mObjectPool[rss.type].Add(rss);
    }


    private Rss GetARss(int type, Vector3 sourceIn)
    {
        if (mObjectPool == null)
        {
            mObjectPool = new Dictionary<int, List<Rss>>();
        }

        if (!mObjectPool.ContainsKey(type))
        {
            mObjectPool.Add(type, new List<Rss>());
        }

        Rss rss;
        if (mObjectPool[type].Count == 0)
        {
            rss = InstantRss(type, sourceIn);
        }
        else
        {
            rss = mObjectPool[type][0];
            mObjectPool[type].RemoveAt(0);
        }

        rss.mTransform.position = sourceIn;
        rss.mTransform.SetParent(transform);
        rss.mTransform.localScale = Vector3.one;

        return rss;

    }

    private Rss InstantRss(int type, Vector3 sourceIn)
    {
        Rss rss = new Rss();

        rss.type = type;
        rss.rotateSpeed = new Vector3(Mathf.Lerp(0, 360, Random.Range(0, 1.0f)),
                                      Mathf.Lerp(0, 360, Random.Range(0, 1.0f)),
                                      Mathf.Lerp(0, 360, Random.Range(0, 1.0f))) * mRotateSpeed;
        GameObject go = Instantiate(mRssPrefab[type], sourceIn, Random.rotationUniform) as GameObject;
        go.SetActive(true);
        rss.mTransform = go.transform;

        return rss;
    }

    public void PlayCoin(Vector3 source, int count)
    {
        var mainCamera = Camera.main;
        var pos = mainCamera.WorldToScreenPoint(source);
        Vector2 ret = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas.GetComponent<RectTransform>(), pos,
                                                                mCanvas.worldCamera, out ret);
        //Play(0, mRssStart.transform.position, mRssEnd.transform.position, count, null);
        Debug.LogFormat("Player Coin Pos {0} {1} {2} {3}", pos, source, ret, mRssEnd.transform.position);
        mRssStart.GetComponent<RectTransform>().anchoredPosition = ret;
        Play(0, mRssStart.transform.position, mRssEnd.transform.position, count, delegate (int x)
        {
            Debug.LogFormat("system action {0}", x);
            mCoinUI.Show();
        });
    }

    /// 类型
    /// 起点坐标-世界坐标
    /// 终点坐标-世界坐标
    /// 粒子数量
    /// 完成后回调
    public void Play(int type, Vector3 source, Vector3 target, int count, System.Action<int> onFinish)
    {
        // Debug.Log("播放特效 Play count=" + count);
        // 生成所有一个资源图标 所耗时间
        float rate = mGeneratetime / count;
        // 扩散半径
        float radius = (target - source).magnitude * mExplosionRadius / 100;
        // 飞行时长
        float flyTime = (target - source).magnitude / mMoveSpeed;
        StartCoroutine(OnAnimation(flyTime, type, source, target, count, rate, radius, onFinish));
    }

    private IEnumerator OnAnimation(float flyTime, int type, Vector3 sourceIn, Vector3 targetIn, int count,
                                    float rate, float radius, System.Action<int> onFinish)
    {
        /// 控制点-生成资源图标后 扩散  形成不一样的轨迹
        List<Vector3> ctrlPoints = new List<Vector3>();
        List<Rss> rsses = new List<Rss>();

        float _generateTime = 0;
        float _generateCount = 0;
        float _moveSpeed = 1 / flyTime;
        bool isArriveFirst = true;

        while (true)
        {

            if (_generateCount < count)
            {
                _generateTime += Time.deltaTime;
                for (int i = 0; i < Mathf.Ceil(_generateTime / rate); i++)
                {
                    if (_generateCount < count)
                    {
                        Rss rss = GetARss(type, sourceIn);
                        ctrlPoints.Add(Random.insideUnitSphere * radius);
                        rsses.Add(rss);
                        _generateCount++;
                        _generateTime -= rate;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = rsses.Count - 1; i >= 0; --i)
            {
                Rss rss = rsses[i];
                rss.mTransform.position =
                    Vector3.Lerp(Vector3.Lerp(sourceIn, sourceIn + ctrlPoints[i], rsses[i].mMoveTime * 5),
                                 targetIn, rsses[i].mMoveTime);
                rss.mTransform.Rotate(rss.rotateSpeed.x * Time.deltaTime,
                                      rss.rotateSpeed.y * Time.deltaTime,
                                      rss.rotateSpeed.z * Time.deltaTime);
                rss.mMoveTime = rss.mMoveTime + Mathf.Lerp(0.1f, 1f, rss.mMoveTime) *
                    _moveSpeed * Time.deltaTime;

                if ((rss.mTransform.position - targetIn).magnitude <= 0.1)
                {
                    if (isArriveFirst == false && onFinish != null)
                    {
                        isArriveFirst = true;
                        onFinish(0);
                    }
                    ReleaseRss(rss);
                    rsses.RemoveAt(i);
                    ctrlPoints.RemoveAt(i);
                }
            }

            if (_generateCount >= count && rsses.Count == 0)
            {
                break;
            }

            yield return null;
        }

        if (onFinish != null)
        {
            onFinish(1);
        }
        else
        {
            Debug.Log("onFinish onFinish onFinish == null");
        }
    }

}
