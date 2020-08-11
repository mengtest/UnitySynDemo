using UnityEngine;
/****
怪物基类
****/
[AutoRegistLua]
public class Monster : Character
{
    public Monster()
    {
        charType=GameEnum.ObjType.Monster;

    }
    //重写显示.
    public override void initView(string prefabPath=""){
          base.initView(prefabPath);
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