using System;
using System.Collections.Generic;
using System.Collections;


    public class AFC_Manager 
    {
        public static Dictionary<string, Type> _typeMap;

        public static void init()
        {
            _typeMap = CreatMap();
        }
        private static Dictionary<string, Type> CreatMap()
        {
            Hashtable map = new Hashtable();
            _typeMap = new Dictionary<string, Type>();

            registerClass("ActionData", typeof(AFC_ActionData_Cmd));

            return _typeMap;
        }

        private static void registerClass(string typeName, Type afc)
        {
            _typeMap[typeName] = afc;
        }

        public static AFC_Base_Cmd getAFC(string typeName)
        {
            return (AFC_Base_Cmd)Activator.CreateInstance(_typeMap[typeName]);
        }

    }

