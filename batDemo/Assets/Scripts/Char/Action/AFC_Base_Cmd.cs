//*************************************************************************
//	技能部件
//*************************************************************************

//技能部件
using UnityEngine;

    public class AFC_Base_Cmd
    {

        public bool executed = false;
        //只运行一次
        public bool onceEvent;

        protected AFC_Base_Data afcData;

        protected CmdAction action;
        private int _frameId = 0;
        private int _maxFrame = 1;
        //如果有 需要 被意外终止，自然终止  需要执行的方法 为true;
        public bool hasSwitchFun = false;
        //需要更新;
        public bool needUpdate = false;
        public bool updateFinished = false;

        public  AFC_Base_Cmd()
        {
            
        }
        /**
        * 初始化数据; 创建显示对象;
        */
        public void init(AFC_Base_Data afcData,CmdAction action) {
            this.afcData = afcData;
            this.action = action;
            this._frameId = Mathf.RoundToInt (this.afcData.time / GameSettings.Instance.deltaTime);
            if (this.afcData.isDuration) {
                this.needUpdate = true;
                this._maxFrame = this._frameId +  Mathf.RoundToInt (this.afcData.length / GameSettings.Instance.deltaTime);
            } else {
                this.needUpdate = false;
                this._maxFrame = this._frameId + 1;
            }
            this.onceEvent = this.afcData.onceEvent;

            if (this.afcData.BoolAbb["onActinCancel"]) {
                this.hasSwitchFun = true;
            }
            //    cc.log("AFC FrameId:",this.afcData.eventName,this._frameId);
        }
        public virtual void onInit(){

        }
        public int FrameId() {
           return this._frameId;
        }
        public int MaxFrame() {
            return this._maxFrame;
        }
        /// <summary>
        /// 命令执行   需要重写。。。。
        /// </summary>
        public virtual void execute(){

        }



        /**
        * 命令更新  需要重写。。。。
        * @param idx 当前帧
        */
        public void update(int curFrame) {
            //判断在 FrameId~ MaxFrame 之间执行。
        }
        //被意外终止，自然终止  需要执行的方法;
        public void switchCmd() {
            this.updateFinished = true;
        }

        /**
        * 重置。
        */
        public  void Reset () {
            this.updateFinished = false;
            this.executed = false;
        }
        /**
        *回收; 
        **/
        public  void Release() {
            this.afcData = null;
            this.action = null;
        }
      

    }

