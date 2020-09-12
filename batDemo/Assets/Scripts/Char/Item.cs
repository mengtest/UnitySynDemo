using GameEnum;
using UnityEngine;
/****
武器基类
****/
public class Item : ObjBase
{
    public ItemData itemData=null;
    public Item()
    {
         charType=GameEnum.ObjType.Item;
    }

    //重写Data.
    public override void initData(){
        ItemData oldData=this.dataNode.GetComponent<ItemData>();
        if(oldData!=null){
            GameObject.DestroyImmediate(oldData);
        }
        this.itemData = this.dataNode.AddComponent<ItemData>();
        this.objData=this.itemData;
        this.itemData.init(this,fixUpdate);
    }
    public override void onViewLoadFin(){
        
    }

    public override void onGet(){
        base.onGet();
     }
    //回收.
    public override void onRecycle(){
        base.onRecycle();
     }
    public override void onRelease(){
        this.itemData=null;
        base.onRelease();
    }
}