using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomSeed
    {
       /// <summary>
       /// 同步随机种子....每个动作或飞行道具带上随机种子 后面他的行为就完全同步了...
       /// </summary>
        public static Dictionary<int, int> charDRandeomSeed = new Dictionary<int, int>();

        public static Dictionary<int, int> charD_Hurt_RandeomSeed = new Dictionary<int, int>();

       //自己随机数 以负数起步；
        public static void setSeedBegin(int id,int beginSeed) {
            charDRandeomSeed[id] = beginSeed;
        }
        
        // 二择
        public static int branch(int charId)
        {
            if (getRandom(charId) < 0.5f)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        public static int getSeedBegin() {
            int t = (int)(UnityEngine.Random.value * 10000);
            return t;
        }
        /// <summary>
        /// 获取随机数 PVPID
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float getRandom(int charId)
        {
            //isVerfy.ContainsKey(data) && loopIs && listSkillTargetData.Count<3
            if (charDRandeomSeed.ContainsKey(charId))
            {
                charDRandeomSeed[charId] = (int)((charDRandeomSeed[charId] * 1103515245L + 12345L) & 0x7fffffff);
       //         DeBugLogger.LogTrace(charDRandeomSeed[charId] + "");
            //    float ttt = charDRandeomSeed[charId] % 10000/10000f;
    //            DeBugLogger.LogTrace(ttt+"");
                return charDRandeomSeed[charId] % 10000 / 10000f;
            }
            else
            {
                charDRandeomSeed[charId] = (int)(UnityEngine.Random.value *10000);
                return getRandom(charId);
            }
        }
    /// <summary>
    ///  获取万分率的随机数 不用再乘10000 最大10000;
    /// </summary>
    /// <param name="charId"></param>
    /// <returns></returns>
        public static float getRandom10000(int charId)
        {
            //isVerfy.ContainsKey(data) && loopIs && listSkillTargetData.Count<3
            if (charDRandeomSeed.ContainsKey(charId))
            {
                charDRandeomSeed[charId] = (int)((charDRandeomSeed[charId] * 1103515245L + 12345L) & 0x7fffffff);
                return charDRandeomSeed[charId] % 1000;
            }
            else
            {
                charDRandeomSeed[charId] = (int) UnityEngine.Random.value *10000;
                return getRandom10000(charId);
            }
        }
        // 随机整数jj  不包含最大值...
        public static int random(int charId,float num)
        {
            return (int)Mathf.Floor((getRandom(charId)) * num);
        }
        //包含最大值...
        public static int random(int charId,int start, int end)
        {
            return (int)Mathf.Floor(getRandom(charId) * (end - start + 1) + start);
        }
        //不取整..
        public static float random(int charId,float start, float end)
        {
            return ((getRandom(charId))* (end - start) + start);
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static float getBattleRandom(int charId)
        {
            //isVerfy.ContainsKey(data) && loopIs && listSkillTargetData.Count<3
            if (charD_Hurt_RandeomSeed.ContainsKey(charId))
            {
                charD_Hurt_RandeomSeed[charId] = (int)((charD_Hurt_RandeomSeed[charId] * 1103515245L + 12345L) & 0x7fffffff);
                //         DeBugLogger.LogTrace(charD_Hurt_RandeomSeed[charId] + "");
                //    float ttt = charD_Hurt_RandeomSeed[charId] % 10000/10000f;
                //            DeBugLogger.LogTrace(ttt+"");
                return charD_Hurt_RandeomSeed[charId] % 10000 / 10000f;
            }
            else
            {
                charD_Hurt_RandeomSeed[charId] = (int)(UnityEngine.Random.value * 10000);
            //    DeBugLogger.LogTrace("随机种子:  "+charD_Hurt_RandeomSeed[charId]);
                return getBattleRandom(charId);
            }
        }
        /// <summary>
        ///  获取万分率的随机数 不用再乘10000 最大10000;
        /// </summary>
        /// <param name="charId"></param>
        /// <returns></returns>
        public static float getBattleRandom10000(int charId)
        {
            //isVerfy.ContainsKey(data) && loopIs && listSkillTargetData.Count<3
            if (charD_Hurt_RandeomSeed.ContainsKey(charId))
            {
                charD_Hurt_RandeomSeed[charId] = (int)((charD_Hurt_RandeomSeed[charId] * 1103515245L + 12345L) & 0x7fffffff);
                return charD_Hurt_RandeomSeed[charId] % 1000;
            }
            else
            {
                charD_Hurt_RandeomSeed[charId] = (int)UnityEngine.Random.value * 10000;
                return getBattleRandom10000(charId);
            }
        }
        
    }

