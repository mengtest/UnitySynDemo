using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色数据..同步数据也写在这
public class ObjData : MonoBehaviour,IData
{
    private ObjBase _obj;
    private Action _onUpdate; 
    
    public string currentAction;

    public float PlaySpeed { get ; set ; }

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
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDestroy() {
        _obj=null;
        _onUpdate=null;
    }
}
