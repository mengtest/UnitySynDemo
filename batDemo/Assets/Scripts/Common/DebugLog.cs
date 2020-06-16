using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Internal;





public class DebugLog
{
    private static Queue<string> m_LogQueue;

	//普通log颜色
	const string _LuaColor = "<color=green>[" + "Lua-";
	const string _CSColor = "<color=green>[" + "CS-";
	//const string redSysColor = "<color=red>[" + "SysTem-:";
	//警告log颜色
	const string _WarningLuaColor = "<color=yellow>[" + "Lua-";
	const string _WarningCSColor = "<color=yellow>[" + "CS-";
	//const string yellowSysColor = "[<color=yellow>" + "SysTem-:";
	//错误log颜色
	const string _ErrorLuaColor = "<color=red>[" + "Lua-";
	const string _ErrorCSColor = "<color=red>[" + "CS-";
	//const string colorSysColor = "<color=blue>[" + "SysTem-:";
	const string _colorEnd = "]</color>  ";

    // 是否允许写log文件
    static public List<string> m_LogError = null;
    public delegate void gameErrorLogCall();
    static private gameErrorLogCall m_gameErrorLogFun;
   
    static StringBuilder _sb = new StringBuilder();
    public static bool needTime=true;
    //普通log开关
	public static bool isOpenLog = true;
	//错误log开关
	public static bool isOpenError = true;
	//警告log开关
	public static bool isOpenWarning = true;

    /// <summary>
    /// 一个错误回调
    /// </summary>
    /// <param name="fun"></param>
    public static void addErrorLogCall(gameErrorLogCall fun)
    {
        m_gameErrorLogFun = fun;
    }

    /// <summary>
    /// 关闭记录
    /// </summary>
    public static void Close()
    {

    }
    public static void setLogSwitcher(bool isOpenLog, bool isOpenError, bool isOpenWaring)
	{
		DebugLog.isOpenLog = isOpenLog;
		DebugLog.isOpenError = isOpenError;
		DebugLog.isOpenWarning = isOpenWaring;
	}

	public static void Log(params object[] logInfo)
	{
		if (logInfo == null || !isOpenLog)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		sb = _logBefore(_CSColor, sb.ToString());
		_log(LogType.Log, _sb.ToString());
	}	  
	public static void LogError(params object[] logInfo)
	{
		if (logInfo == null || !isOpenError)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		sb = _logBefore(_ErrorCSColor, sb.ToString());
		_log(LogType.Error, _sb.ToString());
	}
	public static void LogWarning(params object[] logInfo)
	{
		if (logInfo == null || !isOpenWarning)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		sb = _logBefore(_WarningCSColor, sb.ToString());
		_log(LogType.Warning, _sb.ToString());
	}

	static StringBuilder _logObjectArr(params object[] logInfo)
	{
		_sb.Clear();
		for (int i = 0; i < logInfo.Length; i++)
		{
			_sb.Append(logInfo[i] + "  ");
		}
		_sb.Remove(_sb.Length - 2, 2);
		return _sb;
	}
	static StringBuilder _logBefore(string color,string logInfo, UnityEngine.Object obj = null)
	{
		if (logInfo == null)
			return null;
		//获取当前堆栈信息
		StackTrace st = new StackTrace(true);
		StackFrame[] sf = st.GetFrames();
		string fileName;
		try
		{
			string[] str = sf[2].GetFileName().Split('\\');
			fileName = str[str.Length - 1] + ":" + sf[2].GetMethod().Name;
		}
		catch
		{
			//_log(LogType.Error, "找不到文件名");
            fileName = "";
			st = null;
			//return null;
		}
		_sb.Clear();
		st = null;
		_sb.Append(color);
		_sb.Append(fileName);
		_sb.Append(_colorEnd);
		_sb.Append(logInfo);
        if(needTime){
            DateTime now = DateTime.Now;
            string strTime = string.Format(" - {0}-{1}-{2} {3}:{4}:{5}.{6:000}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
           _sb.Append(strTime);
        }
		return _sb;
	}
	static void _log(LogType logType, string info,UnityEngine.Object obj =null)
	{
		if (logType == LogType.Log )
		{
			if(obj==null)
				UnityEngine.Debug.Log(info);
			else
				UnityEngine.Debug.Log(info, obj);
		}		
		else if (logType == LogType.Error)
		{
			if (obj == null)
				UnityEngine.Debug.LogError(info);
			else
				UnityEngine.Debug.LogError(info, obj);
		}
		else if (logType == LogType.Warning )
		{
			if (obj == null)
				UnityEngine.Debug.LogWarning(info);
			else
				UnityEngine.Debug.LogWarning(info, obj);
		}
	}

    public static void LogLua(string fileName,params object[] logInfo)
	{
      if (logInfo == null || !isOpenLog)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		Log_Lua(LogType.Log,fileName, _sb.ToString());
    }
    public static void LogErrorLua(string fileName,params object[] logInfo)
	{
      if (logInfo == null || !isOpenError)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		Log_Lua(LogType.Error,fileName, _sb.ToString());
    }
    public static void LogWarningLua(string fileName,params object[] logInfo)
	{
      if (logInfo == null || !isOpenWarning)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		Log_Lua(LogType.Warning,fileName, _sb.ToString());
    }
	/// <summary>
	///  logolua
	/// </summary>
	/// <param name="log类型"></param>
	/// <param name="信息所在文件名"></param>
	/// <param name="打印信息"></param>
	public static void Log_Lua(LogType logType, string fileName,string logInfo)
	{
		// if (fileName == null)
		// 	return;
		_sb.Clear();
		string color = null;
		if (logType == LogType.Log)
		{
			if (isOpenLog)
				color = _LuaColor;
			else
				return;
		}
		else if (logType == LogType.Warning)
		{
			if (isOpenWarning)
				color = _WarningLuaColor;
			else
				return;
		}
		else if (logType == LogType.Error)
		{
			if (isOpenError)
				color = _ErrorLuaColor;
			else
				return;
		}
		_sb.Append(color);
        if(fileName!=null){
	    	_sb.Append(fileName);
        }else{
            _sb.Append("err fileName");
        }
		_sb.Append(_colorEnd);
		_sb.Append(logInfo);
        if(needTime){
            DateTime now = DateTime.Now;
            string strTime = string.Format(" - {0}-{1}-{2} {3}:{4}:{5}.{6:000}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
           _sb.Append(strTime);
        }
		_log(logType, _sb.ToString());
	}

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 以下为私有接口

    private static string GetScriptFileName(string strFileLine)
    {
        if (strFileLine == null || strFileLine == "")
        {
            return "";
        }

        strFileLine = strFileLine.Replace("\\", "/");
        int pos = strFileLine.IndexOf("/Scripts/");
        string strResult = strFileLine.Substring(pos + 1, strFileLine.Length - pos - 1);
        return strResult;
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    private static void PushLog(string str)
    {
        if (m_LogQueue == null)
        {
            m_LogQueue = new Queue<string>();
        }

        m_LogQueue.Enqueue(str);

        if (m_LogQueue.Count > 100)
        {
            m_LogQueue.Dequeue();
        }
    }


    // 不常用接口
    public static void Break()
    {
        UnityEngine.Debug.Break();
    }

    public static void ClearDeveloperConsole()
    {
        UnityEngine.Debug.ClearDeveloperConsole();
    }

    public static void DebugBreak()
    {
        UnityEngine.Debug.DebugBreak();
    }

}

