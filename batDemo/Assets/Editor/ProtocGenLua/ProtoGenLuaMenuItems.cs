using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

public class ProtoGenLuaMenuItems
{
    static List<string> files = new List<string>();

    [MenuItem("PB��������/��PB�ļ�λ��")]
    public static void OpenDirForPb()
    {
        OpenDirectory(Application.dataPath.Replace("Assets", "Tools/GameProto"), false);
        //string tempProtoPath = Application.dataPath + "/ScriptsLua/PB";
        //string tempCmdpkgPath = tempProtoPath + "/Cmdpkg.bytes";
        //string tempCmdpkgTargetPath = Application.dataPath + "/Cmdpkg.bytes";
        //string tempMbPath = tempProtoPath + "/md.bytes";
        //string tempMbTargetPath = Application.dataPath + "/md.bytes";
        ////if (File.Exists(tempCmdpkgTargetPath))
        ////{
        //    File.Copy(tempCmdpkgPath.Replace("/", "\\"), tempCmdpkgTargetPath.Replace("/", "\\"), true);
        //    File.Copy(tempMbPath.Replace("/", "\\"), tempMbTargetPath.Replace("/", "\\"), true);
        //}
        //else
        //{
        //    File.Copy(tempCmdpkgPath.Replace("/", "\\"), tempCmdpkgTargetPath.Replace("/", "\\"));
        //    File.Copy(tempMbPath.Replace("/", "\\"), tempMbTargetPath.Replace("/", "\\"));
        //}
    }
    public static void OpenDirectory(string path, bool create = false)
    {
        if (string.IsNullOrEmpty(path)) return;

        path = ChangeToSystemPath(path);
        if (!Directory.Exists(path))
        {
            if (create)
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                Log.logError("No Directory: " + path);
                return;
            }
        }
        System.Diagnostics.Process.Start("explorer.exe", path);
    }
    public static string ChangeToSystemPath(string path)
    {
        path = path.Replace("/", "\\");
        return path;
    }
    [MenuItem("PB��������/Build Protobuf-lua-gen File ����")]
    public static void BuildProtobufFileMain()
    {
        _BuildProtoFile("/main");
    }
    [MenuItem("PB��������/Build Protobuf-lua-gen File ������")]
    public static void BuildProtobufFileMSJ()
    {
        _BuildProtoFile("/msj");
    }
    [MenuItem("PB��������/Build Protobuf-lua-gen File ɣ����")]
    public static void BuildProtobufFileSZH()
    {
        _BuildProtoFile("/szh");
    }
    [MenuItem("PB��������/Build Protobuf-lua-gen File ���")]
    public static void BuildProtobufFilePC()
    {
        _BuildProtoFile("/pc");
    }

    private static void _BuildProtoFile(string canPath)
    {
        _CheckMd5Boo = EditorUtility.DisplayDialog("����Porot��ʾ", "�Ƿ�ѡ��ʹ����������", "YES", "NO");

        EditorUtility.DisplayProgressBar("Proto����", "��ʼ����", 0.1f);

        string AppDataPath = Application.dataPath;
        string dir = AppDataPath + "/ScriptsLua/PB";
        //string pbPath = Application.dataPath.Replace("Assets", "") + "Tools/GameProto";
        string pluginPath = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua/plugin";//proto�Ĺ���λ��
        string protoExePath = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua/plugin";//proto�Ĺ���λ��
        //string serverPath = Application.dataPath.Replace("Assets", "").Replace("gunFire/", "") + "server/SparkGameGO/src/sparkgame.com/miniworld/proto/ProtoSource/";//һ��ʼʹ�õ�proto����λ��
        string serverPath2 = Application.dataPath.Replace("Assets", "").Replace("gunFire/", "") + "server/SparkGameGO/src/sparkgame.com/miniworld/vendor/sparkgame.com/gframe/protoSource";//�������ӵ�λ����Ϣ
        string serverPath3 = Application.dataPath.Replace("Assets", "Tools") + "/GameProto"+ canPath;

        files.Clear();

        EditorUtility.DisplayProgressBar("Proto����", "����ļ�����", 0.3f);

        //List<ProtoFileInfo> tempFileInfoList = _GetProtoFiles(serverPath,serverPath2);
        List<ProtoFileInfo> tempFileInfoList = _GetProtoFilesForFloder(serverPath3, serverPath2);

        string tempStrPath = Application.dataPath + "/ScriptsLua/pb/md.bytes";
        if (System.IO.File.Exists(tempStrPath))
        {
            File.Delete(tempStrPath);
        }

        string tempCmdPath = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua/create_protoc-pb-"+canPath.Replace("/","") + ".cmd";
        string tempProtoSoucePath = "../../GameProto" + canPath;
        string tempOutPath = "../../GameProto";
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = tempCmdPath;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = true;
        info.WorkingDirectory = Application.dataPath.Replace("Assets", "") + "Tools/protoc-gen-lua";
        info.ErrorDialog = true;
        System.Diagnostics.Process task = null;
        try
        {
            task = Process.Start(info);
            if (task != null)
            {
                task.WaitForExit(1000);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("e");
        }
        finally
        {
            if (task != null && task.HasExited)
            {

            }
        }
        //if (_CheckMd5Boo)
        //{
        //    files = _CheckFileMd5(tempFileInfoList);
        //}
        //else
        //{
        //    files = _NoCheckMd5(tempFileInfoList);
        //}

        //EditorUtility.DisplayProgressBar("Proto����", "��������proto��Ӧ��lua�ļ�", 0.5f);

        //string protoc = string.Empty;
        //string protoc_gen_dir = string.Empty;
        //string args = string.Empty;
        //string output = dir;
        //bool isWin = true;

        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    protoc = protoExePath + "/protoc.exe";
        //    protoc_gen_dir = "\"" + pluginPath + "/protoc-gen-lua.bat\"";
        //}
        //else if (Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    protoc = "/usr/local/bin/protoc";
        //    protoc_gen_dir = "/usr/local/bin/protoc-gen-lua";
        //    isWin = false;
        //}
        //args = " --proto_path=" + serverPath3 + " --lua_out=" + output + " --plugin=protoc-gen-lua=" + protoc_gen_dir;

        //foreach (string f in files)
        //{
        //    string name = Path.GetFileName(f);
        //    string ext = Path.GetExtension(f);
        //    string fullName = Path.GetFullPath(f);
        //    UnityEngine.Debug.Log("fullName : " + fullName);
        //    if (!ext.Equals(".proto")) continue;

        //    ProcessStartInfo info = new ProcessStartInfo();
        //    info.FileName = protoc;
        //    if (f.IndexOf(serverPath3) < 0)
        //    {
        //        info.Arguments = args = " --proto_path=" + serverPath2 + " --lua_out=" + output + " --plugin=protoc-gen-lua=" + protoc_gen_dir + " " + name;
        //    }
        //    else
        //    {
        //        info.Arguments = args + " " + name;
        //    }
        //    info.WindowStyle = ProcessWindowStyle.Hidden;
        //    info.UseShellExecute = isWin;
        //    info.WorkingDirectory = dir;
        //    info.ErrorDialog = true;
        //    UnityEngine.Debug.Log(info.FileName + " " + info.Arguments);

        //    Process pro = Process.Start(info);
        //    pro.WaitForExit();
        //}

        EditorUtility.DisplayProgressBar("Proto����", "��ʼ��ȡproto�ļ����ݣ���дpb_list", 0.7f);

        Dictionary<string, List<ProtoFileData>> tempFileDataList = _GetProtoFileContext(tempFileInfoList);

        _Writepb_list(tempFileInfoList, tempFileDataList);

        EditorUtility.DisplayProgressBar("Proto����", "��дpb_list���", 0.99f);

        EditorUtility.ClearProgressBar();

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("���", "����proto��lua�ļ����", "OK");
    }

    /// <summary>
    /// pb_list.lua·��
    /// </summary>
    private static string _Md5FilePath = "Assets/ScriptsLua/PB/pb_list.lua";
    /// <summary>
    /// �Ƿ�ʹ��MD5��������
    /// </summary>
    private static bool _CheckMd5Boo = true;

    /// <summary>
    /// ����ļ���MD5ֵ���и���
    /// </summary>
    private static List<string> _CheckFileMd5(List<ProtoFileInfo> canFileInfos)
    {
        StreamReader sr = new StreamReader(_Md5FilePath);
        string all = sr.ReadToEnd();
        string[] s = all.Split('\n');
        bool tempMD5Boo = false;

        Dictionary<string, string> md5Dic = new Dictionary<string, string>();
        foreach(string tempStr in s)
        {
            if(tempStr.Equals("----MD5----\r"))
            {
                tempMD5Boo = true;
                continue;
            }
            if (!tempMD5Boo) continue;
            if (tempStr.Equals(string.Empty)) continue;
            string[] tempStrList = tempStr.Split('|');
            
            md5Dic.Add(tempStrList[0].Replace("--",""), tempStrList[1]);
        }
        List<string> tempReturnList = new List<string>();
        foreach(ProtoFileInfo tempInfo in canFileInfos)
        {
            string tempMd5;
            string tempStr = tempInfo.filePath.Substring(tempInfo.filePath.IndexOf("ProtoSource") + 12);
            if (md5Dic.TryGetValue(tempStr, out tempMd5)&&tempMd5 != null)
            {
                string cmStr = tempInfo.md5Str + "\r";
                if(!cmStr.Equals(tempMd5))
                {
                    tempReturnList.Add(tempInfo.filePath);
                }
            }
            else
            {
                tempReturnList.Add(tempInfo.filePath);
            }
        }
        sr.Close();
        sr.Dispose();
        return tempReturnList;
    }

    /// <summary>
    /// ������ļ�MD5
    /// </summary>
    private static List<string> _NoCheckMd5(List<ProtoFileInfo> canFileInfos)
    {
        List<string> tempReturnList = new List<string>();
        foreach (ProtoFileInfo tempInfo in canFileInfos)
        {
            tempReturnList.Add(tempInfo.filePath);
        }
        return tempReturnList;
    }

    /// <summary>
    /// ��ȡ���еķ�������Proto�ļ���Ϣ
    /// </summary>
    /// <param name="canDataPath">�����Proto�ļ���·��</param>
    private static List<ProtoFileInfo> _GetProtoFiles(string canDataPath,string canDataPath2)
    {
        List<ProtoFileInfo> tempReturnList = new List<ProtoFileInfo>();
        string copyTextPath = canDataPath + "copy.txt";
        string copyStr = File.ReadAllText(copyTextPath);

        string[] copyStrArr = copyStr.Split(Environment.NewLine.ToCharArray());

        foreach (string str in copyStrArr)
        {
            string fileName = str + ".proto";
            string fullName = canDataPath + fileName;
            if (File.Exists(fullName))
            {
                ProtoFileInfo curFileInfo = new ProtoFileInfo();
                curFileInfo.fileName = fileName.Substring(0, fileName.IndexOf("."));
                curFileInfo.md5Str = FileUtil.md5file(fullName);
                curFileInfo.filePath = fullName.Replace('\\', '/');
                tempReturnList.Add(curFileInfo);
            }
        }

		string filename = "cmdpkg.proto";
		string fullname = canDataPath2 + "\\" + filename;
		if (File.Exists(fullname))
		{
			ProtoFileInfo curFileInfo = new ProtoFileInfo();
			curFileInfo.fileName = filename.Substring(0, filename.IndexOf("."));
			curFileInfo.md5Str = FileUtil.md5file(filename);
			curFileInfo.filePath = fullname.Replace('\\', '/');
			tempReturnList.Add(curFileInfo);
		}
		return tempReturnList;
    }

    private static List<ProtoFileInfo> _GetProtoFilesForFloder(string canServerPath,string canServerPath2)
    {
        List<string> fileList = new List<string>();
        List<ProtoFileInfo> tempReturnList = new List<ProtoFileInfo>();

        fileList.AddRange(Directory.GetFiles(canServerPath, "*", SearchOption.AllDirectories));
        fileList.Sort();
        for (int i = 0; i < fileList.Count; i++)
        {
            string fullName = fileList[i];
            string fileName = fullName.Substring(fullName.LastIndexOf("\\") + 1);
            if (!(fileName.EndsWith(".proto")))
            {
                continue;
            }
            if (File.Exists(fullName))
            { 
                ProtoFileInfo curFileInfo = new ProtoFileInfo();
                curFileInfo.fileName = fileName.Substring(0, fileName.IndexOf("."));
                curFileInfo.md5Str = FileUtil.md5file(fullName);
                curFileInfo.filePath = fullName.Replace('\\', '/');
                tempReturnList.Add(curFileInfo);
            }
        }

        string filename = "cmdpkg.proto";
        string fullname = canServerPath2 + "\\" + filename;
        if (File.Exists(fullname))
        {
            ProtoFileInfo curFileInfo = new ProtoFileInfo();
            curFileInfo.fileName = filename.Substring(0, filename.IndexOf("."));
            curFileInfo.md5Str = FileUtil.md5file(fullname);
            curFileInfo.filePath = fullname.Replace('\\', '/');
            tempReturnList.Add(curFileInfo);
        }
        return tempReturnList;
    }

    /// <summary>
    /// ͨ���ļ���Ϣ�����������ת������Ҫʹ�õĸ�ʽ
    /// </summary>
    private static Dictionary<string, List<ProtoFileData>> _GetProtoFileContext(List<ProtoFileInfo> canFileInfos)
    {
        List<ProtoFileData> tempReturnProtoMessageFileData = new List<ProtoFileData>();
        List<ProtoFileData> tempReturnProtoEnumFileData = new List<ProtoFileData>();
        foreach (ProtoFileInfo tempFileInfo in canFileInfos)
        {
            StreamReader sr = new StreamReader(tempFileInfo.filePath);
            string all = sr.ReadToEnd();
            string[] tempLines = all.Split('\n');

            //��һ���Ƿ��Ǵ�����ע��
            bool tempNextNoteBoo = false;
            //��һ���Ƿ��Ǵ���������
            bool tempNextMessageBoo = false;

            //��һ���Ƿ���С��������
            bool tempNextType = false;
            //��һ���Ƿ���С��������
            bool tempNextTypeName = false;
            //��һ���Ƿ���С����ע��
            bool tempNextTypeNote = false;
            //��һ���Ƿ��ȡС����ע��
            bool tempLastGetNextTypeNote = false;

            //�Ƿ��ȡһ��С���ͽ���
            bool tempNextGetTypeEndBoo = false;
            //�Ƿ��ȡ��һ�������ͽ���
            bool tempNextGetMessageEndBoo = false;

            //�Ƿ���ӽ�ö������ע������
            bool tempIsEnumTypeBoo = false;

            ProtoFileData curProtoData = new ProtoFileData();
            curProtoData.fileName = tempFileInfo.fileName;
            string tempMsgNote = "";
            string tempTypeNote = "";
            string tempType = "";
            foreach (string tempCurLine in tempLines)//����������
            {
                if (tempNextGetMessageEndBoo)//�ж��ǲ��Ǵ����ͽ����������ʹ���һ���µ�������Ϣ��
                {
                    curProtoData = new ProtoFileData();
                    curProtoData.fileName = tempFileInfo.fileName;
                    tempNextGetMessageEndBoo = false;
                }
                char[] tempChar = { ' ', '\t' };
                string tempReplaceStr = tempCurLine.Replace('=', ' ');
                string[] tempStr = tempReplaceStr.Split(tempChar, options: StringSplitOptions.RemoveEmptyEntries);//���ݿո��з���������
                ///�и���������һ���ֶ�
                int tempLineEnd = tempStr.Length - 1;
                for (int i = 0; i < tempStr.Length; i++)//�����зֵ���������
                {
                    string tempValue = tempStr[i].Trim();
                    string tempValue2 = tempStr[i];
                    if (tempValue == string.Empty) continue;//�ռ���

                    if (tempValue == "repeated")
                        continue;

                    #region ����ע��
                    if (tempValue == "//" && !tempNextType)//�Ƿ���ڡ�//�������Ҳ���С���ͻ�ȡʱ�򣬽��������ע�ͻ�ȡ
                    {
                        if (i != tempLineEnd)//�жϲ��Ҳ������һ�У���������ע��
                        {
                            tempNextNoteBoo = true;
                        }
                        continue;
                    }

                    if(tempValue.IndexOf("//") >= 0 && !tempNextType)//�ж��Ƿ���С�//�������Ҳ���С���ͻ�ȡʱ�򣬽��������ע�ͻ�ȡ
                    {
                        tempMsgNote = tempMsgNote + tempValue.Replace("//", "");
                        if (i == tempLineEnd)//�Ƿ������һ���ֶΣ�Ȼ��رմ���ע��
                        {
                            curProtoData.messageNote = tempMsgNote;
                            tempMsgNote = string.Empty;
                            tempNextNoteBoo = false;
                            continue;
                        }
                    }

                    if (tempNextNoteBoo)
                    {
                        tempMsgNote = tempMsgNote + tempValue.Replace("//", "");
                        if (i == tempLineEnd)//�Ƿ������һ���ֶΣ�Ȼ��رմ���ע��
                        {
                            curProtoData.messageNote = tempMsgNote;
                            tempMsgNote = string.Empty;
                            tempNextNoteBoo = false;
                            continue;
                        }
                    }
                    #endregion


                    if (tempValue == "message")//�ж��Ƿ��Ǵ�������
                    {
                        tempNextMessageBoo = true;
                        tempIsEnumTypeBoo = false;
                        continue;
                    }

                    if(tempValue == "enum")//�ж��Ƿ��Ǵ���ö������
                    {
                        tempNextMessageBoo = true;
                        tempIsEnumTypeBoo = true;
                        continue;
                    }

                    if (tempNextMessageBoo)//����������ͣ����ҿ���С������
                    {
                        tempNextMessageBoo = false;
                        curProtoData.messageName = tempValue.Replace("{", "");
                        tempNextType = true;
                        tempNextGetTypeEndBoo = true;
                        continue;
                    }

                    if (tempValue == "{")
                    {
                        continue;
                    }

                    if (tempValue == "}")//�ж��Ƿ��Ѿ�ѭ����һ���������͵�����
                    {
                        if (tempNextType)
                        {
                            tempNextType = false;
                            tempNextGetMessageEndBoo = true;
                            if (tempIsEnumTypeBoo)//�������ͻ���ö������������
                            {
                                tempReturnProtoEnumFileData.Add(curProtoData);
                            }
                            else
                            {
                                tempReturnProtoMessageFileData.Add(curProtoData);
                            }
                        }
                    }

                    if (tempNextType && tempNextGetTypeEndBoo)//�ж��Ƿ���С�����ͣ��Լ��Ƿ��ȡ��һ��С������
                    {
                        if(tempValue == "//")//�ж��Ƿ���ڡ�//�����Ƿ���С��ע�ͣ�
                        {
                            tempNextTypeNote = true;
                            tempLastGetNextTypeNote = true;
                            continue;
                        }

                        if (tempValue.IndexOf("//") >= 0)//�ж��Ƿ���С�//�����Ƿ���С��ע�ͣ�
                        {
                            tempNextTypeNote = true;
                            tempLastGetNextTypeNote = true;
                            tempTypeNote = tempTypeNote + tempValue.Replace("//", "");
                            //if (tempValue2.IndexOf("\r") >= 0)
                            //{
                            //    tempNextTypeNote = false;
                            //}
                            if (i==tempLineEnd)//�ж��Ƿ������һ���ֶΣ��ر�С��ע��
                            {
                                tempNextTypeNote = false;
                            }
                            continue;
                        }

                        if (tempNextTypeNote)//�������С��ע�ͣ��������ע��
                        {
                            tempTypeNote = tempTypeNote + tempValue;
                            //if (tempValue2.IndexOf("\r") >= 0)
                            //{
                            //    tempNextTypeNote = false;
                            //}
                            if (i == tempLineEnd)//�ж��Ƿ������һ���ֶΣ��ر�С��ע��
                            {
                                tempNextTypeNote = false;
                            }
                            continue;
                        }

                        if (tempValue.IndexOf("<") >= 0)
                        {
                            tempType = tempType + tempValue;
                            if(tempValue.IndexOf(">")>=0)
                            {
                                tempNextGetTypeEndBoo = false;
                                curProtoData.messageType.Add(tempType.Replace("=", ""));
                                tempType = "";
                                tempNextTypeName = true;
                            }
                            continue;
                        }

                        tempType = tempType + tempValue;
                        tempNextGetTypeEndBoo = false;
                        curProtoData.messageType.Add(tempType.Replace("=", ""));
                        tempType = "";
                        tempNextTypeName = true;

                        if (tempLastGetNextTypeNote && tempIsEnumTypeBoo)
                        {
                            if (!curProtoData.messageTypeNoteDic.ContainsKey(tempValue))
                            {
                                curProtoData.messageTypeNoteDic.Add(tempValue, tempTypeNote);
                            }
                            tempTypeNote = string.Empty;
                            tempLastGetNextTypeNote = false;
                        }

                        continue;
                    }

                    if (tempNextTypeName)
                    {
                        if (tempLastGetNextTypeNote && !tempIsEnumTypeBoo)
                        {
                            if (!curProtoData.messageTypeNoteDic.ContainsKey(tempValue))
                            {
                                curProtoData.messageTypeNoteDic.Add(tempValue, tempTypeNote);
                            }
                            tempTypeNote = string.Empty;
                            tempLastGetNextTypeNote = false;
                        }
                        if(tempIsEnumTypeBoo)
                        {
                            if (tempValue == "=")
                                continue;
                            if(tempValue.IndexOf("=") >= 0 || tempValue.IndexOf(";")>=0)
                            {
                                tempNextTypeName = false;
                                if(tempValue.IndexOf(";") >= 0)
                                {
                                    curProtoData.messageTypeName.Add(tempValue.Replace("=", "").Replace(";",""));
                                    tempNextGetTypeEndBoo = true;
                                }
                                else
                                {
                                    curProtoData.messageTypeName.Add(tempValue.Replace("=", ""));
                                }
                                continue;
                            }
                        }
                        else
                        {
                            tempNextTypeName = false;
                            curProtoData.messageTypeName.Add(tempValue.Replace("=", ""));
                            continue;
                        }
                    }

                    if (tempValue.IndexOf(";") >0 )
                    {
                        tempNextGetTypeEndBoo = true;
                    }
                }
            }
            sr.Close();
            sr.Dispose();
        }
        Dictionary<string, List<ProtoFileData>> tempReturnDic = new Dictionary<string, List<ProtoFileData>>();
        tempReturnDic.Add("msg", tempReturnProtoMessageFileData);
        tempReturnDic.Add("enum", tempReturnProtoEnumFileData);
        return tempReturnDic;
    }

    /// <summary>
    /// ͨ���ļ���Ϣ�����±�дpb_list�ļ�
    /// </summary>
    private static bool _Writepb_list(List<ProtoFileInfo> canFileInfos, Dictionary<string, List<ProtoFileData>> canFileDatas)
    {
        string pb_list_path = Application.dataPath + "/ScriptsLua/PB/pb_list.lua";
        if (System.IO.File.Exists(pb_list_path))
        {
            File.Delete(pb_list_path);
        }
        StreamWriter sw;
        var u8WithoutBom = new System.Text.UTF8Encoding(false);
        sw = new StreamWriter(pb_list_path, false, u8WithoutBom);

        sw.WriteLine("--Auto Create , Don't Edit--");
        sw.WriteLine("--If you have any questions, please contact ma huibao--");
        sw.Write("\n");
        sw.WriteLine("local tempPath1 = Util.GetProtoBytesPath()..\"Cmdpkg.bytes\"");
        sw.WriteLine("local tempPath2 = Util.GetProtoBytesPath()..\"md.bytes\"");
        sw.WriteLine("assert(pb.loadfile(tempPath1))");
        sw.WriteLine("assert(pb.loadfile(tempPath2))");
        //foreach(ProtoFileInfo tempInfo in canFileInfos)
        //{
        //    sw.WriteLine("require " + "\"PB." + tempInfo.fileName + "_pb\"");
        //}

        List<ProtoFileData> tempFileData;
        canFileDatas.TryGetValue("enum", out tempFileData);
        ProtoFileData tempCMDFileData = null;
        if(tempFileData != null)
        {
            foreach (ProtoFileData tempData in tempFileData)
            {
                sw.WriteLine("---" + tempData.messageNote);
                sw.WriteLine("---@class " + tempData.messageName);
                sw.WriteLine(tempData.messageName + " = {");
                for (int i = 0; i < tempData.messageType.Count; i++)
                {
                    string tempNote;
                    if (tempData.messageTypeNoteDic.TryGetValue(tempData.messageType[i], out tempNote) && tempNote != null)
                    {

                    }
                    else
                    {
                        tempNote = "";
                    }
                    sw.WriteLine("    ---@type number " + tempNote);                                        
                    sw.WriteLine("    " + tempData.messageType[i] + " = " + tempData.messageTypeName[i] + ",");
                }
                sw.WriteLine("}");
                if (tempData.messageName== "GAME_CMD")
                {
                    tempCMDFileData = tempData;
                }
            }
        }

        tempFileData = null;
        canFileDatas.TryGetValue("msg", out tempFileData);

        Dictionary<string, string> tempMsgNameDic = new Dictionary<string, string>();
        foreach (ProtoFileData tempData in tempFileData)
        {
            tempMsgNameDic.Add(tempData.messageName.ToUpper(), tempData.messageName);
        }

        if (tempCMDFileData!=null)
        {
            sw.WriteLine("MsgIDMap = {");
            for (int i = 0; i < tempCMDFileData.messageType.Count; i++)
            {
                string tempNote;
                if (tempCMDFileData.messageTypeNoteDic.TryGetValue(tempCMDFileData.messageType[i], out tempNote) && tempNote != null)
                {

                }
                else
                {
                    tempNote = "";
                }
                string tempMsgUPName = tempCMDFileData.messageType[i].Substring(9).ToUpper();
                string tempWriteName;
                if (tempMsgNameDic.TryGetValue(tempMsgUPName, out tempWriteName) && tempWriteName != null)
                {
                    sw.WriteLine("    ---@type string " + tempNote);
                    sw.WriteLine("    [" + tempCMDFileData.messageTypeName[i] + "] = \"md." + tempWriteName + "\",");
                    sw.WriteLine("    ---@type string " + tempNote + " ����˷���");
                    sw.WriteLine("    [-" + tempCMDFileData.messageTypeName[i] + "] = \"md." + tempWriteName + "Rsp\",");
                }
            }
            sw.WriteLine("}");
        }

        sw.Write("\n");
        sw.WriteLine("if true then return  end");
        sw.Write("\n");

        foreach (ProtoFileInfo tempInfo in canFileInfos)
        {
            sw.WriteLine("---@class " + tempInfo.fileName + "_pb");
            sw.WriteLine(tempInfo.fileName + "_pb = {}");
        }
        if (tempFileData == null) return false;
        foreach(ProtoFileData tempData in tempFileData)
        {
            string tempFileNameStr = tempData.fileName + "_pb";
            string tempMessageReturnData = tempFileNameStr+"_" + tempData.messageName + "Data";
            sw.WriteLine("---@class " + tempMessageReturnData);
            sw.WriteLine(tempMessageReturnData + " = {");
            for(int i =0;i<tempData.messageType.Count;i++)
            {
                string tempNote;
                if(tempData.messageTypeNoteDic.TryGetValue(tempData.messageTypeName[i], out tempNote) && tempNote !=null)
                {

                }
                else
                {
                    tempNote = "";
                }
                sw.WriteLine("    ---@type " + tempData.messageType[i] + " "+ tempNote);
                sw.WriteLine("    "+tempData.messageTypeName[i] + " = nil,");
            }
            sw.WriteLine("}");

            sw.WriteLine("---" + tempData.messageNote);
            sw.WriteLine("---@return " + tempMessageReturnData);
            sw.WriteLine("function " + tempFileNameStr + "." + tempData.messageName + "()");
            sw.WriteLine("end");
        }
        sw.WriteLine("----MD5----");
        foreach (ProtoFileInfo tempInfo in canFileInfos)
        {
            string tempStr = tempInfo.filePath.Substring(tempInfo.filePath.IndexOf("ProtoSource") + 12);
            sw.WriteLine("--" + tempStr + "|"+tempInfo.md5Str);
        }
        sw.Flush();
        sw.Close();
        sw.Dispose();
        return true;
    }

    /// <summary>
    /// proto�ļ���Ϣ
    /// </summary>
    public class ProtoFileInfo
    {
        /// <summary>
        /// �ļ���
        /// </summary>
        public string fileName;
        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string filePath;
        /// <summary>
        /// �ļ�MD5ֵ
        /// </summary>
        public string md5Str;
    }

    /// <summary>
    /// proto�ļ�����Ϣ
    /// </summary>
    public class ProtoFileData
    {
        /// <summary>
        /// �����ļ���
        /// </summary>
        public string fileName;
        /// <summary>
        /// Э����
        /// </summary>
        public string messageName;
        /// <summary>
        /// Э��ע��
        /// </summary>
        public string messageNote;
        /// <summary>
        /// Э���ڲ��������б�
        /// </summary>
        public List<string> messageType = new List<string>();
        /// <summary>
        /// Э���ڲ������б�
        /// </summary>
        public List<string> messageTypeName = new List<string>();

        public Dictionary<string, string> messageTypeNoteDic = new Dictionary<string, string>();

    }
}
