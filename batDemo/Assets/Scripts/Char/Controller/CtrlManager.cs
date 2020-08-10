using UnityEngine;
using System.Collections.Generic;


//控制器管理者 那到控制器 做控制器能做的事情. 移动行走 攻击 摄像机旋转 游戏暂停 combo连招 控制 等
public class CtrlManager  : MonoSingleton<CtrlManager>
{
    private MultipleOnListPool<Controller> _ctrlPool;
     private  List<Controller> _ctrlsOnList;
    public void Init()
    {

       this._ctrlPool =new MultipleOnListPool<Controller>("CtrlPool");
       this._ctrlsOnList= this._ctrlPool.getOnList();
    }
    public Controller getController(GameEnum.CtrlType type) {
            Controller controller=null;
            switch(type){
                case GameEnum.CtrlType.JoyCtrl:
                    controller= this._ctrlPool.get<JoyController>("JoyController");
                break;
                case  GameEnum.CtrlType.AiCtrl:
                break;
                case  GameEnum.CtrlType.NetCtrl:
                break;
                case  GameEnum.CtrlType.keyBordCtrl:
                break;
            }
            return controller;
    }
    private void Update() {
     
    }

    
   
}
