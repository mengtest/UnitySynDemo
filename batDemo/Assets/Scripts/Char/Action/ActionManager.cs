using System;
using System.Collections.Generic;
using System.Collections;



public class ActionManager 
{
    //代码 里注册的动作
    private Dictionary<string, Type> _typeMap;
    //代码 里注册的动作
    private Dictionary<string,int> _actionLayerMap;
    //代码 里注册的动作
    private Dictionary<string,int> _actionCancelLvMap;
    //XML 动态注册的动作.
    private Dictionary<string, ActionData> _actionDataMap;
    private static ActionManager s_Instance = null;
    public static ActionManager instance
    {
        get
        {
            if (null == s_Instance)
            {
                s_Instance = new ActionManager();
                s_Instance.init();
            }
            return s_Instance;
        }
    }
    public  void init()
    {
        _typeMap = new Dictionary<string, Type>();
        _actionLayerMap= new Dictionary<string, int>();
        _actionDataMap=new Dictionary<string, ActionData>();
        _actionCancelLvMap=new Dictionary<string, int>();
        initClass();
    }
     /**
     * 解析 ActData
     * 因为帧同步 需要实例化的数据不能等待，所以需要提前加载完成 
     */
    public void initActData(Action callback){


    }
    public ActionData GetActionData(string actionLabel){
        if(this._actionDataMap.ContainsKey(actionLabel)){
            return this._actionDataMap[actionLabel];
        }
        return null;
    }
    //获得动作层级.
    public int GetActionLayer(string actionLabel){
       if(_actionLayerMap.ContainsKey(actionLabel)){
           //代码已注册的.
            return  _actionLayerMap[actionLabel];
       }else{
          //XML data的
          if(this._actionDataMap.ContainsKey(actionLabel)){
             return this._actionDataMap[actionLabel].actionLayer;
          }else{
              return 0;
          }
       }
    }
    //获得取消优先级
    public int GetCancelPriority(string actionLabel){
       if(_actionCancelLvMap.ContainsKey(actionLabel)){
           //代码已注册的.
            return  _actionCancelLvMap[actionLabel];
       }else{
          //XML data的
          if(this._actionDataMap.ContainsKey(actionLabel)){
             return this._actionDataMap[actionLabel].cancelPriority;
          }else{
              return 0;
          }
       }
    }
    private  void initClass()
    {
        registerClass(GameEnum.ActionLabel.Stand, typeof(Stand),0,GameEnum.CancelPriority.Stand_Move_Null);
        registerClass(GameEnum.ActionLabel.Run, typeof(Run),0,GameEnum.CancelPriority.Stand_Move_Null);
        registerClass(GameEnum.ActionLabel.Dash, typeof(Dash),0,GameEnum.CancelPriority.Stand_Move_Null);
         registerClass(GameEnum.ActionLabel.Jump, typeof(Jump),0,GameEnum.CancelPriority.Stand_Move_Null);
        registerClass(GameEnum.ActionLabel.CmdAction, typeof(CmdAction),0,GameEnum.CancelPriority.Stand_Move_Null);
    }

    private  void registerClass(string actionLabel, Type afc,int Layer=0,int cancelPriority=0)
    {
        _typeMap[actionLabel] = afc;
        _actionLayerMap[actionLabel]= Layer;
        _actionCancelLvMap[actionLabel]=cancelPriority;
    }

    public  ActionBase GetAction(string actionLabel)
    {
        if(_typeMap.ContainsKey(actionLabel)){
            return  Core.BattlePool.get<ActionBase>(actionLabel,_typeMap[actionLabel]);
        }else{
            //通过解析xml json 生成cmdAction
            CmdAction cmdAction =  Core.BattlePool.get<CmdAction>(actionLabel);
            //name 判断下该动作是否被解析过没有就从新解析。
            if (!this._actionDataMap.ContainsKey(actionLabel)) {
                DebugLog.LogError("actionData未被解析, actionLabel: ",actionLabel);
                    //返回默认站立动作。
                cmdAction.recycleSelf();
                return  Core.BattlePool.get<ActionBase>(GameEnum.ActionLabel.Stand,_typeMap[GameEnum.ActionLabel.Stand]);
            }
            return cmdAction;
        }
    }
}
