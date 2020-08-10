using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Player : Character
{
    protected AvatarChar avatar;
    public Player()
    {
       

    }

    //重写显示.
    public override void init(){
        this.avatar= this.node.AddComponent<AvatarChar>();
    //    this.avatar.Init("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
    }


    //回收.
     public override void onRecycle(){
        base.onRecycle();
     }
    public override void Release(){
        this.avatar=null;
        base.Release();
    }
}