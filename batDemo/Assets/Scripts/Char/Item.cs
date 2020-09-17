﻿using GameEnum;
using UnityEngine;
/****
武器基类
****/
[AutoRegistLua]
public class Item : ActionObj
{
    public ItemData itemData=null;
    private float _height;
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
        _height=itemData.getHeight()+0.3f;
         this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDefault);
    }
    public override void onViewLoadFin(){
    }
    public virtual void OnPickUp(Player player){
        //带在身上;
         gameObject.transform.parent=player.gameObject.transform;
         gameObject.SetActive(false);
        itemData.OnPickUp();
    }
    public virtual void DropItem(){
       gameObject.transform.parent=null;
       gameObject.SetActive(true);
       itemData.OnDrop();
       //给它向上力 让它掉出来;
       this.doActionSkillByLabel(GameEnum.ActionLabel.ItemDrop);
    }
    public virtual void OnGround(){
        itemData.OnGround();
    }
    public override bool IsGrounded() 
	{
		return Physics.Raycast(this.gameObject.transform.position+Vector3.up*0.1f, Vector3.down, _height,LayerHelper.GetGroundLayerMask());
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