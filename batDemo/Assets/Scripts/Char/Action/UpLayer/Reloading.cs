//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Reloading : ActionBase
{
       private float startTime=0f;
       private Player player;
        private Gun _Gun;   
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.Reloading;
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
     
            player.GetAniUpPart().Play(GameEnum.AniLabel.rifle_reload,0,0.5f,1,0,1);
            _Gun=player.weaponSystem.getActiveWeapon() as Gun;
            if(_Gun!=null&&player.charData.isMyPlayer){
                //UI派发事件
            }
          //  player.GetAniUpPart().endAniAction=toAction;

        }
         //动作更新;
        public override void Update(){
            base.Update();
            startTime+=GameSettings.Instance.deltaTime;
            if(_Gun.getGunData().CurrentMagzine>0&&player.charData.Btn_Fire){
                  player.doActionSkillByLabel(GameEnum.ActionLabel.Aiming);
                  return;
            }
            if(startTime>=_Gun.getGunData().ReloadTime){
                //换弹完毕
                _Gun.FillMagzin();
                this.cancelPriorityLimit=GameEnum.CancelPriority.Stand_Move_Null;
                if(player.charData.Btn_Aim){
                    player.doActionSkillByLabel(GameEnum.ActionLabel.Aiming);
                }else{
                     player.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle);
                }
            }
        }
           //new object[]{worldDir,isSprint,false,this.JoyAngle}
  
        private void toAction(){

        }
        /**
        * 切换动作 处理逻辑;
        */
        public override void executeSwichAction(){
       //      player.GetAniUpPart().endAniAction=null;
        }

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
            _Gun=null;
            base.onRecycle();
        }
        /*********
        销毁
        *******/
        public override void onRelease()
        {
             player=null;
              _Gun=null;
             base.onRelease();
        }
}

