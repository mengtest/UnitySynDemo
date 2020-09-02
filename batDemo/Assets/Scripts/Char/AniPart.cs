//*************************************************************************
//	动画播放部件
//*************************************************************************
using System;
using UnityEngine;
 
    //动画播放部件
    public class AniPart
    {
        // 动画一帧时间
        private const float ANI_FRAME_TIME = 1.0f/30.0f;
        
        public int Layer=0;
        private ObjBase _obj;
       //当前动画名称.
       public string curAniName { get; private set;}
      // 混合时间 动画间混合 动画过渡 百分比
       private float _fBlendTime=0.25f;
       private bool _isPlay = false;
        //正在播放的时间
        private float _time = 0;
        private float _speed = 1f;
        //一次播放 总时间 总长度
        private float _totalTime = 1;
        //循环次数 0表示无限循环
        private int _loop  = -1;
        private bool _isPause = false;

        private IAniCtrl ctrl=null;
        //播放完后停止.
        private bool _playEndStop=false;
        public Action endAniAction=null;

 //       private int m_nLastStartFrame = -1;

        // 初始化动画控制器
        public void Init(ObjBase obj,int Layer=0)
        {
            this._obj=obj;
            this.Layer=Layer;
            this.endAniAction=null;
        }
        //初始化控制器
        private void initCtrl(){
            if(this.ctrl==null&&this._obj.initViewFin){
                if(this._obj.gameObject.GetComponent<Animator>()){
                    this.ctrl=new AnimatorCtrl(this._obj,this.Layer);
                }else{
                     this.ctrl=new AnimationCtrl(this._obj);
                }
                if(this._isPlay){
                     this.ctrl.play(curAniName,this._time,this._speed,this._fBlendTime);
                }
            }
        }


        public bool hasAni(string strAcionName) { 
          return  ctrl.hasAni(strAcionName);      
        }
        public bool isFinish() {
            if (this._loop == -1) {
            return true;
            }
            return false;
        }

       public float speed{
           set{
                this._speed = value;
                if (this._isPlay && this.ctrl!=null) {
                    this.ctrl.setSpeed(this._speed,this.curAniName);
                }
           }
           get{
               return this._speed;
           }
       }
     public void pause() {
        this._isPause = true;
        if (this.ctrl!=null) {
            this.ctrl.pause(this.curAniName);
        }
    }

    public void stop() {
        this._loop = -1;
        this._time = this._totalTime;
        this._isPlay = false;
        this._isPause = false;
        //看看动画时候需要同步到最后一帧.
        if(this._playEndStop){
            if (this.ctrl!=null) {
                this.ctrl.stop(this.curAniName);
            }
        }
    }

    public void replay() {
        if (this._isPlay) return;
        this._isPause = false;
        this._isPlay = true;
        if (this.ctrl!=null) {
            this.ctrl.play(this.curAniName,this._time,this._speed,this._fBlendTime);
        }
    }

    public void resume() {
        this._isPause = false;
        if (this.ctrl!=null) {
            this.ctrl.resume(this.curAniName);
        }
    }


        // 播放动画
        /**
        @param strAcionName 动作名称
        @param nStartTime 起始时间
        @param nTotalTime 总长度时间
        @param fSpeed 速度
        @param fBlendTime 混合时间 动画间混合 动画过渡
        @param nLoop 循环次数 0表示无限循环
        @param dontRePlaySameAni 不重新播放相同的动画;
         @param layer 新动画系统层级
        */
        public void Play(string strAcionName, float nStartTime = 0,float nTotalTime=1, float fSpeed = 1.0f, float fBlendTime = 0.25f ,int nLoop = 1, bool dontRePlaySameAni=false,bool finishStop=false)
        {            
            this._playEndStop=finishStop;
            if( dontRePlaySameAni && curAniName == strAcionName )
            {
                //继续播放 
                this._fBlendTime = fBlendTime;
                this.speed = fSpeed;
                this._loop = nLoop;
                this._totalTime=nTotalTime;
                this._isPause = false;
                this._isPlay = true;
                 if(this.ctrl!=null&&!this.ctrl.isPlaying(strAcionName)){
                     this.ctrl.play(curAniName,this._time,this._speed,this._fBlendTime);
                 }            
            }else{
                this.curAniName = strAcionName;
                this._time = nStartTime;
                this._speed = fSpeed;
                this._isPlay = true;
                this._totalTime = nTotalTime;
                this._loop = nLoop;
                this._isPause = false;
                 if(this.ctrl!=null){
                     this.ctrl.play(curAniName,this._time,this._speed,this._fBlendTime);
                 }
            }
        }
        public void fixUpdate() {
            initCtrl();
            if (!this._isPlay || this._isPause) {
                return;
            }
            if(this._loop == -1) return;
            this._time = this._time + Time.fixedDeltaTime * this._speed;
            if (this._loop == 0 ) {
                if (this._time >= this._totalTime) {
                    this._time = 0;
                }
            } else if (this._loop > 0) {
                if (this._time >= this._totalTime) {
                    this._loop--;
                    this._time = 0;
                    if (this._loop <= 0) {
                        this.stop();
                        if(this.endAniAction!=null){
                            this.endAniAction();
                             this.endAniAction=null;
                        }
                    }
                }
            }
        }

        public float getCurrentFrame(){
           return  this._time/Time.fixedDeltaTime;
        }

        // 清理资源
        public void Release()
        {
            _obj = null;
            if(ctrl!=null){
               ctrl.Release();
               ctrl = null;
            }
        }

    }

