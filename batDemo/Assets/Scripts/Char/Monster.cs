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
    public override void onViewLoadFin(){
          base.onViewLoadFin();
    }

    //回收.
     public override void onRecycle(){
        base.onRecycle();
     }
    public override void onRelease(){
        base.onRelease();
    }
}