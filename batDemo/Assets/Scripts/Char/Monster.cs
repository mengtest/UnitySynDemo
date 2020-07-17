using UnityEngine;
/****
怪物基类
****/
public class Monster : Character
{
    public Monster()
    {
       

    }
    //重写显示.
    public override void initView(string prefabPath=""){

        this.avatar= this.node.AddComponent<AvatarChar>();
        this.avatar.Init("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
        //this.poolname=prefabPath;
    }

    //回收.
     public override void onRecycle(){
        base.onRecycle();
     }
    public override void Release(){
        base.Release();
    }
}