using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//武器数据..同步数据也写在这
public class WeaponData : MonoBehaviour,IData
{
    private ObjBase _obj;
    private Action _onUpdate; 

    public float PlaySpeed { get ; set ; }

    private IItemData _Data;

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
         _obj=obj;
         _onUpdate=onUpdate;
         initGunData();
    }
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
        _onUpdate=null;
        _Data=null;
    }
}
