﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace GameEnum
{
     //控制类型...
    public enum CtrlType
    {
        Null=0,
        JoyCtrl=1,
        keyBordCtrl=2,
        AiCtrl=3,
        NetCtrl=4,
    }
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
    public class EventType_Entity
    {
        public static string Jump_To_Ground = "JumpToGround";
        public static string Jump_Fall = "JumpFall";
        public static string Jump_Rise = "JumpRise";
        public static string Move_End = "MoveEnd";
    }
    public enum ColliderCheckType
    {
        ColliderCheckType_null = 0,
        ColliderCheckType_CloseTerrain = 1, // 贴地碰撞
        ColliderCheckType_Wall = 2, // 与地形碰撞（主要是空气墙和地表物件）
        ColliderCheckType_DynamicObj = 4, // 动态对象 (角色对象去检测与怪物以及NPC等的碰撞 怪物对象不主动做碰撞检测)
    }

    public enum ColliderObjType
    {
        ColliderCheckType_null = 0,
        ColliderCheckType_Terrain = 1, // 地表
        ColliderCheckType_Wall = 2, // 空气墙
        ColliderCheckType_TerrainObj = 3, // 地表物件
        ColliderCheckType_DynamicObj = 4, // 动态对象
    }
    public enum JumpState
    { 
        JumpToGround=0,
        JumpRise=1,
        JumpFall=2,
    
    }
    public enum HurtState
    {
        Hurt_Normal = 0, //正常;
        Hurt_HurtLie = 1,   //躺地;
    }
    //AI状态..
    public enum AIState
    {
        AI_NONE = 0,      //默认状态
        AI_WARNING = 1,   //警戒;
        AI_ATTACK = 2,    //攻击;
        AI_GOHOME =3,     //回家;
        AI_ESCAPE =4,     //逃跑;escape
        AI_PATROL=5,      //巡逻 ;patrol
        AI_BACKBUILD = 6, //回撤
        AI_WAIT = 7,      //等待状态
        AI_PUTMONSTER = 8,  //放怪状态
        AI_WARNING_POS = 9,   //原地警戒; 有时间间隔.
        AI_TOUCH  =10,       //点击移动
        AI_JOB = 11,         //工作状态--做动作
        AI_MoveToTargetArea =12, //Ai移动到具体位置..
        AI_RandomMove=13,   //两个兵相隔太近像随机点移动.
    }

    //防守方 攻击方 2个阵营
    public enum EnumAICampType
    {
        AI_DefenseCamp = 1,
        AI_AttackCamp = 2,
        AI_AllCamp = 3,
    }

    public struct playAni {
        //动画名称...
       public string strAcionName;
        //开始帧数 ....
        public int nStartFrame ;
        //速度比率 1 normal
        public float fSpeed ;
        //混合时间 0.15f
        public float fBlendTime;
        //循环次数;.
        public int nLoop;
        public bool dontRePlaySameAni;
    }
    public struct ChangePart
    {
        public string strPartName;
        public string strResName;
    }

    public struct AddLinkObj
    {
        public string strRenderObj;
        public string strLinkName;
        public Vector3 vOffset; // 偏移量
        public Vector3 rotate;  // 欧拉角
        public int nFollowType;
    }

    public struct LookAt
    {
        public Vector3 vTarget; // 目标点
        public Vector3 vUp;     // 向上的向量
    }

    public struct Move
    {
        public string strRunAct;
        public float m_dir;   // 如果moveto 则视为目标点
        public Vector3 m_target;
        public bool useYspeed;
        //移动速度;
        public float m_speed;
        //加速度默认0;
        public float m_AcceleratedSpeed;
        //速度倍率默认1;
        public bool ZeroSpeedStop;
        //是否使用移动点 1200默认true;
        public bool m_useMovePoint;
        //是否计算重量;
        public bool m_useWeightPower;
        public object param;
    }
    public struct Jump {
        //向上方向的力 
        public float upPow;
        //向上加速度... 负为重力;
        public float acceleratedupPow;
        //向上的力趋向0时取消加速度;
        public bool ZeroUpStop;
        //是否计算重量;
        public bool m_useWeightPower;
        public bool hasTarget;
        public float TargetY;
    }
    // 实体碰撞消息
    public class EntityColliderInfo
    {
        public ColliderObjType cct;   // 碰撞对象类型
        public int nColliderObjID;    // 碰撞对象ID
        public float fDis;            // 碰撞点和起点的距离
        public Vector3 pos;
        public Vector3 normal;
    }
}