using System.Collections.Generic;
using GameEnum;
using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Player : Character
{
    public AvatarSystem avatar;
    public WeaponSystem weaponSystem;

    private List<Item> canPickUpList;

    private float chekTime=0;
    public Player()
    {
        canPickUpList=new List<Item>();
         charType=GameEnum.ObjType.Player;
    }

    //重写显示.
    public override void init(){
         string[] split = poolname.Split('/');
        if(split.Length>0){
            this._name=split[split.Length-1];
            //+this.id;
        }else{
            this._name=poolname;
            //+"_"+this.id;
        }
        this.node.name=this._name;
        this.avatar = this.dataNode.AddComponent<AvatarSystem>();
        this.weaponSystem =this.dataNode.AddComponent<WeaponSystem>();
        this.weaponSystem.Init(this);
    }
     public override void initData(){
         base.initData();
         this.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,false);
     }
    //eg: initAvatar("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
    public void initAvatar(string aniUrl, string[] modelpaths){
        if(this.avatar==null)return; 
        this.avatar.Init(aniUrl,modelpaths,onBodyFin,this);
    }
    public override void ChangeNodeObj(GameObject obj,bool resetPos=true){
        base.ChangeNodeObj(obj,resetPos);
    }
    //换装.
    public void ChangePart(string partPath){
        if(this.avatar=null)return; 
        this.avatar.ChangePart(partPath);
    }
    public override void OnItemTrigger(Item item,bool isEnter=true){
        base.OnItemTrigger(item,isEnter);
        if(isEnter){
            canPickUpList.Add(item);
            if(canPickAction()){   
                if(checkPickUpItem(item)){
                    this.doActionSkillByLabel(ActionLabel.PickUp);
                }
            }
        }else{
            if(canPickUpList.Contains(item)){
               canPickUpList.Remove(item);
            }
        }
    }
    public bool checkPickUpItem(Item item){
        if(item.isWeapon){
                if(weaponSystem.checkCanAutoPickUpWeapon()){
                    return true;
                }
            }
        //若果是可以自动拾取的道具
        return false;
    }
    public bool canPickAction(){
         if(this.charData.currentBaseAction==ActionLabel.Dash)return false;
        if(this.charData.currentBaseAction==ActionLabel.PickUp)return false;
        return true;
    }
    public void checkPickUpNearItem(){
        if(!canPickAction())return;
        //每1秒检测一次就够了
        for (int i = 0; i < canPickUpList.Count; i++)
        {
            if(canPickUpList[i].isWeapon&&weaponSystem.checkCanAutoPickUpWeapon()){
                 this.doActionSkillByLabel(ActionLabel.PickUp);
            }
            //若果是可以自动拾取的道具
        }
    }
    protected override void fixUpdate() {
       base.fixUpdate();
       chekTime+=Time.fixedDeltaTime;
       if(chekTime>=1){
           chekTime=0;
           this.checkPickUpNearItem();
       }
    }
    public void EquipWeapon(Item item){
        if(canPickUpList.Contains(item)){
            canPickUpList.Remove(item);
        }
        weaponSystem.EquipWeapon(item as Weapon);
    }
    public void EquipNearWeapon(){
        for (int i = 0; i < canPickUpList.Count; i++)
        {
            if(canPickUpList[i].isWeapon){
                this.EquipWeapon(canPickUpList[i]);
                break;
            }
        }
    }
    public void PickUpNearItem(){
        if(canPickUpList.Count>0){
             this.doActionSkillByLabel(ActionLabel.PickUp);
        }
    }
    protected virtual void onBodyFin(){
        this.initViewFin=true;
        this.onViewLoadFin();
        //如果是观察者.
        CameraManager.Instance.cameraCtrl.init(gameObject.transform);
    } 
    public override void OnEvent(string cmd, object[] param=null){
         base.OnEvent(cmd,param);
    }
    
    protected override void initStateMachine(){
        m_FSM = new StateMachine<Character>(this);
        m_FSM.RegisterState(new Char_Idle(m_FSM));
        m_FSM.RegisterState(new Char_Squat(m_FSM));
        m_FSM.RegisterState(new Char_Prone(m_FSM));
        m_FSM.RegisterState(new Char_Skill(m_FSM));
        m_FSM.RegisterState(new Char_Hurt(m_FSM));
        m_FSM.RegisterState(new Char_HurtLie(m_FSM));
        m_FSM.RegisterState(new Char_HurtLinkBone(m_FSM)); //挂点中不可变羊.
        m_FSM.RegisterState(new Char_Polymorph(m_FSM));
        m_FSM.RegisterState(new Char_Dead(m_FSM));
    }
     public override void onGet(){
        base.onGet();
     }
    //回收.
     public override void onRecycle(){
         canPickUpList.Clear();
        base.onRecycle();
     }
    public override void onRelease(){
        canPickUpList.Clear();
        this.canPickUpList=null;
        this.avatar=null;
        this.weaponSystem=null;
        base.onRelease();
    }
}