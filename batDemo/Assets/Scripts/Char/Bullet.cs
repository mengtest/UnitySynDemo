using GameEnum;
using UnityEngine;
/****
子弹
****/
[AutoRegistLua]
public class Bullet : ObjBase
{
    //baseLayer 动作

    public Bullet()
    {
         charType=GameEnum.ObjType.Bullet;
    }
    //移动专用方法.
    public override void OnMove(Vector3 dic){
        //每帧打个射线.
       this.node.transform.position =  this.node.transform.position + dic;
    }
    protected override void Update(){
        base.Update();
    }

    public override void onRecycle(){
        base.onRecycle();
    }
    public override void onRelease(){
        base.onRelease();
    }
}