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
          public const int OnJump=3;


         public const int Char_Move=10;
        public const int  Char_StopMove=11;
         public const int Start_AI =12;
         public const int Stop_AI=13;
         public const int Paused_AI=14;
         public const int Continue_AI=15;
         public const int Char_FollowTarget=16;
         public const int Char_MoveToPos=17;
        //本游戏，特殊做，跟随带偏移
         public const int Char_UseSkill=18;
         public const int Char_MoveToPosList=19;
         public const int Char_MoveToPosWithoutPathFinder=20;
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
         public const string Null = "Null";        
        public const string Stand = "Stand";        
        public const string StandUp = "StandUp";        
        public const string Reborn = "Reborn";  
        public const string Run = "Run";  
         public const string Dash = "Dash";  
        public const string Jump = "Jump";  
        public const string BackOff = "BackOff";  
        public const string CmdAction = "CmdAction";  
        public const string Dead = "Dead";  
        public const string Action = "Action";  



         //Ani Name.
        public const string Idle_Wait_A="Idle_Wait_A";
        public const string Idle_Wait_B="Idle_Wait_B";

        public const string run_fwd = "run_fwd";      
        public const string Mvm_Jog = "Mvm_Jog";      
        
        public const string Mvm_Dash = "Mvm_Dash";   
    }
    [AutoRegistLua]
    //动作层级 
    public class  ActionLayer
    {
        public const int BaseLayer = 0;        
        public const int UpLayer = 1;  
        public const int AddLayer = 2;  
    }
    public enum JumpState
    { 
        JumpOnGround=0,
        JumpRise=1,
        JumpFall=2,
    
    }
}