using UnityEngine;

//判定接口..所有判定 都继承此接口 返回true 成功 flase失败
public interface IJudge
{
    //返回true 条件成立 false 条件不成立 不成立时执行操作.
    bool judge();
    void dispose();
}
