using System;
using GameEnum;
using UnityEngine;

//角色数据..同步数据也写在这
public class CharData : MonoBehaviour,IData
{

    private Character _char=null;
    public bool isMyPlayer=false;
    private Action _onFixUpdate; 
    
    public int pvpId;
    public CtrlType ctrlType=CtrlType.Null;



    //状态属性.......................

    //冲刺
    public bool isDashing=false;
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

    public float PlaySpeed { get ; set ; }


    //动作数据; 技能类型; 1 攻击 2技能 0其他.. 3 roll 滚
    public int currentBaseActionType = 0;
    public string currentBaseAction = GameEnum.ActionLabel.Stand;


    //动作数据; 技能类型; 1 攻击 2技能 0其他.. 3 roll 滚
    public int currentUpLayerActionType = 0;
    public string currentUpLayerAction = GameEnum.ActionLabel.Null;


    //动作数据; 技能类型; 1 攻击 2技能 0其他.. 3 roll 滚
    public int currentAddLayerActionType = 0;
    public string currentAddLayerAction =  GameEnum.ActionLabel.Null;


    public float RunSpeed=5;
    public float DashSpeed=7;

    //状态属性.........................

    // Start is called before the first frame update
    void Start()
    {
        PlaySpeed=1f;
    }

    public void init(ObjBase obj,Action onFixUpdate){
          _char=obj  as Character;
           _onFixUpdate=onFixUpdate;
    }
    public Character getChar(){
        return _char;
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
