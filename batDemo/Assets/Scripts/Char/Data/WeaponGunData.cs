using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//武器数据..同步数据也写在这
public class WeaponGunData : MonoBehaviour,IData
{
    private ObjBase _obj;
    private Action _onFixUpdate; 

    public float PlaySpeed { get ; set ; }

    private Weapon_Gun _Gun;

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
         this._Gun=_obj.gameObject.GetComponent<Weapon_Gun>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDestroy() {
        _obj=null;
        _onFixUpdate=null;
    }
}
