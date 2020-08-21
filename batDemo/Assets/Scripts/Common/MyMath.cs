using System;
using UnityEngine;


    public class MyMath
    {
        // 二择
        public static int branch()
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        // 随机整数
        public static int random(float num)
        {
            return (int)Mathf.Floor(UnityEngine.Random.value * num);
        }
        //包括start 到 包括尾数end...
        public static int random(int start, int end)
        {
            return (int)Mathf.Floor(UnityEngine.Random.value * (end - start + 1) + start);
        }
        public static float random(float start, float end)
        {
            return (UnityEngine.Random.value * (end - start) + start);
        }
        // 给出角度的速度计算
        public static Vector3 getSpeedA(float speed, float angle)
        {
            float speed_x = 0f;
            float speed_y = 0f;
            if (angle >= 360f)
            {
                angle = angle - Mathf.Floor(angle / 360f) * 360f;
            }
            else if (angle < 0f)
            {
                angle = 360f + angle + Mathf.Floor(-angle / 360f) * 360f;
            }
            float radian = angle * Mathf.PI / 180f;
            if (angle == 0f)
            {
                speed_x = 1 * speed;
                speed_y = 0;
            }
            else if (angle == 90)
            {
                speed_x = 0;
                speed_y = -1 * speed;
            }
            else if (angle == 180)
            {
                speed_x = -1 * speed;
                speed_y = 0;
            }
            else if (angle == 270)
            {
                speed_x = 0;
                speed_y = 1 * speed;
            }
            else
            {
                speed_x = Mathf.Cos(radian) * speed;
                speed_y = Mathf.Sin(radian) * speed;
            }
            if (angle > 0 && angle < 90)
            {
  //              speed_x = speed_x;
                speed_y = -speed_y;
            }
            else if (angle > 90 && angle < 180)
            {
 //               speed_x = speed_x;
                speed_y = -speed_y;
            }
            else if (angle > 180 && angle < 270)
            {
  //              speed_x = speed_x;
                speed_y = -speed_y;
            }
            else if (angle > 270 && angle < 360)
            {
//                speed_x = speed_x;
                speed_y = -speed_y;
            }
            else
            {
//                 speed_x = speed_x;
//                 speed_y = speed_y;
            }
            return new Vector3(speed_x, speed_y, 0f);
        }

        // 给出坐标的角度计算（后者在前者的什么方向）
        public static float angleCompute(float x1, float y1, float x2, float y2)
        {
            float temp_a = x2 - x1;
            float temp_b = y2 - y1;
            float temp_c = Mathf.Sqrt(temp_a * temp_a + temp_b * temp_b);
            float temp_angle = Mathf.Abs(Mathf.Asin(temp_b / temp_c) * 180 / Mathf.PI);
            if (x2 > x1)
            {
                if (y2 > y1)
                {
                    temp_angle = 360 - temp_angle;
                }
                else if (y2 < y1)
                {
                }
                else
                {
                    temp_angle = 0;
                }
            }
            else if (x2 < x1)
            {
                if (y2 > y1)
                {
                    temp_angle = 180 + temp_angle;
                }
                else if (y2 < y1)
                {
                    temp_angle = 180 - temp_angle;
                }
                else
                {
                    temp_angle = 180;
                }
            }
            else
            {
                if (y2 > y1)
                {
                    temp_angle = 270;
                }
                else if (y2 < y1)
                {
                    temp_angle = 90;
                }
                else
                {
                    temp_angle = 0f;
                }
            }
            return temp_angle;
        }

        // 角度转换-限定在0-360之间
        public static float reAngle(float angle)
        {
            float rst = angle % 360f;
            if (rst < 0f)
            {
                rst += 360f;
            }
            return rst;
        }

        public static float intersectionAngle(float agl1, float agl2)
        {
            agl1 = reAngle(agl1);
            agl2 = reAngle(agl2);
            float aglDis = Mathf.Abs(agl2 - agl1);
            if (aglDis > 270)
            {
                aglDis = 360 - aglDis;
            }
            else if (aglDis > 180)
            {
                aglDis = 270 - aglDis;
            }
            else if (aglDis > 90)
            {
                aglDis = 180 - aglDis;
            }
            return aglDis;
        }

        public static float angleDifference(float agl1, float agl2)
        {
            agl1 = reAngle(agl1);
            agl2 = reAngle(agl2);
            float aglDis = Mathf.Abs(agl2 - agl1);
            if (aglDis > 180)
            {
                aglDis = 360 - aglDis;
            }

            return aglDis;
        }
        //计算两点中点
        public static Vector2 centerCompute(float x1, float y1, float x2, float y2)
        {
            float ctX = x1 + (x2 - x1) / 2;
            float ctY = y1 + (y2 - y1) / 2;
            return new Vector2(ctX, ctY);
        }

        // 根据方向调转角度
        public static float mAngle(float angle, float scaleX)
        {
            float tagl = 0f;
            if (scaleX < 0)
            {
                tagl = 180 - angle;
            }
            else
            {
                tagl = angle;
            }
            return tagl;
        }

        //
        public static int cheakDeflectionDirection(float angle, float tarAngle)
        {
            float agl1 = reAngle(angle);
            float agl2 = reAngle(tarAngle);
            int direction = 0;
            if (agl2 > agl1)
            {
                direction = agl2 - agl1 < 180 ? 1 : -1;
            }
            else
            {
                direction = agl1 - agl2 < 180 ? -1 : 1;
            }
            return direction;
        }
        public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
            Vector3 resultVec3 = center + point;
            return resultVec3;
        }
    }

