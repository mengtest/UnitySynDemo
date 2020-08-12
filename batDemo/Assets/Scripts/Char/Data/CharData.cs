using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色数据..同步数据也写在这
public class CharData : MonoBehaviour,IData
{

    public Character _char=null;
    private Action _onFixUpdate; 
    
    //状态属性.......................
    public bool isLie = false;
    public bool isStandUp = false;
    public bool lowFLy=false;
    public bool isNumb = false;
    public bool isFly = false;
    public bool isSwoon = false;
    public bool isLink = false;
    //是否在诱捕状态;
    public bool isEnsnared =false;    
    //是否在变羊状态;
    public bool isPolymorph =false;   
    //隐身
    public bool isHideBody = false;

    

    //状态属性.........................

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
