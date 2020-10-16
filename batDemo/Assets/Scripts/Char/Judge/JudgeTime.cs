using UnityEngine;
using System.Collections;

    public class JudgeTime : Judge
    {
        private float m_fLiftTime = 0.0f;

        public JudgeTime(float fLiftTime)
        {
            m_fLiftTime = fLiftTime;
        }

        override public bool judge()
        {
            bool bl = false;
            if (m_fLiftTime <= 0.0f)
            {
                bl = false;
            }
            else
            {
				bl = true;
			}    
             //帧同步框架 判断都在帧上;
            m_fLiftTime -= GameSettings.Instance.deltaTime ;
            // Time.deltaTime;
            return bl;
        }
        public override void dispose()
        {
            base.dispose();
        }
    }

