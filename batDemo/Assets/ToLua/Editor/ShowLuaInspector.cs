using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;
//using System.Diagnostics;
using System.Text;
[CustomEditor(typeof(LuaView))]
public class ShowLuaInspectorRect : Editor
{

	//Stopwatch stopwatch = new Stopwatch();
	Transform transform;
	string path;
    string subPPath;
	bool isShow = false;
    public static string tagName="";
	private void OnEnable()
    {
        subPPath=(target as LuaView).subPath;
		path = Application.dataPath + "/ScriptsLua/View/";
        if(subPPath!=""){
         path=path+ subPPath+"/";
        }
		transform = (target as LuaView).transform;
        tagName=target.name;
		DrawLuaInspector.OnEnable(transform);
	}

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
		//if(isShow)
		if (!_isHasLuaScript())
		{
			if (GUILayout.Button("添加脚本childView"))
			{
				_generateLuaChildScript();
			}
            if (GUILayout.Button("添加脚本PanelView"))
			{
				_generateLuaPanelScript();
			}
		}
		else
		{
			DrawLuaInspector.OnInspectorGUI(transform);
		}
	}

	bool _isHasLuaScript()
	{
		return File.Exists(path + target.name+".lua") ? true : false;
	}
	void _generateLuaPanelScript()
	{
        subPPath=(target as LuaView).subPath;
        path = Application.dataPath + "/ScriptsLua/View/";
        if(subPPath!=""){
         path=path+ subPPath+"/";
        }
         tagName=target.name;
		string name = target.name;
		string path1 = path + name + ".lua";
		if (!File.Exists(path1))
		{
             FileUtils.CreateDirectorySafely(path);
			using (StreamWriter sw = File.CreateText(path1))
			{
				StringBuilder sb = new StringBuilder();
				//name = name.Replace(" ", "");
				sb.AppendLine("---@class " + name + " : PanelView");
				sb.AppendLine(name + " = Class(\""+ name + "\",PanelView)");
				sb.AppendLine("--自动生成-------------------------------------------");
				//添加function:OnUIInit
                 sb.AppendLine("---添加UI引用.");
				sb.AppendLine("function " + name + ":OnUIInit()");
				sb.AppendLine("end\n");
				//添加function：OnUIDestory()
                sb.AppendLine("---移除UI引用.");
				sb.AppendLine("function " + name + ":OnUIDestory()");
                sb.AppendLine("---OnUIDestory自动生成----");
				sb.AppendLine("end\n");
                sb.AppendLine("--自动生成-----------end------------------------------");

                //initialize()
                sb.AppendLine("function " + name + ":initialize()");
                sb.AppendLine("PanelView.initialize(self)");
                  if(subPPath!=""){
                     sb.AppendLine("self.viewName= \"View."+subPPath+"." + name + "\"");
                    sb.AppendLine("self.url=\"View/"+ subPPath+"/"+ name + "\"");
                }else{
                    sb.AppendLine("self.viewName= \"View." + name + "\"");
                    sb.AppendLine("self.url=\"View/" + name + "\"");
                }
                sb.AppendLine("self.needUpdate=false");
                sb.AppendLine("self.ViewLayer=ViewLayer.content");
                sb.AppendLine("self.showMode=ViewShowMode.Normal");
				sb.AppendLine("end\n");

                sb.AppendLine("-----UI加载完成--Open之前-------");
	            sb.AppendLine("function " + name + ":OnInit(param)");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("function " + name + ":OnDestory()");
                sb.AppendLine("--记得nil移除变量");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("--open 时候开始监听");
                sb.AppendLine("function " + name + ":AddListener()");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("--close hiding freeze 时候关闭监听");
                sb.AppendLine("function " + name + ":RemoveListener()");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("--- update");
                sb.AppendLine("function " + name + ":Update()");
                sb.AppendLine("  PanelView.Update(self)");
                sb.AppendLine("");
				sb.AppendLine("end\n");
                
                sb.AppendLine("function " + name + ":onOpen()");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("function " + name + ":onClose()");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("---@return " + name);
				sb.AppendLine("return " + name);
				sw.WriteLine(sb.ToString());
			}
			DrawLuaInspector.OnEnable(transform);
			AssetDatabase.Refresh();
		}
	}
    void _generateLuaChildScript()
	{
     //   DebugLog.Log(target.)
        subPPath=(target as LuaView).subPath;
        path = Application.dataPath + "/ScriptsLua/View/";
        if(subPPath!=""){
         path=path+ subPPath+"/";
        }
         tagName=target.name;
        string name = target.name;
		string path1 = path + name + ".lua";
		if (!File.Exists(path1))
		{
            FileUtils.CreateDirectorySafely(path);
			using (StreamWriter sw = File.CreateText(path1))
			{
				StringBuilder sb = new StringBuilder();
				//name = name.Replace(" ", "");
				sb.AppendLine("---@class " + name + " : ChildView");
				sb.AppendLine(name + " = Class(\""+ name + "\",ChildView)");
                sb.AppendLine("---@return " + name);
				sb.AppendLine("--自动生成-------------------------------------------");
				//添加function:OnUIInit
                 sb.AppendLine("---添加UI引用.");
				sb.AppendLine("function " + name + ":OnUIInit()");
				sb.AppendLine("end\n");
				//添加function：OnUIDestory()
                sb.AppendLine("---移除UI引用.");
				sb.AppendLine("function " + name + ":OnUIDestory()");
                sb.AppendLine("---OnUIDestory自动生成----");
				sb.AppendLine("end\n");
                sb.AppendLine("--自动生成-----------end------------------------------");

                //initialize()
                sb.AppendLine("function " + name + ":initialize()");
                sb.AppendLine("ChildView.initialize(self)");
                if(subPPath!=""){
                     sb.AppendLine("self.viewName= \"View."+subPPath+"." + name + "\"");
                    sb.AppendLine("self.url=\"View/"+ subPPath+"/"+ name + "\"");
                }else{
                    sb.AppendLine("self.viewName= \"View." + name + "\"");
                    sb.AppendLine("self.url=\"View/" + name + "\"");
                }
                sb.AppendLine("self.needUpdate=false");
                sb.AppendLine("self.ViewLayer=ViewLayer.content");
                sb.AppendLine("--是否为部件");
                sb.AppendLine("self.isPart=true");
				sb.AppendLine("end\n");

                sb.AppendLine("--初始化");
	            sb.AppendLine("function " + name + ":OnInit(param)");
                sb.AppendLine("");
				sb.AppendLine("end\n");


                sb.AppendLine("function " + name + ":OnDestory()");
                sb.AppendLine("--记得nil移除变量");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("function " + name + ":AddListener()");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("function " + name + ":RemoveListener()");
                sb.AppendLine("");
				sb.AppendLine("end\n");

                sb.AppendLine("function " + name + ":Update()");
                sb.AppendLine("--- update");
				sb.AppendLine("end\n");


				sb.AppendLine("return " + name);
				sw.WriteLine(sb.ToString());
			}
			DrawLuaInspector.OnEnable(transform);
			AssetDatabase.Refresh();
		}
	}
}

