using UnityEngine;

public interface IPoolObj {
    int id { get; set;}
    //池的名字 这个最好不要改.会影响池；
    string poolname{ get; set; }
    IPool pool {get ; set; }
    bool isRecycled {  get;  set; }
    void init();
    //在获取时;
    void onGet();
    //在回收中;
    void onRecycle();
    //自己回收;
    void recycleSelf();
    //销毁方法;
    void Release();
}

//池接口
public interface IPool {
  void recycle(IPoolObj item);
}
