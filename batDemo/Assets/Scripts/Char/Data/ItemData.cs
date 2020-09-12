using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//武器数据..同步数据也写在这
public class ItemData : MonoBehaviour,IData
{
    private ObjBase _obj;
    private Action _onFixUpdate; 

    public float PlaySpeed { get ; set ; }

    private IItemData _Data;

    // Start is called before the first frame update
    void Start()
    {
         PlaySpeed=1;
    }
    private void FixedUpdate() {
         if(_onFixUpdate!=null){
             this._onFixUpdate();
         }
    }
    public void init(ObjBase obj,Action onFixUpdate){
         _obj=obj;
         _onFixUpdate=onFixUpdate;
         initGunData();
    }
    /*****
    **
       Gun类型  obj.poolname=Weapon/Gun/AR/M4A1   path=Data/GunData/M4A1
       Melee类型  obj.poolname=Weapon/Melee/Knife/Knife01   path=Data/MeleeData/Knife01

    **
    *****/
    public void initGunData(){
         
         this._Data=_obj.gameObject.GetComponent<Weapon_Gun>();
         if(this._Data==null){
              this._Data=_obj.gameObject.AddComponent<Weapon_Gun>();
            // DebugLog.LogError("WeaponGunData >>> Weapon_Gun null",_obj.gameObject.name,_obj.id);
         }
         
    }
    public Weapon_Gun getGunData(){
      return this._Data as Weapon_Gun;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDestroy() {
        _obj=null;
        _onFixUpdate=null;
        _Data=null;
    }
}
