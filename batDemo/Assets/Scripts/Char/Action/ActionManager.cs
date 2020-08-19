using System;
using System.Collections.Generic;
using System.Collections;



public class ActionManager 
{
    private  Dictionary<string, Type> _typeMap;
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
        _typeMap = CreatMap();
        _actionDataMap=new Dictionary<string, ActionData>();

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
    private  Dictionary<string, Type> CreatMap()
    {
        Hashtable map = new Hashtable();
        _typeMap = new Dictionary<string, Type>();

        registerClass(GameEnum.ActionLabel.CmdAction, typeof(CmdAction));

        return _typeMap;
    }

    private  void registerClass(string typeName, Type afc)
    {
        _typeMap[typeName] = afc;
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
