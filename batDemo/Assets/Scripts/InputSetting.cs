using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetting 
{
    static InputSetting mInstance;
    public static InputSetting Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = new InputSetting();
                mInstance.init();
            }
            return mInstance;
        }
    }
    //攻击自动开镜
    public bool AttackAutoAimming =true;

    /**
     * @name: xsddxr909
     * @test: 
     * @msg: 初始化 游戏设置
     * @param {type} 
     * @return: 
     */
    private void init(){

          AttackAutoAimming =LocalCache.GetBool("AttackAutoAimming",true);

    }
}
