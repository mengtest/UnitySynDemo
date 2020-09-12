using GameEnum;
using UnityEngine;
/****
武器基类
****/
public class Weapon : Item
{
    public Weapon()
    {
         charType=GameEnum.ObjType.Weapon;
    }

    //重写Data.
    public override void initData(){
      base.initData();

    }
    public override void onViewLoadFin(){
        base.onViewLoadFin();
    }

    public override void onGet(){
        base.onGet();
     }
    //回收.
    public override void onRecycle(){
        base.onRecycle();
     }
    public override void onRelease(){
        base.onRelease();
    }
}