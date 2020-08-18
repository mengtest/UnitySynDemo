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
       public string actionName;
       public int length;
       public bool loop;
       //命令数据。
       public List<AFC_Base_Data> cmdDataList = new List<AFC_Base_Data>();
       private GameAssetRequest _Reqs;
       private Action onloadCall;
       private string url;

        public ActionData(string actionName){
            this.url = "Action/" + actionName;
            this.actionName = actionName;
            this.cmdDataList = new List<AFC_Base_Data>();
     //       this.init();
        }
        public void init(Action onload=null){
            onloadCall=onload;
            _Reqs = GameAssetManager.Instance.LoadAsset<TextAsset>(this.actionName,onDataloaded);
        }
        private void onDataloaded(UnityEngine.Object[] objs){
            if (objs.Length>0){
                   TextAsset assetItem = objs[0] as TextAsset;
                   XmlDocument doc = new XmlDocument();
                   doc.LoadXml(assetItem.text);
                   XmlNode projectNode = doc.SelectSingleNode("Project");

            }else{
                 DebugLog.LogError("ActionData load err :",this.actionName);
            }
            if(onloadCall!=null){
                onloadCall();
            }
            _Reqs.Unload();
            _Reqs=null;
        }

     


        // 清理资源
        public void Release()
        {
            
        }

    }

