//*************************************************************************
//	技能部件
//*************************************************************************

//技能部件
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

    public class ActionData
    {
        public int actionLayer=GameEnum.ActionLayer.BaseLayer;
        public int cancelPriority=GameEnum.CancelPriority.Stand_Move_Null;
       public string actionName;
       public float length;
       public bool loop;
       //命令数据。
       public List<AFC_Base_Data> cmdDataList = new List<AFC_Base_Data>();
       private GameAssetRequest _Reqs;
       private Action onloadCall;
       private string url;
       private bool isDead=false;

        public ActionData(string actionName){
            this.url = "Action/" + actionName;
            this.actionName = actionName;
            this.cmdDataList = new List<AFC_Base_Data>();
            isDead=false;
     //       this.init();
        }
        public void init(Action onload=null){
            onloadCall=onload;
            _Reqs = GameAssetManager.Instance.LoadAsset<TextAsset>(this.actionName,onDataloaded);
        }
        private void onDataloaded(UnityEngine.Object[] objs){
            if(isDead){
                return;
            }
            if (objs.Length>0){
                  this.cmdDataList.Clear();
                   TextAsset assetItem = objs[0] as TextAsset;
                   XmlDocument doc = new XmlDocument();
                   doc.LoadXml(assetItem.text);
                   XmlNode projectNode = doc.SelectSingleNode("Project");
                   XmlNode actionNode = projectNode.SelectSingleNode("Action");
                   this.length = float.Parse(actionNode.Attributes["length"].Value);
                   this.loop = bool.Parse(actionNode.Attributes["loop"].Value);
                   XmlNodeList list =actionNode.ChildNodes;
                   for (int i = 0; i < list.Count; i++)
                   {
                       XmlNode trackNode=list[i];
                       XmlNodeList eventlist = trackNode.ChildNodes;
                       for (int j = 0; j < eventlist.Count; j++)
                       {
                          XmlNode eventNode = eventlist[j];
                          AFC_Base_Data cmdData= new AFC_Base_Data();
                          cmdData.initXml(eventNode);
                          if(cmdData.eventName=="ActionData"){
                               this.actionLayer=cmdData.IntAbb["actionLayer"];
                               this.cancelPriority=cmdData.IntAbb["cancelPriority"];
                          }
                          this.cmdDataList.Add(cmdData);
                       }
                   }
            }else{
                 DebugLog.LogError("ActionData load err :",this.actionName);
            }
            if(onloadCall!=null){
                onloadCall();
                onloadCall=null;
            }
            if(_Reqs!=null){
               _Reqs.Unload();
            }
            _Reqs=null;
        }


        // 清理资源
        public void Release()
        {
            isDead=true;
            if(_Reqs!=null){
               _Reqs.Unload();
                _Reqs=null;
            }
            if(onloadCall!=null){
                onloadCall=null;
            }
            if(cmdDataList!=null){
                cmdDataList.Clear();
                cmdDataList=null;
            }
        }

    }

