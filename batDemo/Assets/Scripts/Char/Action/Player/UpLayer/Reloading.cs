//*************************************************************************
//动作
//*************************************************************************

//动作
using UnityEngine;

public class Reloading : ActionBase
{
       private float reloadTime=0f;
       private Player player;
        private Gun _Gun;   
        //单次创建.
        public override void init(){
            this.name=GameEnum.ActionLabel.Reloading;
            this.actionLayer=GameEnum.ActionLayer.UpLayer;
            this.actionType=1;
            this.defultPriority=GameEnum.CancelPriority.NormalAction;
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
            _Gun=player.weaponSystem.getActiveWeapon() as Gun;
            if(_Gun.getGunData().CurrentMagzine>0){
                reloadTime=_Gun.getGunData().BattleReloadTime;
            }else{
                 reloadTime=_Gun.getGunData().ReloadTime;
            }
            switch(_Gun.getGunType()){
                 //weapon_gun.AimTime/2f   想让他快一点抬手/1.2F 
                 case GameEnum.Weapon_GunType.AR:
                 //精准步枪
                 case GameEnum.Weapon_GunType.DMR:
                 //狙击枪
                 case GameEnum.Weapon_GunType.SR:
                 //轻机枪
                 case GameEnum.Weapon_GunType.LMG:
                 //冲锋枪
                 case GameEnum.Weapon_GunType.SMG:
                 //散弹枪  喷子
                 case GameEnum.Weapon_GunType.ShotGun:
                     player.GetAniUpPart().Play(GameEnum.AniLabel.rifle_reload,0,2.667f,2.667f/reloadTime,0,1);
                 break;
                 case GameEnum.Weapon_GunType.Pistol:
                     player.GetAniUpPart().Play(GameEnum.AniLabel.pistol_reload,0,2.667f,2.667f/reloadTime,0,1);
                 break;
             }
            if(_Gun!=null&&player.charData.isMyPlayer){
                //UI派发事件
                EventCenter.send(SystemEvent.KEY_INPUT_ONRELOAD_STATE,new object[]{reloadTime},true);
            }
          //  player.GetAniUpPart().endAniAction=toAction;
        }
         //动作更新;
        public override void Update(){
            base.Update();
            reloadTime-=GameSettings.Instance.deltaTime;
            if(_Gun.getGunData().CurrentMagzine>0&&player.charData.Btn_Fire){
                   this.defultPriority=GameEnum.CancelPriority.Stand_Move_Null;
                  player.doActionSkillByLabel(GameEnum.ActionLabel.Aiming);
                  return;
            }
            if(reloadTime<=0){
                //换弹完毕
                _Gun.FillMagzin();
                this.cancelPriorityLimit=GameEnum.CancelPriority.Stand_Move_Null;
                if(player.charData.Btn_Aim||player.charData.Btn_Fire){
        //            DebugLog.Log("aiming");
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
            if(_Gun!=null&&player.charData.isMyPlayer){
                EventCenter.send(SystemEvent.KEY_INPUT_ONRELOAD_STATE,new object[]{0},true);
            }
       //      player.GetAniUpPart().endAniAction=null;
        }

        public override void onGet()
        {
            reloadTime=0;
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

