


//池接口
public interface IAniCtrl {
    //播放 
    void play(string aniName,float startTime=0,float speed=1f,float fBlendTime = 0.25f,float totalTime=1f);

    //暂停
    void pause(string aniName);
    //继续
    void resume(string aniName);
    //停止 到结束帧.
    void stop(string aniName);
    //是否有该动画.
    bool hasAni(string aniName);
    bool  isPlaying(string aniName);
   /// float getTotalTime();

    //改变速度.
    void setSpeed(float speed,string aniName);

    void Release();
}
