using UnityEngine;

public class ShowGameFPS : MonoBehaviour
{
    /// <summary>
    /// 每次刷新计算的时间      帧/秒
    /// </summary>
    public float updateInterval = 0.5f;
    /// <summary>
    /// 最后间隔结束时间
    /// </summary>
    private double lastInterval;
    private int frames = 0;
    private int currFPS;
    private string fpsLabel;
     
     public  Rect m_Rect=new Rect(0,25,100,25);

    // Use this for initialization
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        fpsLabel ="";
    }

    // Update is called once per frame
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            currFPS = (int)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
            fpsLabel = string.Format("FPS:{0}", currFPS);
        }

        //onInfo();
    }

    private long mixNum = long.MaxValue;
    private long maxNum = long.MinValue;
    private int n = 0;
    private void onInfo()
    {
        n++;
        if (n < 30) return;
        n = 0;

       //  string str = "";
//         long num = Profiler.GetMonoUsedSizeLong() / 1048576;
//         if (num < mixNum) mixNum = num;
//         else if (num > maxNum)
//         {
//             maxNum = num;
//             mixNum = long.MaxValue;
//         }
        //str += string.Format("Lua: {0}k", LuaSystem.ins.getGCCount()) + "\n";
   //     str += num + "M " + mixNum + "M~" + maxNum + "M\n";
    //    str += " UsedHeap:" + Profiler.usedHeapSize / 1048576 + "M" + "\n";
   //     str += " MonoHeap:" + Profiler.GetMonoHeapSizeLong() / 1048576 + "M" + "\n";
  //      str += "MonoUsed:" + Profiler.GetMonoUsedSizeLong() / 1048576 + "M" + "\n";
   //     str += "AllMemory:" + Profiler.GetTotalAllocatedMemoryLong() / 1048576 + "M" + "\n";
  //      str += "UnUsedReserved:" + Profiler.GetTotalUnusedReservedMemoryLong() / 1048576 + "M" + "\n";
  //      str += " Reserved:" + Profiler.GetTotalReservedMemoryLong() / 1048576 + "M";


      //  infoLabel.text = str;
    }
    private void OnGUI()
    {
    //    m_Rect.x = 0;
    //    m_Rect.y = 25;
    //    m_Rect.width = 100;
    //    m_Rect.height = 25;
       GUI.color=Color.red;
       GUI.Label(m_Rect, "FPS:" + currFPS);
    }

    void OnDestroy()
    {
        fpsLabel = null;
  //      infoLabel = null;
    }
}
