//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class JudgeFrame : Judge
{
    private float _frame;

    public JudgeFrame(int num)
    {
        _frame = num;
    }

    override public bool judge()
    {
        bool bl = false;
        if (_frame <= 0)
        {
            bl = false;
        }
        else
            bl = true;
      //  _frame -= Time.deltaTime * GlobalData.frameRate;
         _frame -= 1;
        return bl;
    }
//         //顿帧时候 要添加 判断帧;
//         public void delayFrame(int frame)
//         {
//             if (_frame > 0)
//             {
//                 _frame += frame;
//             }
//         }
    public override void dispose()
    {
        base.dispose();
    }
}


