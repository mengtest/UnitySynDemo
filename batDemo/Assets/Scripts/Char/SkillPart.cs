//*************************************************************************
//	技能部件
//*************************************************************************
using System.Collections.Generic;
using UnityEngine;
 
    //技能部件
    public class SkillPart
    {   
        //baseLayer 动作
        public ActionBase currentBaseAction = null;
        //UpLayer 动作
        public ActionBase currentUpAction = null;
        //AddLayer 动作
        public ActionBase currentAddAction = null;
        private Character _char;
         
         //遥杆 或者 目标 要去的方向;
        public Vector3 targetDir=Vector3.zero;
        public Vector3 targetPos=Vector3.zero;
        public List<Vector3> targetPosList=null;
        //目标位置偏移
        public bool targetOffset;
        //受击 与攻击表现；
      //  public t_s_hitData hitdata;


         //需要检测降落点.
        public bool needCheckMapFixSkillPoint=false;
        // 初始化动画控制器
        public void Init(Character chars)
        {
            this._char=chars;
        }
        public void Update(){
            if (this.currentBaseAction != null) {
                this.currentBaseAction.Update();
            }
            if (this.currentUpAction != null) {
                this.currentUpAction.Update();
            }
            if (this.currentAddAction != null) {
                this.currentAddAction.Update();
            }
            //其他后续补...

        }
       /**
        * 
        * @param actionLabel 动作名称;
        * @param frame   从第几帧开始播放;
        * @param chkCencelLV  是否检测取消优先级
        * @param param  其他action中代表参数    CMAction 中 param:number previousFrame 代表 跳转到制定帧 连同开始帧一起计算，计算到当前帧逻辑。
        */
        public bool doActionSkillByLabel(string actionLabel,int frame = 0, bool chkCencelLV = true, object[] param = null, int skillID=0) {
            if (this._char.isRecycled || this._char.charData==null ) {
                return false;
            }
            int layer= ActionManager.instance.GetActionLayer(actionLabel);
            switch(layer){
                case GameEnum.ActionLayer.BaseLayer:
                    return this.doBaseLayerActionSkillByLabel(actionLabel,frame,chkCencelLV,param,skillID);
                case GameEnum.ActionLayer.UpLayer:
                    return  this.doUpLayerActionSkillByLabel(actionLabel,frame,chkCencelLV,param,skillID);
                case GameEnum.ActionLayer.AddLayer:
                    return this.doAddLayerActionSkillByLabel(actionLabel,frame,chkCencelLV,param,skillID);
                default:
                    return true;
            }
        }
        //基础动作.
        private bool doBaseLayerActionSkillByLabel(string actionLabel,int frame = 0, bool chkCencelLV = true, object[] param = null, int skillID=0) {
            if (this.currentBaseAction != null && this.currentBaseAction.poolname == actionLabel) {
                  this.currentBaseAction.Begin(frame, param);
                return true;
            }
            if (chkCencelLV && !this.chkCancelLvActionSkill(actionLabel)) {
                return false;
            }
            // if (skillID > 0 && this.currentBaseAction.skillActionId != this._char.charData.normalAttackId) {
            //     this._char.charData.useSkillState = true;
            // }

            ActionBase tempAction = ActionManager.instance.GetAction(actionLabel);

            if (this.currentBaseAction != null) {
                // if(this.currentBaseAction.skillActionId!=0 && this.currentBaseAction.skillActionId==this._char.charData.normalAttackId){
                //     this._char.charData.isAttacking=false;
                //     this._char.charData.mbManualAtk = false;
                // }
                this.currentBaseAction.executeSwichAction();
                this.currentBaseAction.recycleSelf();
                this.currentBaseAction = null;
            }
            this.currentBaseAction = tempAction;
            this.currentBaseAction.InitAction(this._char);
            if(skillID!=0){
                this.currentBaseAction.skillActionId=skillID;
            }
            this._char.charData.currentBaseActionType = this.currentBaseAction.actionType;
            this._char.charData.currentBaseAction = this.currentBaseAction.poolname;
          //  this._char.charData.hitIdList.clear();
            this.currentBaseAction.Begin(frame, param);

            return true;
        }
        private bool doUpLayerActionSkillByLabel(string actionLabel,int frame = 0, bool chkCencelLV = true, object[] param = null, int skillID=0) {
              
             if (this.currentUpAction != null && this.currentUpAction.poolname == actionLabel) {
                  this.currentUpAction.Begin(frame, param);
                return true;
            }
            if (chkCencelLV && !this.chkCancelLvActionSkill(actionLabel)) {
                return false;
            }

            ActionBase tempAction = ActionManager.instance.GetAction(actionLabel);

            if (this.currentUpAction != null) {
                this.currentUpAction.executeSwichAction();
                this.currentUpAction.recycleSelf();
                this.currentUpAction = null;
            }
            this.currentUpAction = tempAction;
            this.currentUpAction.InitAction(this._char);
            if(skillID!=0){
                this.currentUpAction.skillActionId=skillID;
            }
            this._char.charData.currentUpLayerActionType = this.currentUpAction.actionType;
            this._char.charData.currentUpLayerAction = this.currentUpAction.poolname;

            this.currentUpAction.Begin(frame, param);  

            return true;
        }
         private bool doAddLayerActionSkillByLabel(string actionLabel,int frame = 0, bool chkCencelLV = true, object[] param = null, int skillID=0) {
              
             if (this.currentAddAction != null && this.currentAddAction.poolname == actionLabel) {
                  this.currentAddAction.Begin(frame, param);
                return true;
            }
            if (chkCencelLV && !this.chkCancelLvActionSkill(actionLabel)) {
                return false;
            }

            ActionBase tempAction = ActionManager.instance.GetAction(actionLabel);

            if (this.currentAddAction != null) {
                this.currentAddAction.executeSwichAction();
                this.currentAddAction.recycleSelf();
                this.currentAddAction = null;
            }
            this.currentAddAction = tempAction;
            this.currentAddAction.InitAction(this._char);
            if(skillID!=0){
                this.currentAddAction.skillActionId=skillID;
            }
            this._char.charData.currentAddLayerActionType = this.currentAddAction.actionType;
            this._char.charData.currentAddLayerAction = this.currentAddAction.poolname;

            this.currentAddAction.Begin(frame, param);  

            return true;
        }
         /**
        * 检测动作取消优先级;
        * @param actionLabel 
        * @param linkAction 
        */
        public bool chkCancelLvActionSkill(string actionLabel) {
            return true;
            // if (this.currentAction == null) {
            //     return true;
            // }
            // //各种状态不能切换动作；
            // if (this._char.charData.noAction) {
            //     if (actionLabel == ActionLabel.Stand||actionLabel==ActionLabel.Dead||actionLabel==ActionLabel.DeadSummoner||actionLabel == ActionLabel.BackOff||actionLabel==ActionLabel.DeadMonster){
            //         return true;
            //     }
            //     return false
            // } else if (this._char.charData.isLink) {
            //     return false;
            // }
            // if (this._char.charData.currentActionType == 0) {
            //     return true;
            // }
            // //    console.log("currentAction.cancelPriorityLimit  ",this.currentAction.cancelPriorityLimit,this.currentAction.actionLabel);
            // if (this.currentAction.cancelPriorityLimit >= 0) {
            //     if (actionLabel != ActionLabel.Run && this.skillList.chkSkillisCding(actionLabel)) {
            //         return false;
            //     }
            //     let needrecycle: boolean = false;
            //     if (linkAction == null) {
            //         linkAction = ActionManager.Get().GetAction(actionLabel);
            //         needrecycle = true;
            //     }
            // //   console.log("linkAction.cancelPriorityLimit  ",linkAction.cancelPriorityLimit,linkAction.actionLabel);
            //     if (linkAction.cancelPriorityLimit > this.currentAction.cancelPriorityLimit) {
            //         if (needrecycle) {
            //             linkAction.recycleSelf();
            //         } else {
            //             linkAction = null;
            //         }
            //         return true;
            //     }
            //     if (needrecycle) {
            //         linkAction.recycleSelf();
            //     } else {
            //         linkAction = null;
            //     }
            // }
            // return false;
        }
        public void clearAction(bool switchAction=false) {
           if (this.currentBaseAction != null) {
                if(switchAction){
                    this.currentBaseAction.executeSwichAction();
                }
                this.currentBaseAction.recycleSelf();
                this.currentBaseAction = null;
            }
            if (this.currentUpAction != null) {
                if(switchAction){
                    this.currentUpAction.executeSwichAction();
                }
                this.currentUpAction.recycleSelf();
                this.currentUpAction = null;
            }
            if (this.currentAddAction != null) {
                if(switchAction){
                    this.currentAddAction.executeSwichAction();
                }
                this.currentAddAction.recycleSelf();
                this.currentAddAction = null;
            }
            this.needCheckMapFixSkillPoint=false;
        }
       //重置.
        public void Reset(){
         //   this.hitdata = null;
         //   this.skillList.reSet();
            this.targetOffset = false;
            this.clearAction();
        }
        // 清理资源
        public void Release()
        {
            _char = null;
        }

    }

