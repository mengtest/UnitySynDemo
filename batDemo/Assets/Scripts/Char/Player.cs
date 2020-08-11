using UnityEngine;
/****
角色基类
****/
[AutoRegistLua]
public class Player : Character
{
    public AvatarChar avatar;
    public Player()
    {
         charType=GameEnum.ObjType.Player;

    }

    //重写显示.
    public override void init(){
        this.avatar = this.node.AddComponent<AvatarChar>();
        this.onViewLoadFin();
    }

    //eg: initAvatar("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
    public void initAvatar(string aniUrl, string[] modelpaths){
        if(this.avatar=null)return; 
        this.avatar.Init(aniUrl,modelpaths);
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