class LuaObject
{
    private string oldname = "";
    private string name = "";
    public bool isClick = false;
    public string Name
    {
        get { return name; }
        set
        {
            if (name != value)
            {
                if (name != "")
                    oldname = name;
                name = value;
                DrawLuaInspector.UpdateLua();
            }
        }
    }
    public bool isArray = false;
    private string type = "Transform";
    private string Type
    {
        get
        {
            return type;
        }
        set
        {
            if (type != value)
            {
                type = value;
                if (string.IsNullOrEmpty(type) || type == "Transform") { index = 0; return; }
                index = GetTypes().IndexOf(type);
            }
        }
    }
    private int index = 0;
    public int TypeIndex
    {
        get
        {
            return index;
        }
        set
        {
            if (index != value)
            {
                index = value;
                type = GetTypes()[index];
            }
        }
    }
    private int funindex = -2;
    private string funname;
    public int FunctionIndex
    {
        get
        {
            if (funindex == -2)
                funindex = DrawLuaInspector.Funnames.IndexOf(funname);
            return funindex;
        }
        set
        {
            if (funindex != value)
            {
                funindex = value;
            }
        }
    }
    private string FunName
    {
        get
        {
            if (FunctionIndex < 0) return "";
            return DrawLuaInspector.Funnames[FunctionIndex];
        }
    }
    public string fullmes;
    private UnityEngine.Object obj;
    public UnityEngine.Object Obj
    {
        get
        {
            if (transform == null) return null;
            if (type != "")
                return transform.GetComponent(type);
            return obj;
        }
        set
        {
            if (obj != value)
            {
                var temp = value as Component;
                if (temp)
                {
                    transform = temp.gameObject.transform;
                    obj = temp;
                }
                else
                {
                    var temp1 = value as GameObject;
                    if (temp1)
                    {
                        transform = temp1.transform;
                        obj = temp1.transform;
                    }
                    else
                    {
                        transform = null;
                        obj = null;
                    }
                    index = 0;
                    type = "";
                }

				//DrawLuaInspector.UpdateLua();
			}
        }
    }
    public Transform parent;
    public Transform transform;
    public LuaObject(string name, string all, Transform parent = null, string mes = "")
    {
        this.name = name;
        fullmes = all;
        this.parent = parent;
        var arr = mes.Split(':');
        Transform tran = null;
        Regex r1 = new Regex(@"^self\.([\w\d_]+)[;\s]*$");
        Regex find = new Regex(@"^Find\([""']([^""']*)[""']\);*$", RegexOptions.IgnoreCase);
        Regex subget = new Regex(@"SubGet\([""']([^""]*)[""'],[""']([^""']*)[""']\)", RegexOptions.IgnoreCase);
        Regex getcom = new Regex(@"GetComponent\([""']([^""']*)[""']\)", RegexOptions.IgnoreCase);
        if (name == "" && all.Contains("self:AddClick"))
        {
            mes = mes.Replace(" ", "").Replace("\"", "").Replace("'", "");
            var objname = mes.Split(',')[0];
            funname = mes.Split(',')[1];
            isClick = true;
            if (r1.IsMatch(objname))
            {
                var match = r1.Match(objname);
                var itemname = match.Groups[1].Value;
                var itemobj = DrawLuaInspector.luaobjs.Where(x => x.name == itemname).FirstOrDefault();
                if (itemobj != null)
                {
                    tran = itemobj.transform;
                }
                else tran = null;
            }
            else
                tran = parent.Find(objname);
            if (objname == "" && parent.GetComponent<Button>() == null) { }
            else
                type = "Button";
        }
        else
            foreach (var item in arr)
            {
                var nitem = item.Replace(" ", "").Replace(";", "").Trim();
                if (nitem == "self")
                {
                    tran = parent;
                }
                else if (r1.IsMatch(nitem))
                {
                    var match = r1.Match(nitem);
                    var itemname = match.Groups[1].Value;
                    var itemobj = DrawLuaInspector.luaobjs.Where(x => x.name == itemname).FirstOrDefault();
                    if (itemobj != null)
                    {
                        tran = itemobj.transform;
                    }
                    else tran = null;
                }
                else if (find.IsMatch(nitem))
                {
                    if (tran)
                    {
                        var match = find.Match(nitem);
                        var itemname = match.Groups[1].Value;
                        tran = tran.Find(itemname);
                    }
                }
                else if (subget.IsMatch(nitem))
                {
                    if (tran)
                    {
                        var match = subget.Match(nitem);
                        var itemname = match.Groups[1].Value;
                        type = match.Groups[2].Value;
                        tran = tran.Find(itemname);
                    }
                }
                else if (getcom.IsMatch(nitem))
                {
                    if (tran)
                    {
                        var match = getcom.Match(nitem);
                        type = match.Groups[1].Value;
                    }
                }
            }
        transform = tran;
        if (tran == null) type = "";
        var type1 = type;
        type = "";
        this.Type = type1;
    }

