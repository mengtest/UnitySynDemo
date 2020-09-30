using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//武器数据..同步数据也写在这
[AutoRegistLua]
public class ItemData : MonoBehaviour,IData
{
    private Item _obj;
    private Action _onUpdate; 

    public float PlaySpeed { get ; set ; }

    private IItemData _Data;

    public bool ItemOnHand=false;

    public bool defaultFull=true;

    // Start is called before the first frame update
    void Start()
    {
         PlaySpeed=1;
    }
    private void FixedUpdate() {
         if(_onUpdate!=null){
             this._onUpdate();
         }
    }
    public void init(ObjBase obj,Action onUpdate=null,Action onLateUpdate=null){
         _obj=obj as Item;
         _onUpdate=onUpdate;
         ItemOnHand=false;
         initItemData();
    }
    /*****
    **
       Gun类型  obj.poolname=Gun/AR/M4A1    path=Data/GunData/M4A1
       Melee类型  obj.poolname=Melee/Knife/Knife01   path=Data/MeleeData/Knife01

    **
    *****/
    public void initItemData(){
         string[] split = _obj.poolname.Split('/');
         if(_obj.poolname.StartsWith("Gun")){
            this._Data=_obj.gameObject.GetComponent<Weapon_Gun>();
            if(this._Data==null){
                Weapon_Gun weapon_Gun=_obj.gameObject.AddComponent<Weapon_Gun>();
                this._Data=weapon_Gun;
                weapon_Gun.LoadData_u3d(_obj.poolname);
                // DebugLog.LogError("WeaponGunData >>> Weapon_Gun null",_obj.gameObject.name,_obj.id);
            }else{
                if(this.defaultFull){
                   (this._Data as Weapon_Gun).FillMagzine();
                }
            }
            this._Data.init(_obj);
            _obj.isWeapon=true;
         }else if(_obj.poolname.StartsWith("Item")){
          //Item
          _obj.isWeapon=false;
         }else if(_obj.poolname.StartsWith("Melee")){
           //近战武器.
              _obj.isWeapon=true;
         }
    }
    public Weapon_Gun getGunData(){
      return this._Data as Weapon_Gun;
    }
    public void OnPickUp(){
      this._Data.OnPickUp();
    }
    public void OnDrop(){
       this._Data.OnDrop();
    }
    public void OnGround(){
       this._Data.OnGround();
    }
    public GameEnum.ItemType getItemType(){
        return this._Data.getItemType();
    }
    //物品高度.
    public float getHeight(){
        return _Data.getHeight();
    }
    public void OnDestroy() {
        _obj=null;
        _onUpdate=null;
        _Data=null;
    }
}
