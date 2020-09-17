using GameEnum;
using UnityEngine;
/****
带动作的显示类
****/
[AutoRegistLua]
public class ActionObj : ObjBase
{
    //baseLayer 动作
    public ActionBase currentBaseAction = null;

    public ActionObj()
    {
         charType=GameEnum.ObjType.Obj;
    }
     public override bool doActionSkillByLabel(string actionLabel ,int frame=0,bool chkCancelLv=true,object[] param=null,int skillID=0){
            if (this.currentBaseAction != null && this.currentBaseAction.poolname == actionLabel) {
                  this.currentBaseAction.Begin(frame, param);
                return true;
            }
            //单动作 轨道 不需要那么复杂的取消动作.
            // if (chkCencelLV && !this.chkCancelLvActionSkill(actionLabel)) {
            //     return false;
            // }
            ActionBase tempAction = ActionManager.instance.GetAction(actionLabel);

            if (this.currentBaseAction != null) {
                this.currentBaseAction.executeSwichAction();
                this.currentBaseAction.recycleSelf();
                this.currentBaseAction = null;
            }
            this.currentBaseAction = tempAction;
            this.currentBaseAction.InitAction(this);
            if(skillID!=0){
                this.currentBaseAction.skillActionId=skillID;
            }
     //       (this.objData as ObjData).currentAction = this.currentBaseAction.poolname;
          //  this._char.charData.hitIdList.clear();
            this.currentBaseAction.Begin(frame, param);
            return true;
    }
     protected override void fixUpdate(){
        if (this.currentBaseAction != null) {
            this.currentBaseAction.Update();
        }
        base.fixUpdate();
    }
    public void clearAction(bool switchAction=false) {
        if (this.currentBaseAction != null) {
            if(switchAction){
                this.currentBaseAction.executeSwichAction();
            }
            this.currentBaseAction.recycleSelf();
            this.currentBaseAction = null;
        }
    }
    public override void onRecycle(){
        clearAction();
        base.onRecycle();
    }
    public override void onRelease(){
        clearAction();
        base.onRelease();
    }
}