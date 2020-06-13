#define USING_DOTWEENING

using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;
using System.Linq;
using BindType = ToLuaMenu.BindType;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using static UnityEngine.UI.Button;
using UnityEditor.Animations;
using DG.Tweening;

public static class CustomSettings
{
    public static string saveDir => Application.dataPath + "/ToLua/Source/Generate/";
    public static string toluaBaseType => Application.dataPath + "/ToLua/BaseType/";
    public static string baseLuaDir => Application.dataPath + "/Tolua/Lua/";
    public static string injectionFilesPath => Application.dataPath + "/ToLua/Injection/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
        typeof(UnityEngine.Graphics),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList =
    {
        _DT(typeof(Action)),
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),
        _DT(typeof(DG.Tweening.TweenCallback)),
    };

    static bool NotExportType(Type t)
    {
        return t.IsArray || t.IsGenericType || t.IsPrimitive || t == typeof(string) || t.IsByRef || t.FullName.StartsWith("System") || ignoreTypes.Contains(t);
    }
    static void SealedTypeAdd(List<BindType> bindtypes, Type t)
    {
        if (NotExportType(t)) return;

        foreach (var mem in t.GetMembers(BindingFlags.Public | BindingFlags.Static))
        {
            if (mem is MethodInfo fun)
            {
                foreach (var par in fun.GetParameters())
                {
                    if (bindtypes.Where(x => x.type == par.ParameterType).Count() < 1)
                    {
                        if (par.ParameterType.IsArray || par.ParameterType.IsGenericType) continue;
                        if (par.ParameterType.IsClass || par.ParameterType.IsEnum)
                        {
                            if (!NotExportType(par.ParameterType))
                                bindtypes.Add(_GT(par.ParameterType));
                            SealedTypeAdd(bindtypes, par.ParameterType);
                        }
                    }
                }
            }
            else if (mem is FieldInfo f)
            {
                if (f.FieldType.IsClass || f.FieldType.IsEnum)
                {
                    if (bindtypes.Where(x => x.type == f.FieldType).Count() < 1)
                    {
                        if (!NotExportType(f.FieldType))
                            bindtypes.Add(_GT(f.FieldType));
                        SealedTypeAdd(bindtypes, f.FieldType);
                    }
                }
            }
            else if (mem is PropertyInfo p)
            {
                if (p.PropertyType.IsClass || p.PropertyType.IsEnum)
                {
                    if (bindtypes.Where(x => x.type == p.PropertyType).Count() < 1)
                    {
                        if (!NotExportType(p.PropertyType))
                            bindtypes.Add(_GT(p.PropertyType));
                        SealedTypeAdd(bindtypes, p.PropertyType);
                    }
                }
            }
        }
    }

    public static Type[] ignoreTypes = new[]
    {
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Ray),
        typeof(RaycastHit),
        typeof(Quaternion),
        typeof(Mathf),
        typeof(LayerMask),
        typeof(Bounds),
        typeof(Color),
        typeof(Touch),
        typeof(Plane),
        typeof(Time),
    };
    public static BindType[] customTypeList
    {
        get
        {
            var arr = customTypeList1.ToList();
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var cla in ass.GetTypes())
                {
                    if (cla.GetCustomAttribute<AutoRegistLuaAttribute>() != null)
                    {
                        if (arr.Where(x => x.type == cla).Count() < 1) arr.Add(_GT(cla));
                        if (cla.IsClass)
                        {
                            SealedTypeAdd(arr, cla);
                        }
                    }
                }
            }
            return arr.ToArray();
        }
        set
        {
            customTypeList1 = value;
        }
    }
    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList1 =
    {                
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),
        //_GT(typeof(TestProtol)),
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
        //_GT(typeof(KeyValuePair<int, TestAccount>)),
        //_GT(typeof(Dictionary<int, TestAccount>.KeyCollection)),
        //_GT(typeof(Dictionary<int, TestAccount>.ValueCollection)),
        //_GT(typeof(TestExport)),
        //_GT(typeof(TestExport.Space)),
        //-------------------------------------------------------------------        
                        
        _GT(typeof(LuaInjectionStation)),
        _GT(typeof(InjectType)),
        _GT(typeof(Debugger)).SetNameSpace(null),          
     
