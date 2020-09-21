using System;
using UnityEngine;

public class AnimatorIK : MonoBehaviour
{
    private Character _char=null;
    private Action<int> _onIKFunction; 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void init(ObjBase obj, Action<int> onIKFunction){
          _char=obj  as Character;
          _onIKFunction=onIKFunction;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if(_onIKFunction!=null){
             DebugLog.Log("OnAnimatorIK");   
            _onIKFunction(layerIndex);
        }
    }
    private void OnDestroy() {
        _char=null;
        _onIKFunction=null;
    }
}
