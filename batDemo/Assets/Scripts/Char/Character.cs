using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Character : ObjBase
{
    private GameEnum.CtrlType _ctrlType;
    protected Controller ctrl;
    public Character()
    {
       

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
               ctrl.init(this);
           }       
        }
    }
    //获取控制器.
    public Controller GetCtrl(){
        return this.ctrl;
    }

    //回收.
     public override void onRecycle(){
         if(ctrl!=null){
             ctrl.Release();
             ctrl=null;
         }
        base.onRecycle();
     }
    public override void Release(){
         if(ctrl!=null){
             ctrl.Release();
             ctrl=null;
         }
        base.Release();
    }
}