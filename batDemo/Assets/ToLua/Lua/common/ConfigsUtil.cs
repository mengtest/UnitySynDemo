using System.Collections.Generic;
using UnityEngine;

public partial class Configs
{
    static void Init<T>(Dictionary<int, T> dict, string tablename)
    {
        //bool isServer = UnityGlobal.instance.isServer;
        if (true)   // 不区分客户端服务器
        {
            var config = ""; // Res.Manager.Load("Resources/config", Res.ResType.text).ToString();
            string[] lines = config.Split('\n');
            string tag = "*" + tablename + "^";
            List<string[]> dataList = new List<string[]>();
            foreach (string line in lines)
            {
                if (line.Contains(tag))
                {
                    string[] cons = line.Split('^');
                    dataList.Add(cons);
                }
            }
            if (dataList.Count > 0)
            {
                foreach (string[] arr in dataList)
                {
                    var item = System.Activator.CreateInstance<T>();
                    int index = 1;
                    int key = int.Parse(arr[1].ToString());
                    foreach (var pro in typeof(T).GetProperties())
                    {
                        var v = arr[index++];
                        //Log.logError("set", pro.ToString(), v);
                        try
                        {
                            if (pro.PropertyType == typeof(int))
                            {
                                if (string.IsNullOrEmpty(v))
                                {
                                    pro.SetValue(item, 0);
                                }
                                else
                                {
                                    pro.SetValue(item, int.Parse(v.ToString()));
                                }
                            }
                            else if (pro.PropertyType == typeof(float))
                            {
                                if (string.IsNullOrEmpty(v))
                                {
                                    pro.SetValue(item, (float)0);
                                }
                                else
                                {
                                    pro.SetValue(item, float.Parse(v.ToString()));
                                }
                            }
                            else if (pro.PropertyType == typeof(string))
                            {
                                v = v.Replace("'", "");
                                pro.SetValue(item, v.ToString());
                            }
                            else if (pro.PropertyType == typeof(int[]))
                            {
                                var list = new List<int>();
                                if (!string.IsNullOrEmpty(v.ToString()))
                                {
                                    string[] vArr = v.Split('|');
                                    foreach (var str in vArr)
                                    {
                                        list.Add(int.Parse(str));
                                    }
                                }
                                pro.SetValue(item, list.ToArray());
                            }
                            else if (pro.PropertyType == typeof(float[]))
                            {
                                var list = new List<float>();
                                string[] vArr = v.Split('|');
                                if (!string.IsNullOrEmpty(v.ToString()))
                                {
                                    foreach (var str in vArr)
                                    {
                                        list.Add(float.Parse(str));
                                    }
                                }
                                pro.SetValue(item, list.ToArray());
                            }
                            else if (pro.PropertyType == typeof(string[]))
                            {
                                var list = new List<string>();
                                string[] vArr = v.Split('|');
                                if (!string.IsNullOrEmpty(v.ToString()))
                                {
                                    foreach (var str in vArr)
                                    {
                                        list.Add(str.ToString());
                                    }
                                }
                                pro.SetValue(item, list.ToArray());
                            }
                            else
                            {
                                Log.logError("不支持的类型", v);
                            }
                        }
                        catch (System.Exception e)
                        {
                            Log.logError("config有错误:",tablename, pro.Name, v.ToString(), e.Message);
                        }
                    }
                    dict[key] = item;
                }
            }
            else
            {
                Log.logError("config解析错误", tablename);
            }
        }
        else
        {
            var table = Lua.LuaManager.instance.lua.GetTable("Configs." + tablename);
            if (table == null)
            {
                Debug.LogError("没有找到数据表:" + tablename);
                return;
            }
            foreach (var dic in table.ToDictTable())
            {
                var value = dic.Value as LuaInterface.LuaTable;
                var item = System.Activator.CreateInstance<T>();
                foreach (var pro in typeof(T).GetProperties())
                {
                    try
                    {
                        var v = value[pro.Name];
                        if (pro.PropertyType == typeof(int))
                        {
                            if (v == null)
                            {
                                pro.SetValue(item, 0);
                            }
                            else
                            {
                                pro.SetValue(item, int.Parse(v.ToString()));
                            }
                        }
                        else if (pro.PropertyType == typeof(float))
                        {
                            if (v == null)
                            {
                                pro.SetValue(item, 0);
                            }
                            else
                            {
                                pro.SetValue(item, float.Parse(v.ToString()));
                            }
                        }
                        else if (pro.PropertyType == typeof(string))
                        {
                            if (v == null)
                            {
                                pro.SetValue(item, "");
                            }
                            else
                            {
                                pro.SetValue(item, v.ToString());
                            }
                        }
                        else if (pro.PropertyType == typeof(int[]))
                        {
                            var list = new List<int>();
                            if (v != null)
                            {
                                var o = v as LuaInterface.LuaTable;
                                for (var i = 0; i < o.Length; i++)
                                {
                                    list.Add(int.Parse(o[i + 1].ToString()));
                                }
                            }
                            pro.SetValue(item, list.ToArray());
                        }
                        else if (pro.PropertyType == typeof(float[]))
                        {
                            var list = new List<float>();
                            if (v != null)
                            {
                                var o = v as LuaInterface.LuaTable;
                                for (var i = 0; i < o.Length; i++)
                                {
                                    list.Add(float.Parse(o[i + 1].ToString()));
                                }
                            }
                            pro.SetValue(item, list.ToArray());
                        }
                        else if (pro.PropertyType == typeof(string[]))
                        {
                            var o = v as LuaInterface.LuaTable;
                            var list = new List<string>();
                            for (var i = 0; i < o.Length; i++)
                            {
                                list.Add(o[i + 1].ToString());
                            }
                            pro.SetValue(item, list.ToArray());
                        }
                        else
                        {
                            Log.logError("不支持的类型");
                        }
                    }
                    catch (System.Exception e)
                    {
                        Log.logError("config有错误:", dic.Key, tablename, pro.Name, value.ToString(), e.Message);
                    }
                }
                dict[int.Parse(dic.Key.ToString())] = item;
            }
        }
    }
}

