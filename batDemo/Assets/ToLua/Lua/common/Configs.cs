using System.Collections.Generic;
partial class Configs{
static Dictionary<int,Buff_DataItem> _Buff_Data;
public static Dictionary<int,Buff_DataItem> Buff_Data{
get{
if(_Buff_Data==null){_Buff_Data=new Dictionary<int,Buff_DataItem>();Init(_Buff_Data,"Buff_Data");}return _Buff_Data;
}

}
public class Buff_DataItem{
public int BuffId{get;set;}
public int IsActSkill{get;set;}
public string BuffName{get;set;}
public string BuffIcon{get;set;}
public int Type{get;set;}
public string EffectId{get;set;}
public string SoundId{get;set;}
public int OnEventType{get;set;}
public int liftTime{get;set;}
public int interval{get;set;}
public int BuffType1{get;set;}
public string[] Value1_1{get;set;}
public string Value1_2{get;set;}
public int BuffType2{get;set;}
public string[] Value2_1{get;set;}
public string Value2_2{get;set;}
public string Info{get;set;}
}
static Dictionary<int,GlobalBaseConfigItem> _GlobalBaseConfig;
public static Dictionary<int,GlobalBaseConfigItem> GlobalBaseConfig{
get{
if(_GlobalBaseConfig==null){_GlobalBaseConfig=new Dictionary<int,GlobalBaseConfigItem>();Init(_GlobalBaseConfig,"GlobalBaseConfig");}return _GlobalBaseConfig;
}

}
public class GlobalBaseConfigItem{
public int Id{get;set;}
public string Name{get;set;}
public string[] Para{get;set;}
public string Info{get;set;}
}
static Dictionary<int,CardsSlotItem> _CardsSlot;
public static Dictionary<int,CardsSlotItem> CardsSlot{
get{
if(_CardsSlot==null){_CardsSlot=new Dictionary<int,CardsSlotItem>();Init(_CardsSlot,"CardsSlot");}return _CardsSlot;
}

}
public class CardsSlotItem{
public int Id{get;set;}
public int Page{get;set;}
public int Index{get;set;}
public int[] Currency{get;set;}
public int[] Price{get;set;}
}
static Dictionary<int,CardsItem> _Cards;
public static Dictionary<int,CardsItem> Cards{
get{
if(_Cards==null){_Cards=new Dictionary<int,CardsItem>();Init(_Cards,"Cards");}return _Cards;
}

}
public class CardsItem{
public int Id{get;set;}
public int PropIdx{get;set;}
public string Name{get;set;}
public int CharPro{get;set;}
public int ModelUrl{get;set;}
public int Icon{get;set;}
public int Quality{get;set;}
public int CanUse{get;set;}
public int CardFragment{get;set;}
public int UnlockkCard{get;set;}
public string Info{get;set;}
public int[] AvatarSetIdLv{get;set;}
}
static Dictionary<int,CardsPropertyItem> _CardsProperty;
public static Dictionary<int,CardsPropertyItem> CardsProperty{
get{
if(_CardsProperty==null){_CardsProperty=new Dictionary<int,CardsPropertyItem>();Init(_CardsProperty,"CardsProperty");}return _CardsProperty;
}

}
public class CardsPropertyItem{
public int Id{get;set;}
public string Name{get;set;}
public int Level{get;set;}
public string CharName{get;set;}
public string SkillName{get;set;}
public int SkillId{get;set;}
public int SkillCD{get;set;}
public string SkillIcon{get;set;}
public int PassiveSkillId{get;set;}
public string PassiveSkillIcon{get;set;}
public int SkillType{get;set;}
public int RewardId{get;set;}
public int Gold{get;set;}
public int CostCard{get;set;}
public int Diamond{get;set;}
}
static Dictionary<int,RewardIndexItem> _RewardIndex;
public static Dictionary<int,RewardIndexItem> RewardIndex{
get{
if(_RewardIndex==null){_RewardIndex=new Dictionary<int,RewardIndexItem>();Init(_RewardIndex,"RewardIndex");}return _RewardIndex;
}

}
public class RewardIndexItem{
public int IndexId{get;set;}
public int StartItem{get;set;}
public int EndItem{get;set;}
public int RandonCount{get;set;}
}
static Dictionary<int,RewardItemsItem> _RewardItems;
public static Dictionary<int,RewardItemsItem> RewardItems{
get{
if(_RewardItems==null){_RewardItems=new Dictionary<int,RewardItemsItem>();Init(_RewardItems,"RewardItems");}return _RewardItems;
}

}
public class RewardItemsItem{
public int IndexId{get;set;}
public int ItemId{get;set;}
public string IdName{get;set;}
public int ItemCount{get;set;}
public float Time{get;set;}
public int SelectRatio{get;set;}
public int ShowPriority{get;set;}
}
static Dictionary<int,RewardItem> _Reward;
public static Dictionary<int,RewardItem> Reward{
get{
if(_Reward==null){_Reward=new Dictionary<int,RewardItem>();Init(_Reward,"Reward");}return _Reward;
}

}
public class RewardItem{
public int Id{get;set;}
public string RewardName{get;set;}
public int StartRewardIndex{get;set;}
public int EndRewardIndex{get;set;}
public int[] ExtraItem{get;set;}
public int[] ShowPriority{get;set;}
}
static Dictionary<int,BattleMatchItem> _BattleMatch;
public static Dictionary<int,BattleMatchItem> BattleMatch{
get{
if(_BattleMatch==null){_BattleMatch=new Dictionary<int,BattleMatchItem>();Init(_BattleMatch,"BattleMatch");}return _BattleMatch;
}

}
public class BattleMatchItem{
public int Id{get;set;}
public string Name{get;set;}
public int TeamMemberCount{get;set;}
public int MaxCount{get;set;}
public int MinCount{get;set;}
}
static Dictionary<int,BattleModelItem> _BattleModel;
public static Dictionary<int,BattleModelItem> BattleModel{
get{
if(_BattleModel==null){_BattleModel=new Dictionary<int,BattleModelItem>();Init(_BattleModel,"BattleModel");}return _BattleModel;
}

}
public class BattleModelItem{
public int Id{get;set;}
public string Name{get;set;}
public string UI_Info{get;set;}
public string MapID{get;set;}
public int Index{get;set;}
public int[] MatchIDs{get;set;}
public int IsOpen{get;set;}
}
static Dictionary<int,Battle_BagItem> _Battle_Bag;
public static Dictionary<int,Battle_BagItem> Battle_Bag{
get{
if(_Battle_Bag==null){_Battle_Bag=new Dictionary<int,Battle_BagItem>();Init(_Battle_Bag,"Battle_Bag");}return _Battle_Bag;
}

}
public class Battle_BagItem{
public int Id{get;set;}
public string Name{get;set;}
public int Type{get;set;}
public int Quality{get;set;}
public string Icon{get;set;}
public string ModelUrl{get;set;}
public int Volume{get;set;}
public int ReduceDmgRate{get;set;}
public int Durability{get;set;}
public float Weight{get;set;}
public int BuffId{get;set;}
public int CastTime{get;set;}
public int Usable{get;set;}
}
static Dictionary<int,BattleItemRefreshItem> _BattleItemRefresh;
public static Dictionary<int,BattleItemRefreshItem> BattleItemRefresh{
get{
if(_BattleItemRefresh==null){_BattleItemRefresh=new Dictionary<int,BattleItemRefreshItem>();Init(_BattleItemRefresh,"BattleItemRefresh");}return _BattleItemRefresh;
}

}
public class BattleItemRefreshItem{
public int Id{get;set;}
public string Type{get;set;}
public int TypeID{get;set;}
public string SubType{get;set;}
public int WeaponType{get;set;}
public string Name{get;set;}
public int Score{get;set;}
public int RefreshNumber{get;set;}
public int Quality{get;set;}
public int EffectSize{get;set;}
public string ModelPos{get;set;}
public string ModelRotate{get;set;}
public float ModelScale{get;set;}
public int NormalItemWeight{get;set;}
public int HeightItemWeight{get;set;}
public int AirDropWeight{get;set;}
}
static Dictionary<int,BattleConfigItem> _BattleConfig;
public static Dictionary<int,BattleConfigItem> BattleConfig{
get{
if(_BattleConfig==null){_BattleConfig=new Dictionary<int,BattleConfigItem>();Init(_BattleConfig,"BattleConfig");}return _BattleConfig;
}

}
public class BattleConfigItem{
public int Id{get;set;}
public string Name{get;set;}
public string Para{get;set;}
public string Info{get;set;}
}
static Dictionary<int,WeaponHitPartItem> _WeaponHitPart;
public static Dictionary<int,WeaponHitPartItem> WeaponHitPart{
get{
if(_WeaponHitPart==null){_WeaponHitPart=new Dictionary<int,WeaponHitPartItem>();Init(_WeaponHitPart,"WeaponHitPart");}return _WeaponHitPart;
}

}
public class WeaponHitPartItem{
public int Id{get;set;}
public string WeaponType{get;set;}
public int WeaponPos{get;set;}
public int BodyDmgRate{get;set;}
public int HeadDmgRate{get;set;}
public int LimbDmgRate{get;set;}
}
static Dictionary<int,Weapon_SlotItem> _Weapon_Slot;
public static Dictionary<int,Weapon_SlotItem> Weapon_Slot{
get{
if(_Weapon_Slot==null){_Weapon_Slot=new Dictionary<int,Weapon_SlotItem>();Init(_Weapon_Slot,"Weapon_Slot");}return _Weapon_Slot;
}

}
public class Weapon_SlotItem{
public int Id{get;set;}
public int WeaponType{get;set;}
public string Name{get;set;}
public int AmmoID{get;set;}
public int Muzzleprefix{get;set;}
public int[] Muzzle{get;set;}
public int Foregripprefix{get;set;}
public int[] Foregrip{get;set;}
public int Magazineprefix{get;set;}
public int[] Magazine{get;set;}
public int Scopeprefix{get;set;}
public int[] Scope{get;set;}
public int Stockprefix{get;set;}
public int Stock{get;set;}
public int Uniqueprefix{get;set;}
public int Unique{get;set;}
}
static Dictionary<int,Weapon_RecoilTempleteItem> _Weapon_RecoilTemplete;
public static Dictionary<int,Weapon_RecoilTempleteItem> Weapon_RecoilTemplete{
get{
if(_Weapon_RecoilTemplete==null){_Weapon_RecoilTemplete=new Dictionary<int,Weapon_RecoilTempleteItem>();Init(_Weapon_RecoilTemplete,"Weapon_RecoilTemplete");}return _Weapon_RecoilTemplete;
}

}
public class Weapon_RecoilTempleteItem{
public int Id{get;set;}
public string Name{get;set;}
public float[] Yaw_params{get;set;}
public float[] Pitch_params{get;set;}
}
static Dictionary<int,GrenadeItem> _Grenade;
public static Dictionary<int,GrenadeItem> Grenade{
get{
if(_Grenade==null){_Grenade=new Dictionary<int,GrenadeItem>();Init(_Grenade,"Grenade");}return _Grenade;
}

}
public class GrenadeItem{
public int ID{get;set;}
public int WeaponType{get;set;}
public string Name{get;set;}
public int Quality{get;set;}
public string Icon{get;set;}
public string ModelUrl{get;set;}
public float RunSpeedRate{get;set;}
public float WalkSpeedRate{get;set;}
public int DamageRadius{get;set;}
public int GrenadeTiming{get;set;}
public int ReadyDelayTime{get;set;}
public int HitMaxCount{get;set;}
public int RotateAngle{get;set;}
public int[] Range{get;set;}
public int[] Damage{get;set;}
public string BoomSound{get;set;}
}
static Dictionary<int,WeaponItem> _Weapon;
public static Dictionary<int,WeaponItem> Weapon{
get{
if(_Weapon==null){_Weapon=new Dictionary<int,WeaponItem>();Init(_Weapon,"Weapon");}return _Weapon;
}

}
public class WeaponItem{
public int Id{get;set;}
public int WeaponType{get;set;}
public string Name{get;set;}
public string WeaponLink{get;set;}
public int FireType{get;set;}
public int Quality{get;set;}
public string ShowIcon{get;set;}
public string Icon{get;set;}
public string ModelUrl{get;set;}
public float SwitchWeaponTime{get;set;}
public int[] WeaponPos{get;set;}
public int[] Damage{get;set;}
public int Magzine{get;set;}
public int[] MagzineChange{get;set;}
public float ReloadTime{get;set;}
public int BulletNum{get;set;}
public float BodyDamageByRate{get;set;}
public float HeadDamageByRate{get;set;}
public float LimbDamageByRate{get;set;}
public float FireRate{get;set;}
public float EnableScopeDur{get;set;}
public float AccRange_Base{get;set;}
public float AccRange_Max{get;set;}
public float AccAdd1{get;set;}
public float AccAdd2{get;set;}
public float AccRateRun{get;set;}
public float AccRateJump{get;set;}
public float AccRateSquat{get;set;}
public float AccRateLie{get;set;}
public int RecoilID{get;set;}
public int shotFire_Max{get;set;}
public float RecoilRateYaw{get;set;}
public float RecoilRatePitch{get;set;}
public int EnableRandomYaw{get;set;}
public int Yaw_rateID{get;set;}
public float ShotFireDecRate{get;set;}
public float ShotFireDec{get;set;}
public float AccDec{get;set;}
public float RunSpeedRate{get;set;}
public float WalkSpeedRate{get;set;}
public float AimMoveAdditive{get;set;}
public float SlowPower{get;set;}
public int SlowTime{get;set;}
public int FireCount{get;set;}
public float FireInterval{get;set;}
public float AutoAimAngleCoe{get;set;}
public float AutoAimBaseSpeed{get;set;}
public float StandAutoAimRate{get;set;}
public float AutoAimOffset{get;set;}
public float AutoAimBodyOffset{get;set;}
public float SniperShake{get;set;}
public int ShakeBulletCount{get;set;}
public float DelayClipInTime{get;set;}
public float DelayBoltTime{get;set;}
public string FireSound{get;set;}
public string ClipOutSound{get;set;}
public string ClipInSound{get;set;}
public string BoltSound{get;set;}
public string Anim1{get;set;}
public string Anim2{get;set;}
public string Muzzle{get;set;}
}
static Dictionary<int,MeleeWeaponItem> _MeleeWeapon;
public static Dictionary<int,MeleeWeaponItem> MeleeWeapon{
get{
if(_MeleeWeapon==null){_MeleeWeapon=new Dictionary<int,MeleeWeaponItem>();Init(_MeleeWeapon,"MeleeWeapon");}return _MeleeWeapon;
}

}
public class MeleeWeaponItem{
public int ID{get;set;}
public int WeaponType{get;set;}
public int WeaponMeleeType{get;set;}
public string Name{get;set;}
public int Quality{get;set;}
public string Icon{get;set;}
public string ModelUrl{get;set;}
public float RunSpeedRate{get;set;}
public float WalkSpeedRate{get;set;}
public float Dist{get;set;}
public int Damage{get;set;}
public float SwitchWeaponTime{get;set;}
public float FireRate{get;set;}
public int HitAngle{get;set;}
public string anim{get;set;}
}
static Dictionary<int,Slot_PropertiesItem> _Slot_Properties;
public static Dictionary<int,Slot_PropertiesItem> Slot_Properties{
get{
if(_Slot_Properties==null){_Slot_Properties=new Dictionary<int,Slot_PropertiesItem>();Init(_Slot_Properties,"Slot_Properties");}return _Slot_Properties;
}

}
public class Slot_PropertiesItem{
public int Id{get;set;}
public string Name{get;set;}
public int Quality{get;set;}
public string Describe{get;set;}
public string Icon{get;set;}
public string ModelUrl{get;set;}
public float DeRecoilRateYaw{get;set;}
public float DeRecoilRatePitch{get;set;}
public float AddShoteFireDecRate{get;set;}
public float DeAccRange_Base{get;set;}
public float DeFireRate{get;set;}
public float DeEnableScopeDur{get;set;}
public float DeShotFireDecRate{get;set;}
public float DeAccDec{get;set;}
public float DeReloadTime{get;set;}
public float Magnification{get;set;}
public float AddWeaponPos{get;set;}
public int Throughwall{get;set;}
}
static Dictionary<int,Weapon_RandomRateRecoilTempItem> _Weapon_RandomRateRecoilTemp;
public static Dictionary<int,Weapon_RandomRateRecoilTempItem> Weapon_RandomRateRecoilTemp{
get{
if(_Weapon_RandomRateRecoilTemp==null){_Weapon_RandomRateRecoilTemp=new Dictionary<int,Weapon_RandomRateRecoilTempItem>();Init(_Weapon_RandomRateRecoilTemp,"Weapon_RandomRateRecoilTemp");}return _Weapon_RandomRateRecoilTemp;
}

}
public class Weapon_RandomRateRecoilTempItem{
public int Id{get;set;}
public float[] Yaw_Rate_params{get;set;}
}
static Dictionary<int,PoisonCircleConfigItem> _PoisonCircleConfig;
public static Dictionary<int,PoisonCircleConfigItem> PoisonCircleConfig{
get{
if(_PoisonCircleConfig==null){_PoisonCircleConfig=new Dictionary<int,PoisonCircleConfigItem>();Init(_PoisonCircleConfig,"PoisonCircleConfig");}return _PoisonCircleConfig;
}

}
public class PoisonCircleConfigItem{
public int Id{get;set;}
public int SceneID{get;set;}
public int CircleRadius{get;set;}
public int FindPointRadius{get;set;}
public int PoisonSafeTime{get;set;}
public int CircleMoveTime{get;set;}
public int PoisonDamage{get;set;}
}
static Dictionary<int,AvatarItem> _Avatar;
public static Dictionary<int,AvatarItem> Avatar{
get{
if(_Avatar==null){_Avatar=new Dictionary<int,AvatarItem>();Init(_Avatar,"Avatar");}return _Avatar;
}

}
public class AvatarItem{
public int ID{get;set;}
public int ModelID{get;set;}
public string Name{get;set;}
public int Quality{get;set;}
public int Part{get;set;}
public string[] ShieldID{get;set;}
public string Photo{get;set;}
public int[] CloseEnv{get;set;}
public int CreateGive{get;set;}
}
static Dictionary<int,AvatarTimeItem> _AvatarTime;
public static Dictionary<int,AvatarTimeItem> AvatarTime{
get{
if(_AvatarTime==null){_AvatarTime=new Dictionary<int,AvatarTimeItem>();Init(_AvatarTime,"AvatarTime");}return _AvatarTime;
}

}
public class AvatarTimeItem{
public string ID_Level{get;set;}
public float Time{get;set;}
}
static Dictionary<int,ExperienceConfigItem> _ExperienceConfig;
public static Dictionary<int,ExperienceConfigItem> ExperienceConfig{
get{
if(_ExperienceConfig==null){_ExperienceConfig=new Dictionary<int,ExperienceConfigItem>();Init(_ExperienceConfig,"ExperienceConfig");}return _ExperienceConfig;
}

}
public class ExperienceConfigItem{
public int Levels{get;set;}
public int Exp{get;set;}
public int LevelUpExp{get;set;}
public int LvUpReward{get;set;}
}
static Dictionary<int,SettingConfigItem> _SettingConfig;
public static Dictionary<int,SettingConfigItem> SettingConfig{
get{
if(_SettingConfig==null){_SettingConfig=new Dictionary<int,SettingConfigItem>();Init(_SettingConfig,"SettingConfig");}return _SettingConfig;
}

}
public class SettingConfigItem{
public int Id{get;set;}
public int type{get;set;}
public string Name{get;set;}
public string Para{get;set;}
public string Info{get;set;}
}
static Dictionary<int,CareerScoreItem> _CareerScore;
public static Dictionary<int,CareerScoreItem> CareerScore{
get{
if(_CareerScore==null){_CareerScore=new Dictionary<int,CareerScoreItem>();Init(_CareerScore,"CareerScore");}return _CareerScore;
}

}
public class CareerScoreItem{
public int Id{get;set;}
public string ScoreType{get;set;}
public string Name{get;set;}
public float Section{get;set;}
public float Power{get;set;}
public float Slope{get;set;}
public float Constant{get;set;}
}
static Dictionary<int,ItemConfigItem> _ItemConfig;
public static Dictionary<int,ItemConfigItem> ItemConfig{
get{
if(_ItemConfig==null){_ItemConfig=new Dictionary<int,ItemConfigItem>();Init(_ItemConfig,"ItemConfig");}return _ItemConfig;
}

}
public class ItemConfigItem{
public int Id{get;set;}
public string Name{get;set;}
public int Quality{get;set;}
public int LinkId{get;set;}
public string Description{get;set;}
public int LimitNum{get;set;}
public int UseType{get;set;}
public int LimitCompose{get;set;}
public int ComposeID{get;set;}
public float EffectiveTime{get;set;}
public string Value1{get;set;}
public string Value2{get;set;}
public string Icon{get;set;}
public string ModelUrl{get;set;}
public string Recommendation{get;set;}
}
static Dictionary<int,ItemIdxItem> _ItemIdx;
public static Dictionary<int,ItemIdxItem> ItemIdx{
get{
if(_ItemIdx==null){_ItemIdx=new Dictionary<int,ItemIdxItem>();Init(_ItemIdx,"ItemIdx");}return _ItemIdx;
}

}
public class ItemIdxItem{
public int Id{get;set;}
public string ExcelName{get;set;}
public string Name{get;set;}
public int LinkIdx{get;set;}
}
static Dictionary<int,HotZoneItem> _HotZone;
public static Dictionary<int,HotZoneItem> HotZone{
get{
if(_HotZone==null){_HotZone=new Dictionary<int,HotZoneItem>();Init(_HotZone,"HotZone");}return _HotZone;
}

}
public class HotZoneItem{
public int Id{get;set;}
public int SceneID{get;set;}
public int SignId{get;set;}
public string Name{get;set;}
public int AreaRadius{get;set;}
}
}
