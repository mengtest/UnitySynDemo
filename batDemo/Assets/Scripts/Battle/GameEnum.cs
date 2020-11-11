using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameEnum
{
    ///按钮 输入
    [AutoRegistLua]
    public class KeyInput {
        //跳
       public const string Jump="Jump";
        //瞄准
       public const string Aim="Aim";
        //攻击
        public const string Attack="Attack";
         //拾取
         public const string PickUp="PickUp";
         //丢弃
         public const string DropItem="DropItem";
         //选择武器1
         public const string SelectWeapon_1="SelectWeapon_1";
         //选择武器2
         public const string SelectWeapon_2="SelectWeapon_2";
        //蹲下
        public const string Squat="Squat";
        //换弹 
         public const string Reload="Reload";

         //闪避 
         public const string Roll="Roll";

         //闪现;
         public const string Blink="Blink";

         //技能 1
         public const string Skill_1="Skill_1";
        //技能 2
         public const string Skill_2="Skill_2";
         
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
     //控制类型...
     [AutoRegistLua]
    public enum CtrlType
    {
        Null=0,
        JoyCtrl=1,
        keyBordCtrl=2,
        AiCtrl=3,
        NetCtrl=4,
    }
    [AutoRegistLua]
    public enum AimState
    {
        Null=0,
        //瞬间状态.
        Begin=1,
        Aiming=2,
        AimingFinish=3,
        //瞬间状态.
        AimOff=4,
    }
    [AutoRegistLua]
    public enum GunState
    {
        Idle=0,
        Firing=1,
        Reloading=2,
    }
    [AutoRegistLua]
    public enum ObjType
    {
        //显示对象----基类.
        Obj=0,
        //角色
        Character=1,
        //怪物
        Monster=2,
        //玩家
        Player=3,
        //道具-----基类
        Item=4,
        //武器
         Weapon=5,
         Gun=6,
         //子弹.
         Bullet=7,
    }
    [AutoRegistLua]
    //打击材质.
    public class HitMatType
    {
        //默认 石头
       public const string Prototype = "Prototype";      
       //肉类 角色
        public const string Meat = "Meat"; 
       //金属
        public const string Metal = "Metal";      
        //沙
        public const string Sand = "Sand";    
        //雪 冰
        public const string Snow = "Snow";    
        //石头 岩石 水泥
        public const string Stone = "Stone";  
        //木头
        public const string Wood = "Wood";  
        //草地 草各种草
        public const string Grass = "Grass";  
    }
     [AutoRegistLua]
    public enum EffectType
    {
        //普通特效
        Effect=0,
        //可控制粒子特效
        PsEffect=1,
    }
     /**
     * 取消优先级 高的可以断底的 技能 技能取消用;
     */
     [AutoRegistLua]
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
        public const string Walk = "Walk";  
         public const string Dash = "Dash";  
        public const string Jump = "Jump";  
        public const string DoubleJump = "DoubleJump";
        public const string BackOff = "BackOff";  
        public const string CmdAction = "CmdAction";  
        public const string Dead = "Dead";  
        public const string PickUp = "PickUp"; 

        public const string Aiming = "Aiming"; 
        public const string Shooting = "Shooting"; 

        public const string UpIdle="UpIdle";
        public const string ChangeWeapon="ChangeWeapon";
        public const string Reloading="Reloading";
        public const string ItemDrop = "ItemDrop";  
        public const string ItemDefault = "ItemDefault"; 


        public const string Action = "Action";  



    }
    [AutoRegistLua]
    public class AniLabel{
         //Ani Name.
        public const string Idle="Idle";

        
        public const string aim_Idle="aim_Idle";
        public const string Idle_Wait_A="Idle_Wait_A";
        public const string Idle_Wait_B="Idle_Wait_B";

        public const string run_fwd = "run_fwd";  
          public const string walk_fwd    = "walk_fwd";  
        public const string Mvm_Jog = "Mvm_Jog";      
        
        public const string Mvm_Dash = "Mvm_Dash";   
        public const string  Mvm_DashEnd_C="Mvm_DashEnd_C";
        public const string  Mvm_DashEnd_B="Mvm_DashEnd_B";

        public const string Mvm_Stop_Front="Mvm_Stop_Front";

        public const string Jmp_Base_A_Rise="Jmp_Base_A_Rise";
        public const string Jmp_DoubleJump_A_Rise="Jmp_DoubleJump_A_Rise";
        
        public const string Jmp_Base_A_OnGround="Jmp_Base_A_OnGround";
        public const string Jmp_Base_A_Fall="Jmp_Base_A_Fall";
        public const string PickUp = "PickUp"; 
        

        //下半身
        public const string Down_Jmp_Base_A_Rise="Down_Jmp_Base_A_Rise";
        public const string Down_Jmp_Base_A_OnGround="Down_Jmp_Base_A_OnGround";
        public const string Down_Jmp_Base_A_Fall="Down_Jmp_Base_A_Fall";
          public const string DownIdle="DownIdle";




          //上半身.....................................................
        public const string UpIdle="UpIdle";
        public const string ChangeWeapon="ChangeWeapon";

        public const string pistol_aim="pistol_aim";
        public const string pistol_reload="pistol_reload";
        public const string pistol_shot="pistol_shot";
        public const string pistol_guard="pistol_guard";

        public const string rifle_aim="rifle_aim";
        public const string rifle_reload="rifle_reload";
        public const string rifle_shot="rifle_shot";

        public const string rifle_idle="rifle_idle";

    }
    [AutoRegistLua]
    //动作层级 
    public class  ActionLayer
    {
        public const int BaseLayer = 0;        
        public const int UpLayer = 1;  
        public const int AddLayer = 2;  
    }
    [AutoRegistLua]
    public enum JumpState
    { 
        JumpOnGround=0,
        JumpRise=1,
        JumpFall=2,
    
    }
    //道具类型  int(id/100000)
    [AutoRegistLua]
    public enum ItemType                                    
	{
       //主武器 枪  10101001  
        Gun=101,
       //副武器 手枪
        PistolGun=102,
       //近战武器
        Melee=103,
       //投掷物
         Grenade=104,
        //配件
         Slot=201,
        //防弹衣 装备.
         Armor=301,
        //背包
         Bag=302,
         //头盔,
         Helmet=303,
        //灌木 隐身衣 
         Shrup=304,
        //子弹
         Bullet=401,
         //能量 
         Drug_Boost=402,
        //治疗
         Drug_Healing=403,
        //Fashion 时装
          Avatar_Fashion=500,
        //Fashion 套装
          Avatar_Suit=501,
         //卡牌
         Cards=601,
        //卡牌碎片
         Card_Fragment=602,
        //万能卡牌碎片
         Card_General_Fragment=603,
        //基础货币
         Currency=701,
        //特殊货币
         SpecialCurrency=702,
        //经验卡
         ExperienceCard=801,
        //升级卡
         UpgradeCard=802,
        //通行证
         PassCheck=803,
        //奖励
         Reward=804,
        //改名卡
         RenamingCard=805,
        //装饰
         Decorate=901,
        //表情动作
         Expression_Action=902,
    }

    // 枪武器类型.
    [AutoRegistLua]
    public enum Weapon_GunType                                    
	{
		  NONE=0,
         //步枪
		  AR=101,
         //精准步枪
		  DMR=102,
         //狙击枪
          SR=103,
         //轻机枪
          LMG=104,
         //冲锋枪
          SMG=105,
         //散弹枪  喷子
          ShotGun=106,
         //特殊武器  爆炸弓 ,榴弹枪
          Special=107,
          //火箭筒
          Launchers=108,
         //手枪.
          Pistol=201,
    }
    //近战武器类型.
    [AutoRegistLua]
    public enum Weapon_MeleeType                                    
	{
         //拳击_手套 拳套.
          Boxing_Glove=301,
         //匕首
          Knife = 302,
         //单手剑
          Sword = 303,
         //大剑
          BroadSword = 303,
         //双刃.
           DualBlades = 304,
         //镰刀
           Sickle = 305,
          //长矛
          Lance =306,
	}
    [AutoRegistLua]
    public enum Weapon_GrenadeType                                    
	{
         FragGrenade=401,
           SmokeGrenade=402,

    }
    [AutoRegistLua]
    public enum GunNameLink                                    
	{
           M4,
           AKM,
           M16A4,
    }
    [AutoRegistLua]
    // 开火类型
	public enum FireType                                    
	{
        //1、单发
		SEMI=1,
        //2、连发 也是 半自动
		BURST=2,
        //3、全自动
		AUTO=3,
	}
}