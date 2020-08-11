using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色数据..同步数据也写在这
public class CharData : MonoBehaviour,IData
{

    public Character _char=null;
    private Action _onFixUpdate; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void init(ObjBase obj,Action onFixUpdate){
          _char=obj  as Character;
           _onFixUpdate=onFixUpdate;
    }
    private void FixedUpdate() {
         if(_onFixUpdate!=null){
             this._onFixUpdate();
         }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy() {
        _char=null;
        _onFixUpdate=null;
    }

}
