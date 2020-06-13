using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Diagnostics;
using System;
//using Lua;
//未完待续
//[AutoRegistLua]
public class Log
{
	//普通log颜色
	const string _LuaColor = "<color=blue>[" + "Lua-";
	const string _CSColor = "<color=blue>[" + "CS-";
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
	static StringBuilder _sb = new StringBuilder();
	//普通log开关
	static bool _isOpenLog = true;
	//错误log开关
	static bool _isOpenError = true;
	//警告log开关
	 static bool _isOpenWarning = true;
	//public static void log(object[] param)
	//{
	//	if (param == null)
	//		return;
	//	StringBuilder sb = logArr(param);
	//	sb = _logBefore(CSColor, sb.ToString());
	//	_log(LogType.Log, sb.ToString());
	//}
	public static void setLogSwitcher(bool isOpenLog, bool isOpenError, bool isOpenWaring)
	{
		_isOpenLog = isOpenLog;
		_isOpenError = isOpenError;
		_isOpenWarning = isOpenWaring;
	}

	public static void log(params object[] logInfo)
	{
		if (logInfo == null || !_isOpenLog)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		sb = _logBefore(_CSColor, sb.ToString());
		_log(LogType.Log, _sb.ToString());
	}	  
	public static void logError(params object[] logInfo)
	{
		if (logInfo == null || !_isOpenError)
			return;
		StringBuilder sb = _logObjectArr(logInfo);
		sb = _logBefore(_ErrorCSColor, sb.ToString());
		_log(LogType.Error, _sb.ToString());
	}
	public static void logWarning(params object[] logInfo)
	{
		if (logInfo == null || !_isOpenWarning)
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
	//public static StringBuilder logArr(object[] param)
	//{
	//	//if (param == null)
	//	//	return null;
	//	//Type type = param.GetType();
	//	sb.Clear();
	//	for (int i = 0; i < param.Length; i++)
	//	{
	//		if (param[i].GetType() == typeof(string))
	//			sb.Append("\"" + param[i] + "\"  ");
	//		else if (param[i].GetType() == typeof(char))
	//			sb.Append("\'" + param[i] + "\'  ");
	//		else if (param[i].GetType() != typeof(bool))
	//			sb.Append(param[i] + "  ");
	//		else
	//			sb.Append(((bool)param[i] == true ? "true" : "false") + "  ");
	//	}
	//	sb.Remove(sb.Length - 2, 2);
	//	return sb;
	//}


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
		return _sb;
	}
	//public static void 
	public static void logError(string logInfo,UnityEngine.Object obj=null)
	{
		if (!_isOpenError)
			return;
		StringBuilder sb = _logBefore(_ErrorCSColor, logInfo, obj);
		if (sb != null)
			_log(LogType.Error, sb.ToString(),obj);
	}

	public static void log(string logInfo, UnityEngine.Object obj = null)
	{
		if (!_isOpenLog)
			return;
		StringBuilder sb = _logBefore(_CSColor, logInfo, obj);
		if (sb != null)
			_log(LogType.Log, sb.ToString(), obj);
	}
	//public static void logError(object[] param)
	//{
	//	if (param == null)
	//		return;
	//	StringBuilder sb = logArr(param);
	//	sb = _logBefore(ErrorCSColor, sb.ToString());
	//	_log(LogType.Log, sb.ToString());
	//}

	public static void logWarning(string logInfo,UnityEngine.Object obj =null)
	{
		if (!_isOpenWarning)
			return;
		StringBuilder sb = _logBefore(_WarningCSColor, logInfo, obj);
		if (sb != null)
			_log(LogType.Warning, sb.ToString(),obj);
	}

	//public static void logWarning(object[] param)
	//{
	//	if (param == null)
	//		return;
	//	StringBuilder sb = logArr(param);
	//	sb = _logBefore(WarningCSColor, sb.ToString());
	//	_log(LogType.Log, sb.ToString());
	//}

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
	/// <summary>
	///  ��ӡlua�ļ�����Ϣ
	/// </summary>
	/// <param name="log类型"></param>
	/// <param name="信息所在文件名"></param>
	/// <param name="打印信息"></param>
	public static void logLua(LogType logType, string fileName,string logInfo)
	{
		if (fileName == null)
			return;
		_sb.Clear();
		string color = null;
		if (logType == LogType.Log)
		{
			if (_isOpenLog)
				color = _LuaColor;
			else
				return;
		}
		else if (logType == LogType.Warning)
		{
			if (_isOpenWarning)
				color = _WarningLuaColor;
			else
				return;
		}
		else if (logType == LogType.Error)
		{
			if (_isOpenError)
				color = _ErrorLuaColor;
			else
				return;
		}
		_sb.Append(color);
		_sb.Append(fileName);
		_sb.Append(_colorEnd);
		_sb.Append(logInfo);
		_log(logType, _sb.ToString());
	}


	//static StringBuilder logArr<T>(T[] param)
	//{
	//	if (param == null || param.Length == 0)
	//		return null;
	//	//Type type = param[0].GetType();

	//	//if (type == typeof(bool))
	//	//{
	//	//	sb.Clear();
	//	//	for (int i = 0; i < param.Length; i++)
	//	//	{
	//	//		sb.Append(param[i] == true ? "true" : "false") + "--");
	//	//	}
	//	//	sb.Remove(sb.Length - 2, 2);
	//	//}
	//	//else
	//	//{
	//	//	if (param[i].GetType() == typeof(string))
	//	//		sb.Append("\"" + param[i] + "\"--");
	//	//	else if (param[i].GetType() == typeof(char))
	//	//		sb.Append("\'" + param[i] + "\'--");
	//	//	else if (param[i].GetType() != typeof(bool))
	//	//		sb.Append(param[i] + "--");
	//	//	else
	//	//}
	//	sb.Clear();
	//	for (int i = 0; i < param.Length; i++)
	//	{
	//		sb.Append(param[i] + "  ");
	//	}
	//	sb.Remove(sb.Length - 2, 2);
	//	return sb;
	//}

	//static void log<T>(T []param)
	//{
	//	if (param==null)
	//		return;
	//	StringBuilder sb = logArr<T>(param);
	//	sb = _logBefore(CSColor, sb.ToString());
	//	_log(LogType.Log, sb.ToString());
	//}
	//static void logWarning<T>(T[] param)
	//{
	//	if (param == null)
	//		return;
	//	StringBuilder sb = logArr<T>(param);
	//	sb = _logBefore(WarningCSColor, sb.ToString());
	//	_log(LogType.Log, sb.ToString());
	//}
	//static void logError<T>(T[] param)
	//{
	//	if (param == null)
	//		return;
	//	StringBuilder sb = logArr<T>(param);
	//	sb = _logBefore(ErrorCSColor, sb.ToString());
	//	_log(LogType.Log, sb.ToString());
	//}
	//监听异常信息打印
	public static void getException(StackTrace st)
	{
		if (st == null)
			return;
		_sb.Clear();
		StackFrame[] frames = st.GetFrames();
		try
		{
			int len = frames[0].GetFileName().Length;
			logError("文件名" + frames[0].GetFileName());
		}
		catch
		{
			return;
		}
	}
}
