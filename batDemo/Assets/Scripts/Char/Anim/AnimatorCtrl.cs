// using System;
 using UnityEngine;
 using System.Collections;
using System.Collections.Generic;
// using System.Text;


public class AnimatorCtrl:IAniCtrl
     {
//         // 动画对象
        private Animator _Animator = null;
        public Dictionary<string, AnimationState> m_AnimationState = new Dictionary<string, AnimationState>();
        private string m_strCurAni = "";
        private float m_fBlendTime = 0.0f;
        private float m_fSpeed = 1.0f;
        private float m_fLastTime = 0.0f;

        private int Layer=0;

        public  AnimatorCtrl(ObjBase obj,int Layer=0){
            this.Layer=Layer;
             _Animator = obj.gameObject.GetComponent<Animator>();
             if(_Animator==null){
                  DebugLog.LogError("{0}没有添加 Animation系统，请检查资源！", obj.poolname);
             }
        }
        public Animator getAnimator(){
           return _Animator;
        }
        public bool isPlaying(string aniName) { 
                if(_Animator==null) return false;
                 //通过名字获取对应层级的动画是否正在播放
                return _Animator.GetCurrentAnimatorStateInfo(this.Layer).IsName(aniName);
        }
        public bool hasAni(string aniName)
        {
             return  m_AnimationState.ContainsKey(aniName);      
        }

        public void pause(string aniName)
        {
            _Animator.speed=0;
        }

        public void play(string aniName, float startTime = 0, float speed = 1, float fBlendTime = 0.25f,float totalTime=1f)
        {
            m_fSpeed = speed;
            m_fBlendTime =fBlendTime;
            _Animator.speed=speed;
            _Animator.CrossFade(aniName, fBlendTime,this.Layer,startTime/totalTime);
        }
        public void Release()
        {
            this._Animator=null;
             this.m_AnimationState=null;
        }

        public void resume(string aniName)
        {
            _Animator.speed=m_fSpeed;
        }

        public void setSpeed(float speed, string aniName)
        {
            _Animator.speed = speed;
            m_fSpeed = speed;
        }

        public void stop(string aniName)
        {
             _Animator.speed=0;
             _Animator.CrossFade(aniName, m_fBlendTime,this.Layer,1);
        }
}

