//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class ChangeWeapon : ActionBase
{
       private float startTime=0f;
       private Player player;
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.ChangeWeapon;
            this.actionLayer=GameEnum.ActionLayer.UpLayer;
            this.defultPriority=GameEnum.CancelPriority.BaseAction;
        }

         // 初始化动作.
        public override void InitAction(ObjBase obj)
        {
            base.InitAction(obj);
            player= (this.obj as Player);
        }
         //动作进入
        public override void GotoFrame(int frame=0,object[] param=null){
             this.currentFrame = frame;
             player.GetAniUpPart().Play(GameEnum.AniLabel.ChangeWeapon,0,0.5f,1f,0.2f,1);
             
        }
         //动作更新;
        public override void Update(){
            base.Update();
            startTime+=Time.fixedDeltaTime;
            if(startTime>=0.5f){
                if(player.charData.Btn_Aim){
                    this.cancelPriorityLimit=GameEnum.CancelPriority.Stand_Move_Null;
                    player.doActionSkillByLabel(GameEnum.ActionLabel.Aiming);
                }else{
                     player.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle);
                }
            }
        }
           //new object[]{worldDir,isSprint,false,this.JoyAngle}
  

        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
            
        }

        //动作 需要改成3种分支 base up add
        public override void onGet()
        {
            startTime=0;
             base.onGet();
        }
        /**
        回收..
        **/
        public override void onRecycle()
        {
            player=null;
            base.onRecycle();
        }
        /*********
        销毁
        *******/
        public override void onRelease()
        {
             player=null;
             base.onRelease();
        }
}

