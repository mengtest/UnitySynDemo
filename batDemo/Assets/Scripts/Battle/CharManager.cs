using UnityEngine;
using System.Collections.Generic;

public class CharManager  : MonoSingleton<CharManager>
{
    public string str="";
    private  List<Character> _charOnList;
    private MultipleOnListPool<Character> _characterPool;
    public MultipleOnListPool<Character> CharPool {
         get{
             return this._characterPool;
         }
    }
    public void Init()
    {
       this._characterPool =new MultipleOnListPool<Character>("CharacterPool");
       this._charOnList= this._characterPool.getOnList();
       
    }
    public Character CreatCharacter(GameEnum.ObjType objType=GameEnum.ObjType.Character,string path="",GameEnum.CtrlType ctrlType=GameEnum.CtrlType.Null){
        Character chars=null; 
        switch(objType){
            case GameEnum.ObjType.Character:
                chars=this._characterPool.get<Character>(path);
            break;
            case GameEnum.ObjType.Monster:
                 chars=this._characterPool.get<Monster>(path);
            break;
            case GameEnum.ObjType.Player:
                chars=this._characterPool.get<Player>(path);
            break;
        }
        if(ctrlType!=GameEnum.CtrlType.Null){
            chars.ctrlType=ctrlType;
        }
        return chars;
    }
    private void FixedUpdate() {
        //  for (int i = 0; i < _charOnList.Count; i++)
        //  {
        //      if( _charOnList[i].needUpdate){
        //         _charOnList[i].fixUpdate();
        //      }
        //  }
    }
    private void Update() {
     
    }
    public void RecycleAll(){
        this._characterPool.recycleAll();
    }
    public void ClearAll(){
        this._characterPool.clearAll();
    }
   
}
