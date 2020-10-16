using UnityEngine;
using System.Collections.Generic;
using GameEnum;

public class ObjManager  : MonoSingleton<ObjManager>
{
    public string str="";

    ///角色池;
    private  List<Character> _charOnList;
    private MultipleOnListPool<Character> _characterPool;
    public MultipleOnListPool<Character> CharPool {
         get{
             return this._characterPool;
         }
    }
    
    ///武器池
     private MultipleOnListPool<Weapon> _weaponPool;
    public MultipleOnListPool<Weapon> WeaponPool {
         get{
             return this._weaponPool;
         }
    }
    private  List<Weapon> _weaponOnList;

     ///其他显示对象   子弹
    private MultipleOnListPool<ObjBase> _objPool;
    public MultipleOnListPool<ObjBase> ObjPool {
         get{
             return this._objPool;
         }
    }
    private  List<ObjBase> _objOnList;


     ///特效池
    private MultipleOnListPool<EffectBase> _effectPool;
    public MultipleOnListPool<EffectBase> EffectPool {
         get{
             return this._effectPool;
         }
    }
    private  List<EffectBase> _effectOnList;


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
       this._effectPool =new MultipleOnListPool<EffectBase>("EffectPool");
       this._effectOnList= this._effectPool.getOnList();
       this._objPool =new MultipleOnListPool<ObjBase>("ObjPool");
       this._objOnList= this._objPool.getOnList();

    }
    //通过受击部位获得 受击本体.
    public Character GetCharacterByBody(GameObject body){
       Character character=null;
       CharacterController cc= body.transform.GetComponentInParent<CharacterController>();
       if(cc!=null){
            for (int i = 0; i < this._charOnList.Count; i++)
            {
                character=this._charOnList[i];
                if(character.gameObject==cc.gameObject){
                    return character;
                }

            }
       }
       return character;
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
    public ObjBase CreatObjBase(string path="",GameObject obj=null,GameEnum.ObjType objType = ObjType.Bullet){
        ObjBase objBase=null;
        switch(objType){
            case ObjType.Bullet:
                objBase=this._objPool.get<Bullet>(path);
            break;
            case ObjType.Obj:
            default:
                objBase=this._objPool.get<ObjBase>(path);
            break;
        }
        if(obj!=null){
            objBase.initView(obj);
        }else{
            objBase.initView();
        }
        objBase.initData();
        return objBase;
    }
    public EffectBase CreatEffect(string path="",GameObject obj=null,float delayRecycleTime=0,float autoDeatoryTime=0, GameEnum.EffectType type=GameEnum.EffectType.Effect){
        EffectBase effect=null;
        switch(type){
            case GameEnum.EffectType.Effect:
              effect=this._effectPool.get<EffectBase>(path);
            break;
            case GameEnum.EffectType.PsEffect:
               effect=this._effectPool.get<PsEffect>(path);
            break;
        }
        effect.delayRecycleTime=delayRecycleTime;
        if(autoDeatoryTime>0){
           effect.autoDestroy(autoDeatoryTime);
        }
        if(obj!=null){
            effect.initView(obj);
        }else{
            effect.initView();
        }
        return effect;
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