    public List<string> GetTypes()
    {
        List<string> types = new List<string>() { };
        if (transform)
            foreach (var item in transform.GetComponents<Component>())
            {
                try
                {
                    types.Add(item.GetType().Name);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        return types;
    }
    string ChangeToFullType(string simtype)
    {
        if (simtype == "")
        {
            if (transform.GetComponent<RectTransform>() != null)
                return typeof(RectTransform).FullName;
            return typeof(Transform).FullName;
        }
        foreach (var item in transform.GetComponents<Component>())
        {
            try
            {
                if (item.GetType().Name == simtype) return item.GetType().FullName;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        return typeof(RectTransform).FullName;
    }
    public string GetCode()
    {
        if (!isClick)
        {
            if (transform == null)
                return "    self." + name + "=nil";
            var code = "self." + name + "=self:";
            if (Type != "" && Type != "Transform" && Type != "RectTransfrom") code += "SubGet(\"" + GetRelPath(transform) + "\",\"" + Type + "\")";
            else
                code += "Find(\"" + GetRelPath(transform) + "\")";
            return "    ---@type " + ChangeToFullType(Type) + "\r\n    " + code;
        }
        else
        {
            return "    self:AddClick(\"" + GetRelPath(transform) + "\",\"" + FunName + "\")";
        }
    }
    public void Update(ref string text)
    {
        if (oldname != "" && name != "")
        {
            Regex r = new Regex(@"self\." + name + @"([^\w\d_])");
            if (r.IsMatch(text))
            {
                Debug.LogError(string.Format("该lua文件已经有了名为{0}的变量", name));
                name = oldname;
                oldname = "";
                return;
            }
            Regex r1 = new Regex(@"self\." + oldname + @"([^\w\d_])");
            text = r1.Replace(text, "self." + name + "$1");
            fullmes = r1.Replace(fullmes, "self." + name + "$1");
            oldname = "";
        }
		else
		{
			if (isClick && (transform == null || FunName == "")) return;
			Regex r = new Regex(@":OnUIInit\(.*?\)([\s\S]*?)[\r\n]+end", RegexOptions.Multiline);
			var code = GetCode();
			if (code == "") return;
			foreach (Match item in r.Matches(text))
			{
           //    DebugLog.Log(item.Groups[1].Value);
				var str = item.Groups[1].Value;
				if (string.IsNullOrEmpty(str)) continue;
				string str1 = "";
				if (isClick)
				{
					str1 = str.Replace(fullmes, addname + code);
				}
				else if (name != "")
					str1 = str.Replace(fullmes, addname + code);
				else
					str1 = str.Replace(fullmes + "\r\n", "").Replace(fullmes + "\n", "").Replace("\r\n" + fullmes, "").Replace("\n" + fullmes, "");
				text = text.Replace(str, str1);
			}
			fullmes = code;
			addname = "";
		}
	}
    public string GetRelPath(Transform tran)
    {
        if (tran == parent || tran == null) return "";
        var path = tran.name;
        while (true)
        {
            tran = tran.parent;
            if (tran == null || tran == parent) break;
            path = tran.name + "/" + path;
        }
        return path;
    }

    string addname = "";
    internal void Add(string v)
    {
        addname = "    " + v + "\r\n";
    }
}
class DrawLuaInspector
{
    static string luapath = Application.dataPath + "/ScriptsLua/View";
    static string viewpath = "Assets/Res/View";
    static List<string> funnames;
    public static List<string> Funnames
    {
        get
        {
            if (funnames == null)
                UpdateFunmes();
            return funnames;
        }
    }
    static void UpdateFunmes()
    {
        Regex r = new Regex(@"function " + Path.GetFileNameWithoutExtension(path) + @":([\w\d_]+)\s*\(\s*\)");
        funnames = new List<string>();
        foreach (Match item in r.Matches(luatext))
        {
            var name = item.Groups[1].Value;
            if (name.StartsWith("On"))
                funnames.Add(name);
        }
    }
    static string luatext = "";
    static DefaultAsset asset;
    static string path;
    static int count;
    public static List<LuaObject> luaobjs = new List<LuaObject>();
    static bool change = false;
    static GameObject select;
    [InitializeOnLoadMethod]
    static void OnClick()
    {
        EditorApplication.hierarchyChanged += () => { change = true; UpdateLua(); };
        //EditorApplication.playmodeStateChanged += () => { change = false; Clear(); };
    }
    static string relpath
    {
        get
        {
            return path.Replace("\\", "/").Replace(Application.dataPath.Replace("Assets", ""), "");
        }
    }
	public static void OnEnable(Transform transform, bool update = false)
    {
        count = 1;
        var paths = Directory.GetFiles(luapath, "*.lua", SearchOption.AllDirectories);
        var temp = transform;
        List<string> selects = null;
        while (true && temp != null)
        {

            selects = paths.Where(x => Path.GetFileNameWithoutExtension(x) == temp.name).ToList();
            if (selects.Count() > 1)
            {
                count = 3;
                return;
            }
            else if (selects.Count() == 1)
                break;
            temp = temp.parent;
        }
        count = 4;
        if (temp)
        {
            var luapath = selects[0];
            UpdateLua();
            if (temp == transform) count = 1;
            else count = 2;
            if (path == luapath && !update) return;
            transform = temp;
            luaobjs.Clear();
            if (!update)
            {
                if (change)
                {
                    change = false;
                    //if (luatext != "")
                    //    if (EditorUtility.DisplayDialog("保持lua文件", "lua文件有修改,是否保存?", "保存", "不保存"))
                    //    {
                    //        UpdateLua(true);
                    //    }
                }
                luatext = File.ReadAllText(luapath);
            }
            path = luapath;
            asset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(relpath);
        }
        if (count == 4)
        {
            UpdateLua();
        }
        else if (count < 3 && luaobjs.Count < 1)
        {
            Regex r = new Regex(@":OnUIInit\(.*?\)([\s\S]*?)[\r\n]+end", RegexOptions.Multiline);
            var str = "";
            foreach (Match item in r.Matches(luatext))
            {
                str += item.Groups[1].Value;
            }
            var arr = str.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.None);
            var pre = "";
            foreach (var mes in arr)
            {
                if (mes.Trim().StartsWith("---@type"))
                {
                    pre = mes;
                    continue;
                }
                CreateDict(pre, mes, transform);
                pre = "";
            }
        }
        UpdateFunmes();
    }
    static Regex reg = new Regex(@"^\s*self\.([\w\d_]+)\s*=\s*(self.*|nil|null)$|^\s*self\.?([\w\d_]*)[\s;]*$|^\s*self:AddClick\((.*)\)[\s;]*$");
    static void CreateDict(string pre, string mes, Transform tran)
    {
        var match = reg.Match(mes);
        if (pre != "")
            mes = pre + "\r\n" + mes;
        if (!string.IsNullOrEmpty(match.Groups[4].Value))
        {
            var obj = new LuaObject("", mes, tran, match.Groups[4].Value);
            luaobjs.Add(obj);
        }
        if (!string.IsNullOrEmpty(match.Groups[3].Value))
        {
            var obj = new LuaObject(match.Groups[3].Value, mes, tran);
            luaobjs.Add(obj);
        }
        else if (!string.IsNullOrEmpty(match.Groups[1].Value))
        {
            var obj = new LuaObject(match.Groups[1].Value, mes, tran, match.Groups[2].Value);
            luaobjs.Add(obj);
        }
    }
    static void CreateDictArr(string mes)
    {

    }
	
    public static void UpdateLua(bool save = false)
    {
	
		if (luaobjs.Count > 0 && path != "")							
		{
			foreach (var item in luaobjs)
			{
				item.Update(ref luatext);
			}
		}
		UpdateFunmes();
		if (save)
        {
            addOnUIDestory(ref luatext);
            File.WriteAllText(path, luatext);
        }
    }
    private static void addOnUIDestory(ref string luatext){
      //  DebugLog.Log("~~~~~<>>~~~~~~ ",luatext);
            string des= "function " + ShowLuaInspectorRect.tagName + ":OnUIDestory()";
           	Regex r = new Regex(des+@"([\s\S]*?)[\r\n]+end", RegexOptions.Multiline);
			foreach (Match item in r.Matches(luatext))
			{
           //    DebugLog.Log("~~~~~~~~~~~~~~~~~ ",item.Groups[0].Value);
				var str = item.Groups[0].Value;
                if (string.IsNullOrEmpty(str)) continue;
           //      DebugLog.Log("@!#@#!@#@! ",str);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(des);
              //  sb.AppendLine("");
                foreach (var itemvv in luaobjs)
			    {
                  sb.AppendLine("self."+itemvv.Name+"=nil");
                }
                sb.AppendLine("end");
                string strnew=sb.ToString();
             //   DebugLog.Log(strnew);
                luatext=luatext.Replace(str,strnew);
                return;
            }
    }
    static void Clear()
    {
        luaobjs.Clear();
        luatext = "";
        path = "";
    }
    public static void OnInspectorGUI(Transform transform)
    {
        if (count == 3)
        {
            GUILayout.Label(Path.GetFileNameWithoutExtension(path) + " 该lua文件有重名");
        }
        else if (count == 1)
        {
			
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("LuaScript:" + Path.GetFileNameWithoutExtension(path));
            if (GUILayout.Button("选中"))
            {
                Selection.activeInstanceID = asset.GetInstanceID();
            }
            //if (GUILayout.Button("打开"))
            //{
            //    string editorPath = OpenLuaHelper.SubLimePath();
            //    System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //    proc.StartInfo.FileName = editorPath;
            //    proc.StartInfo.Arguments = string.Format("{0}:1:0", path);
            //    proc.Start();
            //}
            GUILayout.EndHorizontal();
            GUI.changed = false;
            LuaObject delobj = null;
            if (luaobjs.Count() < 1)
            {
                if (GUILayout.Button("+"))
                {
                    if (!luatext.Contains(":OnUIInit()"))
                    {
                        luatext += "\r\nfunction " + Path.GetFileNameWithoutExtension(path) + ":OnUIInit()\r\nend";
                    }
                    File.WriteAllText(path, luatext.Replace(":OnUIInit()", ":OnUIInit()\r\n    self.temp=nil"));
                    Clear();
                    OnEnable(transform);
                }
            }
            var update = false;
			for (int i = 0; i < luaobjs.Count; i++)
			{
				LuaObject item = luaobjs[i];
				if (item.isClick)
					return;
				GUILayout.BeginHorizontal();
				var name = GUILayout.TextField(item.Name, GUILayout.Width(100));
				if (!string.IsNullOrEmpty(name)) item.Name = name;
				var oldobj = item.Obj;
				item.Obj = EditorGUILayout.ObjectField(item.Obj, typeof(UnityEngine.Object), true, GUILayout.Width(100));
				if (oldobj == null && item.Obj != null)
				{
					item.Name = item.Obj.name;
				}
				if (item.Obj)
					item.TypeIndex = EditorGUILayout.Popup(item.TypeIndex, item.GetTypes().ToArray(), GUILayout.Width(60));
				if (GUILayout.Button("+", GUILayout.Width(30)))
				{
					Regex r = new Regex(@"[\d]+$");
					string addname = item.Name;
					int index = 1;
					if (r.IsMatch(addname))
						index = int.Parse(r.Match(addname).Value);
					addname = addname.Replace(index.ToString(), "");
					while (luaobjs.Where(x => x.Name == addname + index).Count() > 0) { index++; }
					item.Add("self." + addname + index + "=nil");
					UpdateLua();
					update = true;
				}
				if (GUILayout.Button("-", GUILayout.Width(30)))
				{
					item.Name = "";
					delobj = item;
				}
				GUILayout.EndHorizontal();
			}
			UpdateLua();
			//stopwatch.Stop();
			//Log.logError("Onenable耗时：" + count+"---" + stopwatch.ElapsedMilliseconds);
			//GUILayout.Label("点击事件");
			//foreach (var item in luaobjs.Where(x => x.isClick && x.Obj != null))
			//{
			//    GUILayout.BeginHorizontal();
			//    item.Obj = EditorGUILayout.ObjectField(item.Obj, typeof(UnityEngine.Object), true, GUILayout.Width(200));
			//    item.FunctionIndex = EditorGUILayout.Popup(item.FunctionIndex, funnames.ToArray(), GUILayout.Width(200));
			//    if (GUILayout.Button("+", GUILayout.Width(30)))
			//    {
			//        item.Add("self:AddClick(\"\",\"\")");
			//        UpdateLua();
			//        update = true;
			//    }
			//    //if (GUILayout.Button("-", GUILayout.Width(30)))
			//    //{
			//    //    item.Name = "";
			//    //    delobj = item;
			//    //}
			//    GUILayout.EndHorizontal();
			//}
			if (delobj != null && luaobjs.Contains(delobj))
                luaobjs.Remove(delobj);
            if (update) OnEnable(transform, true);
            GUILayout.BeginHorizontal();
            if (GUI.changed) { change = true; }
            if (GUILayout.Button("读取lua文件"))
            {
                change = false;
                Clear();
                OnEnable(transform);
            }
            //if (change) GUI.color = Color.red;
            if (GUILayout.Button("保存修改到lua"))
            {
                UpdateLua(true);
                change = false;
            }
            GUI.color = Color.white;
            GUILayout.EndHorizontal();
			
		}
        else
        {
           
        }
    }
}
