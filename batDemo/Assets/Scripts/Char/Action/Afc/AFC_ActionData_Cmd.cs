//*************************************************************************
//	动作基础数据
//*************************************************************************

//技能部件
using UnityEngine;

public class AFC_ActionData_Cmd :AFC_Base_Cmd
{

    public override void onInit(){
            action.speed=this.afcData.floatAbb["speed"];
            action.fadeLength=this.afcData.floatAbb["fadeLength"];
            action.defultPriority=this.afcData.IntAbb["cancelPriority"];
            action.actionType=this.afcData.IntAbb["actionType"];
            //这里改成 upAction 上半身 addAction 添加动作.
            action.actionLayer=this.afcData.IntAbb["actionLayer"];
     }

    public override void execute()
    {
        
    }
}

