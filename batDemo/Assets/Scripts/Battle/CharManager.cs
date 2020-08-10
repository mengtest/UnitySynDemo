using UnityEngine;
using System.Collections.Generic;

public class CharManager  : MonoSingleton<CharManager>
{
    public string str="";
    private  List<Character> _charOnList;
    private MultipleOnListPool<Character> _characterPool;
    public MultipleOnListPool<Character> CharPool {
         get{
             return this._characterPool;
         }
    }
    public void Init()
    {
       this._characterPool =new MultipleOnListPool<Character>("CharacterPool");
       this._charOnList= this._characterPool.getOnList();
       
    }
    private void Update() {
     
    }
    public void RecycleAll(){
        this._characterPool.recycleAll();
    }
    public void ClearAll(){
        this._characterPool.clearAll();
    }
   
}
