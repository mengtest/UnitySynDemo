using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Internal;



public enum LogLevel
{
    /// 输出信息
    LogLevel_Info = 1,
    /// 错误信息
    LogLevel_Error = 2,
    /// 警告信息
    LogLevel_Warning = 3,
    /// 跟踪信息
    LogLevel_Log = 4,
};

public class DebugLog
{
    private static Queue<string> m_LogQueue;
    /// <summary>
    /// 游戏报错
    /// </summary>
    public static string GAME_ERROR = "GAME_ERROR";

    // 是否打开Log
    static public bool EnableLog = true;
    // 允许输出的最大Log等级
    static public LogLevel MaxLogLevel = LogLevel.LogLevel_Log;
    // 是否允许写log文件
    static public List<string> m_LogError = null;
    public delegate void gameErrorLogCall();
    static private gameErrorLogCall m_gameErrorLogFun;
   

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

    static public void LogInfo(string strFormat,object message=null, params object[] args)
    {
        if (!EnableLog)
        {
            return;
        }
        if (LogLevel.LogLevel_Info > MaxLogLevel)
        {
            return;
        }
        string strFileLine = "";
        StackTrace insStackTrace = new StackTrace(true);
        if (insStackTrace != null)
        {
            StackFrame insStackFrame = insStackTrace.GetFrame(1);
            if (insStackFrame != null)
            {
                strFileLine = String.Format("{0}({1})", insStackFrame.GetFileName(), insStackFrame.GetFileLineNumber());
                strFileLine = GetScriptFileName(strFileLine);
            }
        }
        Log_s(LogLevel.LogLevel_Info, strFileLine, strFormat, message, args);
    }
    static public void LogError(string strFormat,object message=null, params object[] args)
    {
        if (!EnableLog)
        {
            return;
        }
        if (LogLevel.LogLevel_Error > MaxLogLevel)
        {
            return;
        }
        string strFileLine = "";
        StackTrace insStackTrace = new StackTrace(true);
        if (insStackTrace != null)
        {
            StackFrame insStackFrame = insStackTrace.GetFrame(1);
            if (insStackFrame != null)
            {
                strFileLine = String.Format("{0}({1})", insStackFrame.GetFileName(), insStackFrame.GetFileLineNumber());
                strFileLine = GetScriptFileName(strFileLine);
            }
        }
        Log_s(LogLevel.LogLevel_Error, strFileLine, strFormat,message, args);
    }
    static public void LogWarning(string strFormat,object message=null ,params object[] args)
    {
        if (!EnableLog)
        {
            return;
        }
        if (LogLevel.LogLevel_Warning > MaxLogLevel)
        {
            return;
        }
        string strFileLine = "";
        StackTrace insStackTrace = new StackTrace(true);
        if (insStackTrace != null)
        {
            StackFrame insStackFrame = insStackTrace.GetFrame(1);
            if (insStackFrame != null)
            {
                strFileLine = String.Format("{0}({1})", insStackFrame.GetFileName(), insStackFrame.GetFileLineNumber());
                strFileLine = GetScriptFileName(strFileLine);
            }
        }
        Log_s(LogLevel.LogLevel_Warning, strFileLine, strFormat,message, args);
    }
    static public void Log(string strFormat,object message=null, params object[] args)
    {
        if (!EnableLog)
        {
            return;
        }
        if (LogLevel.LogLevel_Log > MaxLogLevel)
        {
            return;
        }
        string strFileLine = "";
        StackTrace insStackTrace = new StackTrace(true);
        if (insStackTrace != null)
        {
            StackFrame insStackFrame = insStackTrace.GetFrame(1);
            if (insStackFrame != null)
            {
                strFileLine = String.Format("{0}({1})", insStackFrame.GetFileName(), insStackFrame.GetFileLineNumber());
                strFileLine = GetScriptFileName(strFileLine);
            }
        }
        Log_s(LogLevel.LogLevel_Log, strFileLine, strFormat,message, args);
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

    static private void Log_s(LogLevel level, string strFileLine, string strFormat, object message,params object[] args)
    {
        if (!EnableLog)
        {
            return;
        }

        if (level > MaxLogLevel)
        {
            return;
        }

        // 添加时间参数
        DateTime now = DateTime.Now;
        string strTime = string.Format("{0}-{1}-{2} {3}:{4}:{5}.{6:000}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
        string strLog = "";
        string strOutLog = args.Length > 0 ? string.Format(strFormat, args) : strFormat;
        switch (level)
        {
            case LogLevel.LogLevel_Info:
                strLog = string.Format("{0}{1} {2} - {3}", "[Info]", strOutLog, strFileLine, strTime);
                UnityEngine.Debug.Log(strLog); 
                if(message!=null){
                     UnityEngine.Debug.Log(message);
                }  
                break;
            case LogLevel.LogLevel_Log:
                strLog = string.Format("{0}{1} {2} - {3}", "[Log]", strOutLog, strFileLine, strTime);
                UnityEngine.Debug.Log(strLog);
                if(message!=null){
                     UnityEngine.Debug.Log(message);
                }
                break;
            case LogLevel.LogLevel_Error:
                strLog = string.Format("{0}{1} {2} - {3}", "[Error]", strOutLog, strFileLine, strTime);
                UnityEngine.Debug.LogError(strLog);
                if(message!=null){
                   UnityEngine.Debug.LogError(message);
                }
                break;
            case LogLevel.LogLevel_Warning:
                strLog = string.Format("{0}{1} {2} - {3}", "[Warning]", strOutLog, strFileLine, strTime);
                UnityEngine.Debug.LogWarning(strLog);
                if(message!=null){
                     UnityEngine.Debug.LogWarning(message);
                }
                break;
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

