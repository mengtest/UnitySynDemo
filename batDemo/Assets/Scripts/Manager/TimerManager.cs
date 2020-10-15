using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TimerItemModel : PoolObj
{
    //秒.
    public float delay;
    public float during;
    public bool isFrame;
    public bool isLoop;
    public Action callBack;
    public bool removeFlag;

    public override void onRelease()
    {
        callBack=null;
    }

    public override void onRecycle()
    {
       callBack=null;
    }


}

/// <summary>
/// 计时器管理类
/// </summary>
public class TimerManager : MonoSingleton<TimerManager>
{
    List<TimerItemModel> timerList = new List<TimerItemModel>();
    private Pool<TimerItemModel> pool=new Pool<TimerItemModel>("TimerItemModelPool");

    void Update()
    {
        int count = timerList.Count;
        if (count <= 0)
        {
            return;
        }
        for (int i = count - 1; i >= 0; i--)
        {
            TimerItemModel tData = timerList[i];
            if (tData.removeFlag)
            {
                timerList.Remove(tData);
                tData.recycleSelf();
            }
        }
        for (int i = timerList.Count - 1; i >= 0; i--)
        {
            var tData = timerList[i];
            if (tData.callBack == null || tData.removeFlag)
            {
                tData.removeFlag = true;
                continue;
            }
            if (tData.isFrame)
            {
                tData.during += 1;
                if (tData.isLoop)
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.during = 0;
                        tData.callBack();
                    }
                }
                else
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.removeFlag = true;
                        tData.callBack();
                    }
                }
            }
            else
            {
             //   int dt = (int)(Time.deltaTime * 1000);
                tData.during += Time.deltaTime;
                if (tData.isLoop)
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.during = 0;
                        tData.callBack();
                    }
                }
                else
                {
                    if (tData.during >= tData.delay)
                    {
                        tData.removeFlag = true;
                        tData.callBack();
                    }
                }
            }
        }
    }

    /// <summary>
    /// delay秒之后执行一次CallBack，不带参数
    /// </summary>
    public void Once(float delay,Action callBack, bool coverBefore = true)
    {
        addTime(delay, false, false, callBack, coverBefore);
    }

    /// <summary>
    /// 每n秒执行一次
    /// </summary>
    public void Loop(float delay, Action callBack, bool coverBefore = true)
    {
        addTime(delay, false, true, callBack, coverBefore);
    }

    /// <summary>
    /// n帧之后执行一次
    /// </summary>
    public void FrameOnce(float delayFrame, Action callBack, bool coverBefore = true)
    {
        addTime(delayFrame, true, false, callBack, coverBefore);
    }

    /// <summary>
    /// 每n帧执行一次
    /// </summary>
    public void FrameLoop(float delayFrame, Action callBack, bool coverBefore = true)
    {
        addTime(delayFrame, true, true, callBack, coverBefore);
    }

    /// <summary>
    /// 清除计时器，计时器一定要清理，非循环的计时器也要注意
    /// </summary>
    public void ClearTimer(Action callBack)
    {
        foreach (TimerItemModel tData in timerList)
        {
            if (tData.callBack == callBack)
            {
                tData.removeFlag = true;
            }
        }
    }

    /// <summary>
    /// 消除所有的计时器，除了管理类，都不准调用
    /// </summary>
    public void ClearAllTimer()
    {
       DebugLog.Log("清除所有定时器");
       //timerList.Clear();
       foreach (TimerItemModel tData in timerList)
       {
          tData.removeFlag = true;
       }
    }

    void addTime(float delay, bool isFrame, bool isLoop, Action callBack, bool coverBefore)
    {
        if (callBack == null)
        {
            return;
        }
        if (coverBefore)
        {
            foreach (TimerItemModel tData in timerList)
            {
                if (tData.callBack == callBack)
                {
                    tData.removeFlag = true;
                }
            }
        }
        TimerItemModel item = pool.get();
        item.delay = delay;
        item.isFrame = isFrame;
        item.isLoop = isLoop;
        item.callBack = callBack;
        item.during = 0;
        item.removeFlag = false;
        timerList.Add(item);
    }

    TimerItemModel findTimeItem(Action callBack)
    {
        foreach (TimerItemModel tData in timerList)
        {
            return tData;
        }
        return null;
    }

}
