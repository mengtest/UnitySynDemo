using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core 
{
   private static MultiplePool m_battlePool=new MultiplePool("BattlePool");
   private static MultiplePool m_objectPool=new MultiplePool("ObjectPool");
   public static void Init(){
         
   }
    public static MultiplePool ObjectPool{
        get{
           return Core.m_objectPool;
        }
    }
    public static MultiplePool BattlePool{
        get{
           return Core.m_battlePool;
        }
    }

}
