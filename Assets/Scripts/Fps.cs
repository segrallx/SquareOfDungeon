using UnityEngine;

public class Fps : MonoBehaviour
{
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
    private GUIStyle guiStyle = new GUIStyle();
    // create a new variable
    // private float ScreenX;
    // private float ScreenY;

    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        //ScreenY = ((Screen.height) - 200);
    }

    void OnGUI()
    {
        guiStyle.fontSize = 40; //change the font size
        guiStyle.normal.textColor = Color.green;
        GUILayout.Label(" " + fps.ToString("f2"), guiStyle);
    }

    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
    }

}
