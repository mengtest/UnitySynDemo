using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色数据..同步数据也写在这
public class ObjData : MonoBehaviour,IData
{
    private ObjBase _obj;
    private Action _onFixUpdate; 

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate() {
         if(_onFixUpdate!=null){
             this._onFixUpdate();
         }
    }
    public void init(ObjBase obj,Action onFixUpdate){
         _obj=obj;
         _onFixUpdate=onFixUpdate;
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
