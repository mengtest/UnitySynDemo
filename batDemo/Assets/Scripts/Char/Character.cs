using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Character : ObjBase
{
    private GameEnum.CtrlType _ctrlType=GameEnum.CtrlType.Null;
    protected Controller ctrl=null;
    
    public Character()
    {
        this.charType=GameEnum.ObjType.Character;
        this.needUpdate=false;
    }
    
    public GameEnum.CtrlType ctrlType   { 
        get{ 
            return _ctrlType;
        }
        set {
           if(_ctrlType!=value){
               _ctrlType=value;
               if(ctrl!=null){
                   ctrl.recycleSelf();
               }
               ctrl=CtrlManager.Instance.getController(this._ctrlType);
               if(ctrl!=null){
                 ctrl.init(this);
               }
           }       
        }
    }
    public override void onViewLoadFin(){
        objData = this.node.GetComponent<CharData>();
        if(objData==null){
            DebugLog.LogError("no charData mono ->",this.poolname);
        }else{
            objData.init(this,onFixUpdate);
        }
    }
    //mono 各自的updtate;
    public virtual void onFixUpdate() {
       if(this._move!=null){
            this._move.fixUpdate();
        }
    }
    //获取控制器.
    public Controller GetCtrl(){
        return this.ctrl;
    }

    //回收.
     public override void onRecycle(){
         _ctrlType=GameEnum.CtrlType.Null;
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
         }
        base.onRecycle();
     }
    public override void Release(){
         if(ctrl!=null){
             ctrl.recycleSelf();
             ctrl=null;
         }
        base.Release();
    }
}