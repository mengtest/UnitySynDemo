//*************************************************************************
//	技能部件
//*************************************************************************

//技能部件
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

    public class AFC_Base_Data
    {
        //public int idx;
        //类型 名称;
        public string eventName;
        //当前帧 时间 秒.
        public float time;
        //是否持续有效 接受update
        public bool isDuration;
        //长度 秒;
        public float length = 0;
        //只运行一次
        public bool onceEvent=false;

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
        //array  Int   uint Enum
        public Dictionary<string, List<int>> ArrayIntAbb =new Dictionary<string, List<int>>();
        //array   float 
        public Dictionary<string, List<float>> ArrayFloatAbb=new Dictionary<string, List<float>>();
        //array  bool
        public Dictionary<string, List<bool>> ArrayBoolAbb=new Dictionary<string, List<bool>>();
        //array  vector3  eulerangle
        public Dictionary<string, List<Vector3>> ArrayVec3Abb=new Dictionary<string, List<Vector3>> ();

        public  AFC_Base_Data()
        {
            
        }

       public void initXml(XmlNode eventNode) {
           //目前只做常用类解析  后续要用到再加.
            this.time = float.Parse(eventNode.Attributes["time"].Value);
            this.eventName = eventNode.Attributes["eventName"].Value;
            this.isDuration = bool.Parse(eventNode.Attributes["isDuration"].Value);
            if(this.isDuration){
                this.length=float.Parse(eventNode.Attributes["length"].Value);
            }
            //各个属性. 存储.
            XmlNodeList list =eventNode.ChildNodes;
            for (int i = 0; i < list.Count; i++)
            {
                XmlNode _fieldNode =list[i];
                 //统一变小写安全.
                if(_fieldNode.Name=="onceEvent"){
                   this.onceEvent = bool.Parse(_fieldNode.Attributes["value"].Value);
                }else{
                    string fieldTypeName = _fieldNode.Name.ToLower();
                    switch(fieldTypeName)
                    {
                        case "float":
                        this.floatAbb[_fieldNode.Name] = float.Parse(_fieldNode.Attributes["value"].Value);
                        break;
                        case "int":
                        case "Enum":
                        case "uint":
                            this.IntAbb[_fieldNode.Name] = int.Parse(_fieldNode.Attributes["value"].Value);
                        break;
                        case "string":
                            this.StringAbb[_fieldNode.Name] = _fieldNode.Attributes["value"].Value;
                        break;
                        case "bool":
                        this.BoolAbb[_fieldNode.Name] = bool.Parse(_fieldNode.Attributes["value"].Value);
                        break;
                        case "vector3":
                        case "eulerangle":
                        this.Vec3Abb[_fieldNode.Name] =new Vector3(float.Parse(_fieldNode.Attributes["x"].Value),float.Parse(_fieldNode.Attributes["y"].Value),float.Parse(_fieldNode.Attributes["z"].Value));
                        break;
                        case "array":
                            string subTypeName = _fieldNode.Attributes["type"].Value.ToLower();
                            XmlNodeList fieldlist =_fieldNode.ChildNodes;
                            switch(subTypeName)
                        {
                            case "float":
                                    ArrayFloatAbb[_fieldNode.Name]=new List<float>();
                                    for (int j = 0; j < fieldlist.Count; j++)
                                    {
                                        XmlNode _fieldChildNode =fieldlist[i];
                                        ArrayFloatAbb[_fieldNode.Name].Add(float.Parse(_fieldChildNode.Attributes["value"].Value));
                                    }
                            break;
                            case "int":
                            case "Enum":
                            case "uint":
                                    ArrayIntAbb[_fieldNode.Name]=new List<int>();
                                    for (int j = 0; j < fieldlist.Count; j++)
                                    {
                                        XmlNode _fieldChildNode =fieldlist[i];
                                        ArrayIntAbb[_fieldNode.Name].Add(int.Parse(_fieldChildNode.Attributes["value"].Value));
                                    }
                            break;
                            case "string":
                                    ArrayStrAbb[_fieldNode.Name]=new List<string>();
                                    for (int j = 0; j < fieldlist.Count; j++)
                                    {
                                        XmlNode _fieldChildNode =fieldlist[i];
                                        ArrayStrAbb[_fieldNode.Name].Add(_fieldChildNode.Attributes["value"].Value);
                                    }
                            break;
                            case "bool":
                                ArrayBoolAbb[_fieldNode.Name]=new List<bool>();
                                    for (int j = 0; j < fieldlist.Count; j++)
                                    {
                                        XmlNode _fieldChildNode =fieldlist[i];
                                        ArrayBoolAbb[_fieldNode.Name].Add(bool.Parse(_fieldChildNode.Attributes["value"].Value));
                                    }
                            break;
                            case "vector3":
                            case "eulerangle":
                                ArrayVec3Abb[_fieldNode.Name]=new List<Vector3>();
                                    for (int j = 0; j < fieldlist.Count; j++)
                                    {
                                        XmlNode _fieldChildNode =fieldlist[i];
                                        ArrayVec3Abb[_fieldNode.Name].Add(new Vector3(float.Parse(_fieldChildNode.Attributes["x"].Value),float.Parse(_fieldChildNode.Attributes["y"].Value),float.Parse(_fieldChildNode.Attributes["z"].Value)));
                                    }
                            break;
                        }
                        break;
                        default:

                        break;
                    }
                }
            }
       }

    }

