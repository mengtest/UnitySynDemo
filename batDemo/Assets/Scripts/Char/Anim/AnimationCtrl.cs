// using System;
 using UnityEngine;
 using System.Collections;
using System.Collections.Generic;
// using System.Text;


public class AnimationCtrl:IAniCtrl
     {
//         // 动画一帧时间
//         private const float ANI_FRAME_TIME = 1.0f/30.0f;

//         // 动画对象
        private Animation m_Animations = null;
        public Dictionary<string, AnimationState> m_AnimationState = new Dictionary<string, AnimationState>();
        private string m_strCurAni = "";
        private float m_fBlendTime = 0.0f;
        private float m_fSpeed = 1.0f;
        private float m_fLastTime = 0.0f;
        private bool  m_bPause = false; // 暂停标识

        public  AnimationCtrl(ObjBase obj){
             m_Animations = obj.gameObject.GetComponent<Animation>();
             if(m_Animations!=null){
                  IEnumerator it = m_Animations.GetEnumerator();
                    while (it.MoveNext())
                    {
                        AnimationState AniState = (AnimationState)it.Current;
                        if (!m_AnimationState.ContainsKey(AniState.name))
                        {
                            m_AnimationState.Add(AniState.name, AniState);
                        }
                        else
                        {
                            DebugLog.LogError("{0}存在重复动作名{1}，请检查资源！", obj.poolname, AniState.name);
                        }
                    }
             }else{

                  DebugLog.LogError("{0}没有添加 Animation系统，请检查资源！", obj.poolname);
             }
        }

        public bool isPlaying(string aniName) {
            if(m_Animations==null) return false;
            return m_Animations.isPlaying;
        }
        public bool isPlayingUpLayer(string aniName)
        {
            return this.isPlaying(aniName);
        }
        public bool hasAni(string aniName)
        {
             return  m_AnimationState.ContainsKey(aniName);      
        }

        public void pause(string aniName)
        {
               AnimationState state=null;
               if(m_AnimationState.TryGetValue(aniName,out state))
                {
                    state.speed = 0.0f;
                    m_bPause = true;
                }
        }

        public void play(string aniName, float startTime = 0, float speed = 1, float fBlendTime = 0.25F)
        {
              AnimationState state=null;
            if(m_AnimationState.TryGetValue(aniName,out state))
            {
                state.speed = m_fSpeed;
                m_fSpeed = speed;
                state.time=startTime;
                 m_Animations.CrossFade(aniName, m_fBlendTime);
            }
        }
        public void playUpLayer(string aniName, float startTime = 0, float speed = 1, float fBlendTime = 0.25F)
        {
           this.play(aniName, startTime , speed , fBlendTime);
        }

        public void Release()
        {
            this.m_Animations=null;
             this.m_AnimationState=null;
        }

        public void resume(string aniName)
        {
             AnimationState state=null;
            if(m_AnimationState.TryGetValue(aniName,out state))
            {
                state.speed = m_fSpeed;
                if (state.wrapMode == WrapMode.Loop) {
                    m_Animations.CrossFade(aniName, m_fBlendTime);
                }
                m_bPause = false;
            }
        }

        public void setSpeed(float speed, string aniName)
        {
            AnimationState state=null;
            if(m_AnimationState.TryGetValue(aniName,out state))
            {
                state.speed = m_fSpeed;
                m_fSpeed = speed;
            }
        }

        public void stop(string aniName)
        {
             AnimationState state=null;
            if(m_AnimationState.TryGetValue(aniName,out state))
            {
                state.time = state.length;
                state.speed = 0;
                m_Animations.CrossFade(aniName, m_fBlendTime);
             }
        }
}