#if USING_DOTWEENING

        _GT(typeof(DG.Tweening.DOTween)),
        _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
        _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.LoopType)),
        _GT(typeof(DG.Tweening.PathMode)),
        _GT(typeof(DG.Tweening.PathType)),
        _GT(typeof(DG.Tweening.RotateMode)),
        _GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),    
#else
        _GT(typeof(Component)),
        _GT(typeof(Transform)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        //_GT(typeof(LineRenderer))
        //_GT(typeof(TrailRenderer))
#endif
      
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(GameObject)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        //_GT(typeof(Physics)),
        //_GT(typeof(Collider)),
        _GT(typeof(Time)),
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)),
        _GT(typeof(Shader)),
        _GT(typeof(Renderer)),
        _GT(typeof(Screen)),
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),
        _GT(typeof(AssetBundle)),
        //_GT(typeof(ParticleSystem)),
        //_GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),
        //_GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
#if UNITY_5_3_OR_NEWER && !UNITY_5_6_OR_NEWER
        _GT(typeof(UnityEngine.Experimental.Director.DirectorPlayer)),
#endif
        _GT(typeof(Animator)),
        _GT(typeof(AnimatorOverrideController)),
        _GT(typeof(RuntimeAnimatorController)),
        //_GT(typeof(Input)),
        //_GT(typeof(KeyCode)),
        //_GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),


        //_GT(typeof(MeshRenderer)),
#if !UNITY_5_4_OR_NEWER
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)), 
#endif

        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),

        _GT(typeof(Animation)),
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(SkinWeights)),
        _GT(typeof(RenderTexture)),
        _GT(typeof(Resources)),
        _GT(typeof(LuaProfiler)),

        _GT(typeof(Button)),
        _GT(typeof(Image)),
        _GT(typeof(Toggle)),
        _GT(typeof(ToggleGroup)),
        _GT(typeof(Text)),
        _GT(typeof(InputField)),
        _GT(typeof(Rect)),
        _GT(typeof(Sprite)),
        _GT(typeof(Dropdown)),
        _GT(typeof(Slider)),
        _GT(typeof(RectTransform)),
        _GT(typeof(RectTransformUtility)),
        _GT(typeof(PointerEventData)),
        _GT(typeof(ScrollRect)),
        _GT(typeof(SpriteRenderer)),
        _GT(typeof(WWW)),
        _GT(typeof(ButtonClickedEvent)),
        _GT(typeof(UnityEngine.TextAnchor)),
        _GT(typeof(UnityEngine.ColorUtility)),
        _GT(typeof(CanvasGroup)),
        _GT(typeof(Canvas)),
        _GT(typeof(UnityEngine.UI.ColorBlock)),
        _GT(typeof(UnityEngine.UI.Dropdown.OptionData)),
        _GT(typeof(DateTime)),
        _GT(typeof(TimeSpan)),
        _GT(typeof(TextMesh)),
    };

    public static List<Type> dynamicList = new List<Type>()
    {
        //typeof(MeshRenderer),
#if !UNITY_5_4_OR_NEWER
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),
#endif

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(SkinWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
        typeof(RectTransformUtility),

    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {

    };

    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>()
    {
        /*typeof(Transform),
        typeof(UIRoot),
        typeof(UICamera),
        typeof(UIViewport),
        typeof(UIPanel),
        typeof(UILabel),
        typeof(UIAnchor),
        typeof(UIAtlas),
        typeof(UIFont),
        typeof(UITexture),
        typeof(UISprite),
        typeof(UIGrid),
        typeof(UITable),
        typeof(UIWrapGrid),
        typeof(UIInput),
        typeof(UIScrollView),
        typeof(UIEventListener),
        typeof(UIScrollBar),
        typeof(UICenterOnChild),
        typeof(UIScrollView),        
        typeof(UIButton),
        typeof(UITextList),
        typeof(UIPlayTween),
        typeof(UIDragScrollView),
        typeof(UISpriteAnimation),
        typeof(UIWrapContent),
        typeof(TweenWidth),
        typeof(TweenAlpha),
        typeof(TweenColor),
        typeof(TweenRotation),
        typeof(TweenPosition),
        typeof(TweenScale),
        typeof(TweenHeight),
        typeof(TypewriterEffect),
        typeof(UIToggle),
        typeof(Localization),*/
    };

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }


    [MenuItem("Lua/Attach Profiler", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    [MenuItem("Lua/Detach Profiler", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
}
