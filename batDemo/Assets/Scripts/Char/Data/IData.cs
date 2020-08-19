


//池接口
using System;

public interface IData {
    float PlaySpeed { get; set; }

    void init(ObjBase obj,Action onFixUpdate);
}
