using System;
using System.Collections;
using System.Collections.Generic;
using GameEnum;
using UnityEngine;

//枪_武器数据..同步数据也写在这
public class Weapon_Gun : MonoBehaviour
{
     public string Name;
    //枪_型号;
     public GunNameLink GunLinkType = GunNameLink.M4;
      // Audio clips for shoot and reload.
     public AudioClip shotSound;
     public AudioClip reloadSound;               
	 public AudioClip pickSound;
     public AudioClip dropSound;
     public AudioClip noBulletSound; 
     //Hud icon
     public Sprite sprite;

    // Position offsets relative to the player's right hand.
    // 右手偏移;
    public Vector3 rightHandPosition;                         
    // Rotation Offsets relative to the player's right hand.
    // 旋转;
	public Vector3 relativeRotation;
     //子弹伤害
    public float bulletDamage = 10f; 
    //后坐力
    public float recoilAngle; 
    //枪类型;
    public Weapon_GunType weapon_GunType = Weapon_GunType.Assault;
                                  

    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDestroy() {
       
    }
}
