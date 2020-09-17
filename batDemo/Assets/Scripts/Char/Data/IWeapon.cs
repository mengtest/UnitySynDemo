


//道具接口 这是个mono脚本的接口 碰撞在mono上 
using System;
using GameEnum;

public interface IItemData {
     void init(Item items);
     //触发半径
     ItemType getItemType();

     //关闭触发器; 开启box
     void OnPickUp();
     //开启box 
     void OnDrop();
     //掉落到地上 开启触发器 关闭box;
     void OnGround();
     //获得物品高度.
     float getHeight();
}
