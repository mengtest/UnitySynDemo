using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
/****
特效粒子 可控制播放 停止.
****/
public class PsEffect : EffectBase
{
    public bool isStop=false;
    private float _rateOverTime=0;
    public ParticleSystem[] particleSystemList;

    public PsEffect()
    {
        
    }
   
    public override void onViewLoadFin(){
          particleSystemList = gameObject.GetComponentsInChildren<ParticleSystem>(true);
          if(isStop){
              Stop();
          }
          if(_rateOverTime!=0){
              setEmissionRate(_rateOverTime);
          }
    }
    
    public void  Play(){
        isStop=false;
        if(!initViewFin)return;
        for (int i = 0; i < particleSystemList.Length; i++)
        {
             ParticleSystem ps = particleSystemList[i];
             ps.Play();
        }
    }
    //从新播放
    public void  RePlay(){
        isStop=false;
        if(!initViewFin)return;
        for (int i = 0; i < particleSystemList.Length; i++)
        {
             ParticleSystem ps = particleSystemList[i];
             ps.Simulate(0, false, true);
             ps.Play();
        }
    }
    public void  StopNextFrame(){
        isStop=true;
        if(!initViewFin)return;
        if(isDead)return;
        float time=GameSettings.Instance.deltaTime;
        if(_rateOverTime!=0){
            time= 1/_rateOverTime;
        }
        TimerManager.Instance.Once(time,Stop,false);
    }
    public void  Stop(){
        isStop=true;
        if(!initViewFin)return;
        if(isDead)return;
        for (int i = 0; i < particleSystemList.Length; i++)
        {
             ParticleSystem ps = particleSystemList[i];
             ps.Stop();
        }
    }
    //单位时间发射的粒子数量。rateOverTime =  1/fireRate 
    public void setEmissionRate(float rateOverTime){
        _rateOverTime=rateOverTime;
        if(!initViewFin)return;
        for (int i = 0; i < particleSystemList.Length; i++)
        {
             ParticleSystem ps = particleSystemList[i];
             EmissionModule emission=ps.emission;
             emission.rateOverTime=rateOverTime;
        }
    }


    public override void onGet()
    {
        isStop=false;
        _rateOverTime=0;
        base.onGet();
    }
    /**
     回收..
    **/
    public override void onRecycle()
    {
        base.onRecycle();
    }
    /*********
      销毁
    *******/
    public override void onRelease(){
        particleSystemList=null;
        base.onRelease();
    }
}