


//池接口
using System;
public interface IData {
    //动画播放速度
    float PlaySpeed { get; set; }
    void init(ObjBase obj,Action onUpdate=null,Action onLateUpdate=null);
}
