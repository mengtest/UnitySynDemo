using UnityEngine;
using System.Collections.Generic;

public class ObjManager  : MonoSingleton<ObjManager>
{
    public string str="";
    private  List<Character> _charOnList;
    private MultipleOnListPool<Character> _characterPool;
    public MultipleOnListPool<Character> CharPool {
         get{
             return this._characterPool;
         }
    }
     private MultipleOnListPool<Weapon> _weaponPool;
    public MultipleOnListPool<Weapon> WeaponPool {
         get{
             return this._weaponPool;
         }
    }
    private  List<Weapon> _weaponOnList;
    public void Init()
    {
       this._characterPool =new MultipleOnListPool<Character>("CharacterPool");
       this._charOnList= this._characterPool.getOnList();
        this._weaponPool =new MultipleOnListPool<Weapon>("WeaponPool");
       this._weaponOnList= this._weaponPool.getOnList();
       
    }
    public Character CreatCharacter(string path="",GameObject obj=null,GameEnum.ObjType objType=GameEnum.ObjType.Player,GameEnum.CtrlType ctrlType=GameEnum.CtrlType.JoyCtrl){
        Character chars=null; 
        switch(objType){
            case GameEnum.ObjType.Character:
                chars=this._characterPool.get<Character>(path);
                if(obj!=null){
                   chars.initView(obj);
                }else{
                   chars.initView();
                }
            break;
            case GameEnum.ObjType.Monster:
                chars=this._characterPool.get<Monster>(path);
                if(obj!=null){
                   chars.initView(obj);
                }else{
                   chars.initView();
                }
            break;
            case GameEnum.ObjType.Player:
                chars=this._characterPool.get<Player>(path);
                if(obj!=null){
                   chars.initView(obj);
                }
            break;
        }
        if(ctrlType!=GameEnum.CtrlType.Null){
            chars.ctrlType=ctrlType;
        }
        return chars;
    }
    //path=Data/GunData/M4A1
    public Weapon CreatWeapon(string path="",GameObject obj=null){
        Weapon weapon=this._weaponPool.get<Weapon>(path);
        if(obj!=null){
            weapon.initView(obj);
        }else{
            weapon.initView();
        }
        return weapon;
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
        this._weaponPool.recycleAll();
    }
    public void ClearAll(){
        this._characterPool.clearAll();
        this._weaponPool.recycleAll();
    }
   
}
