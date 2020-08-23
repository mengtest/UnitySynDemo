using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameEnum
{
    [AutoRegistLua]
     public class ControllerCmd
     {
         //遥感移动;
         public const int OnJoy_Move=1;
         //遥感抬起;
         public const int OnJoy_Up=2;


         public const int Char_Move=3;
        public const int  Char_StopMove=4;
         public const int Start_AI =5;
         public const int Stop_AI=6;
         public const int Paused_AI=7;
         public const int Continue_AI=8;
         public const int Char_FollowTarget=9;
         public const int Char_MoveToPos=10;
        //本游戏，特殊做，跟随带偏移
         public const int Char_UseSkill=11;
         public const int Char_MoveToPosList=12;
         public const int Char_MoveToPosWithoutPathFinder=13;
     }
     [AutoRegistLua]
    public class PlayMode {
        ///< 单机重播模式
       public const string ReplayMode="ReplayMode";
        ///< 单机模式
       public const string SingleMode="SingleMode";
        ///< 多人模式
        public const string MultiplayerMode="MultiplayerMode";
    }
     [AutoRegistLua]
     //控制类型...
    public enum CtrlType
    {
        Null=0,
        JoyCtrl=1,
        keyBordCtrl=2,
        AiCtrl=3,
        NetCtrl=4,
    }
    [AutoRegistLua]
    public enum ObjType
    {
        //对象.
        Obj=0,
        //角色
        Character=1,
        //怪物
        Monster=2,
        //玩家
        Player=3,
    }
    [AutoRegistLua]
     /**
     * 取消优先级 高的可以断底的 技能 技能取消用;
     */
    public class CancelPriority{
        //什么动作都不能切换;
       public static int CantDoAnyAction=-1;
       //stand move null Action.
      public static int  Stand_Move_Null=0;
      public static int  BaseAction=2;
        //roll attack
       public static int NormalAction=3;
        //skill
      public static int  SkillAction=4;
     //   Can 
    }
    [AutoRegistLua]
    public class CharState{
       public static int  Char_Idle = 0;  //正常状态
       public static int Char_Squat = 1; //蹲下
       public static int Char_Prone = 2; //趴下
       public static int Char_Weak =4; //虚弱 组队中
       public static int Char_Dead=5; //死亡
      public static int  Char_Skill=6; // 攻击 技能 位移 有攻击硬直 条件的等.
       public static int Char_Swimming = 7; //游泳
       public static int Char_Hurt = 8; //被打断 受击状态. 不能攻击
       public static int Char_HurtLie = 9;  //受击躺下 躺地   不能攻击
      public static int  Char_HurtLinkBone = 10; //绑定受击 链接状态. 绑到身上甩甩甩 不能攻击
       public static int Char_Polymorph = 11; //变成小动物 变羊 不能攻击 被攻击会破羊
       public static int Char_Freeze=12; //冰冻  
      public static int  Char_Swoon=13;  //眩晕  不能攻击
       public static int Char_Strobe =14; //滑索动作.
    }
    [AutoRegistLua]
    public class  ActionLabel
    {
        //动作标签 
         public static string Null = "Null";        
        public static string Stand = "Stand";        
        public static string StandUp = "StandUp";        
        public static string Reborn = "Reborn";  
        public static string Run = "Run";  
        public static string BackOff = "BackOff";  
        public static string CmdAction = "CmdAction";  
        public static string Dead = "Dead";  
        public static string Action = "Action";  


         //Ani Name.
        public static string run_fwd = "run_fwd";      
        public static string sprint_fwd = "sprint_fwd";      


    }
    [AutoRegistLua]
    //动作层级 
    public class  ActionLayer
    {
        public const int BaseLayer = 0;        
        public const int UpLayer = 1;  
        public const int AddLayer = 2;  
    }
    
}