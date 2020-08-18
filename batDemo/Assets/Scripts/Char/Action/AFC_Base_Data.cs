//*************************************************************************
//	技能部件
//*************************************************************************

//技能部件
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class AFC_Base_Data
    {
        public int idx;
        //类型 名称;
        public string eventName;

        public float time;

        public bool isDuration;
        //长度 秒;
        public int length = 0;
        //只运行一次
        public bool onceEvent;

        //string
        public Dictionary<string, string> StringAbb =new Dictionary<string, string>();
        //Int   uint Enum
        public Dictionary<string, int> IntAbb=new Dictionary<string, int>();
         // float  
        public Dictionary<string, float> floatAbb=new Dictionary<string, float>();
        //bool
        public Dictionary<string, bool> BoolAbb=new Dictionary<string, bool>();
        //vector3  eulerangle
        public Dictionary<string, Vector3> Vec3Abb= new Dictionary<string, Vector3>();
        //quaternion
        // public QuatAbb: Map<string, cc.Quat>;
        //array  string
        public Dictionary<string, List<string>> ArrayStrAbb =new Dictionary<string, List<string>>();
        //array  Int   uint
        public Dictionary<string, List<int>> ArrayNumAbb =new Dictionary<string, List<int>>();
        //array   float 
        public Dictionary<string, List<float>> ArrayFloatAbb=new Dictionary<string, List<float>>();
        //array  bool
        public Dictionary<string, List<bool>> ArrayBoolAbb=new Dictionary<string, List<bool>>();
        //array  vector3  eulerangle
        public Dictionary<string, List<Vector3>> ArrayVec3Abb=new Dictionary<string, List<Vector3>> ();

        public  AFC_Base_Data()
        {
            
        }
       
      public void init(object jsonT) {

      }

       public void initXml(object xml) {

       }

    }

