using UnityEngine;
using System.Collections.Generic;
using GameEnum;

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

    private static Player _Myplayer;
    public  static void setMyPlayer(Player player=null){
        if(_Myplayer!=null){
            _Myplayer.charData.isMyPlayer=false;
        }
         _Myplayer=player;
         _Myplayer.charData.isMyPlayer=true;
    }
    public  static Player MyPlayer{
      get{
            return _Myplayer;
      }  
    }

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
        chars.initData();
        if(ctrlType!=GameEnum.CtrlType.Null){
            chars.ctrlType=ctrlType;
        }
        return chars;
    }
    //方便新枪 测试 丢地图 加脚本就可以跑了.
    public Gun CreatGun(Weapon_Gun gunMono){
       return CreatWeapon(gunMono.getGunPath(),gunMono.gameObject) as Gun;
    }
    //path=Data/GunData/M4A1
    public Weapon CreatWeapon(string path="",GameObject obj=null,ItemType itemType =ItemType.Gun){
        Weapon weapon=null;
        switch(itemType){
        case ItemType.Gun:
        case ItemType.PistolGun:
            weapon=this._weaponPool.get<Gun>(path);
        break;
        default:
                weapon=this._weaponPool.get<Weapon>(path);
        break;
        }
        if(obj!=null){
            weapon.initView(obj);
        }else{
            weapon.initView();
        }
        weapon.initData();
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
