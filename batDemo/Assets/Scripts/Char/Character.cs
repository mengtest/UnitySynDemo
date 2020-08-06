using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Character : ObjBase
{
    protected AvatarChar avatar;
    protected IController ctrl;
    public Character()
    {
       

    }
    public void initCtrl(){

    }
    //重写显示.
    public override void initView(string prefabPath=""){

        this.avatar= this.node.AddComponent<AvatarChar>();
        this.avatar.Init("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
        //this.poolname=prefabPath;
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
        this.avatar=null;
        base.Release();
    }
}