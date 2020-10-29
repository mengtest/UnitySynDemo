using System.Collections.Generic;
using GameEnum;
using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Player : Character
{
    protected Controller ctrl=null;
    public CameraCtrl cameraCtrl;
    public AvatarSystem avatar;
    public WeaponSystem weaponSystem;

    private List<Item> canPickUpList;

    private float chekTime=0;
    public Player()
    {
        canPickUpList=new List<Item>();
         charType=GameEnum.ObjType.Player;
    }
    public CtrlType ctrlType{ 
        get{ 
            return charData.ctrlType;
        }
        set {
           if(charData.ctrlType!=value){
               charData.ctrlType=value;
               if(ctrl!=null){
                   ctrl.recycleSelf();
               }
               ctrl=CtrlManager.Instance.getController(charData.ctrlType);
               if(ctrl!=null){
                 ctrl.init(this);
               }
           }       
        }
    }
    //获取控制器.
    public Controller GetCtrl(){
        return this.ctrl;
    }
    public bool isCamTarget(){
         return cameraCtrl.isCamTarget();
    }
    public void CameraFocus(Camera camera){
        cameraCtrl.setMainCamera(camera);
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
         initCameraCtrl();
         this.doActionSkillByLabel(GameEnum.ActionLabel.Stand,0,false);
         this.doActionSkillByLabel(GameEnum.ActionLabel.UpIdle,0,false);
     }

     public void initCameraCtrl(){
         if(cameraCtrl==null){
            GameObject  cam =new GameObject(this._name+"_Cam");
            cameraCtrl = cam.AddComponent<CameraCtrl>();
            cameraCtrl.target=this.gameObject.transform;
            cameraCtrl.init(this);
         }
     }
     public override void ChangeNodeObj(GameObject obj,bool resetPos=true){
        base.ChangeNodeObj(obj,resetPos);
        if(cameraCtrl!=null){
          cameraCtrl.init(this);
        }
    }

    public void SetCharacterCtrlHeight(float height){
        if( this.characterController){
             this.characterController.center=new Vector3(0,height/2f,0);
             this.characterController.height=height;
        }
        cameraCtrl.targetFocusHeight=height;  
    }


    //eg: initAvatar("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
    public void initAvatar(string aniUrl, string[] modelpaths){
        if(this.avatar==null)return; 
        this.avatar.Init(aniUrl,modelpaths,onBodyFin,this);
    }
    //换装.
    public void ChangePart(string partPath){
        if(this.avatar=null)return; 
        this.avatar.ChangePart(partPath);
    }
    public override void OnItemTrigger(Item item,bool isEnter=true){
        base.OnItemTrigger(item,isEnter);
        if(isEnter){
            if(!canPickUpList.Contains(item)){
               canPickUpList.Add(item);
            }
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
    protected override void Update() {
       base.Update();
       chekTime+=GameSettings.Instance.deltaTime;
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
        if(this.cameraCtrl){
             this.cameraCtrl.gameObject.SetActive(true);
        }
        base.onGet();
     }
    //回收.
     public override void onRecycle(){
         canPickUpList.Clear();
         if(this.cameraCtrl){
             this.cameraCtrl.gameObject.SetActive(false);
         }
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
         }
        base.onRecycle();
     }
    public override void onRelease(){
        if(this.cameraCtrl){
            GameObject.Destroy(this.cameraCtrl.gameObject);
            this.cameraCtrl=null;
        }
        if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
        }
        canPickUpList.Clear();
        this.canPickUpList=null;
        this.avatar=null;
        this.weaponSystem=null;
        base.onRelease();
    }
}