/*
 * @Descripttion: 
 * @version: 1.0.0
 * @Author: xsddxr909
 * @Date: 2020-02-24 16:31:04
 * @LastEditors: xsddxr909
 * @LastEditTime: 2020-07-17 20:06:07
 */
using System.Collections;
using UnityEngine;

public class GameExample : SingletonT<GameExample> {
    
    public GameExample()
    {

    }

    public IEnumerator testPool(){
       Character char1 =  CharManager.Instance.CharPool.get<Character>("char_A");
        Character  char2 =  CharManager.Instance.CharPool.get<Character>("char_A");
       Monster monster1 =   CharManager.Instance.CharPool.get<Monster>("char_B");
       yield return new WaitForSeconds(5);
       char1.recycleSelf();

        yield return new WaitForSeconds(5);
        monster1.recycleSelf();
        char2.recycleSelf();

          yield return new WaitForSeconds(5);
          char1 =  CharManager.Instance.CharPool.get<Character>("char_A");
          char2=CharManager.Instance.CharPool.get<Character>("char_A");
          monster1 =   CharManager.Instance.CharPool.get<Monster>("char_B");

           yield return new WaitForSeconds(5);
            char1.recycleSelf();
             monster1.recycleSelf();
              char2.recycleSelf();
    }


    private  GameAssetRequest reqs;
    //加载对象.
    public  void LoadResObj(){
        //"Monster/mst_xg01/mst_xg01_01"
        //"Character/skullking/skullking_01"
        //"Building/mst_building_pt02_1/mst_building_pt02_1_01"
        //"Effect/Monster/mst_xg08/mst_xg08@Skill_01_2"
	
    	 reqs = GameAssetManager.Instance.LoadAsset<GameObject>("Monster/mst_xg01/mst_xg01_01",onloaded);
    }

    private  void onloaded(UnityEngine.Object[] objs)
    {
        GameObject obj1=null;
        if (objs.Length>0){

           obj1 = GameObject.Instantiate(objs[0]) as GameObject;
            if (GameSettings.Instance.useAssetBundle)
            {
                RenderHelper.RefreshShader(ref obj1);
            }
        }
         //不使用时清除.
         if(reqs!=null){
             reqs.Unload();
             reqs = null;
         }
    }
    IEnumerator startLogicCoroutine(){
        AvatarChar avatar= GameExample.Instance.CreatAvatar();	  
        AvatarChar avatar2= GameExample.Instance.CreatAvatar();	  
        avatar.gameObject.transform.Translate(new Vector3(-50,0,0));
        avatar2.gameObject.transform.Translate(new Vector3(50,0,0));
        yield return 0;
        avatar.ChangePart("Infility_limb_01");
        avatar2.ChangePart("Infility_limb_02");
        yield return new WaitForSeconds(0.5f);
        avatar.ChangePart("Infility_body_02");
        avatar2.ChangePart("Infility_body_01");
        yield return new WaitForSeconds(1);
        avatar.ChangePart("Infility_head_02");
        avatar2.ChangePart("Infility_head_02");
        yield return new WaitForSeconds(2);
        avatar.ChangeWeapon("Infility_weapon_01");
        avatar2.ChangeWeapon("Infility_weapon_03");
        yield return new WaitForSeconds(10);
        avatar.ChangeWeapon("Infility_weapon_02_01");
        avatar.ChangeWeapon("Infility_weapon_02_02",true);
        yield return new WaitForSeconds(10);
        avatar.ChangeWeapon("Infility_weapon_03");
        avatar.ChangeWeapon("",true);
        avatar2.ChangeWeapon("Infility_weapon_01");
        avatar2.ChangeWeapon("",true);
    }
    public AvatarChar CreatAvatar(){       
         GameObject obj1=new GameObject();
         obj1.transform.parent=null;
         AvatarChar avatar= obj1.AddComponent<AvatarChar>();
         avatar.Init("Infility",new string[]{"Infility_head_01","Infility_body_01","Infility_limb_02"});
         return avatar;
    }

      //ugui
      //text  脚本  Shadow outline
}
