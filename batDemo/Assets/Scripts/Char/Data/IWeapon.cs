


//道具接口
using System;
using GameEnum;

public interface IItemData {
     void init(Item items);
     //触发半径
     ItemType getItemType();
}
