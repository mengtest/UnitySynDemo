using System;
using System.Collections;
using System.Collections.Generic;
using GameEnum;
using UnityEngine;

//武器系统  我想弱化远程  武器栏只
public class BonePos : MonoBehaviour
{
    //记录基础站立姿态 的 所有数据....Stand
    // 盘骨 , 脊柱 , 胸部 , 右手 , 左臂
    public Transform hips, spine, chest, rightHand, leftArm;   
	public Vector3 initialRootRotation;                                  // Initial root bone local rotation.
	public Vector3 initialHipsRotation;                                  // Initial hips rotation related to the root bone.
	public Vector3 initialSpineRotation;                                 // Initial spine rotation related to the root bone.
    public Vector3 initialChestRotation;                          // Initial chest rotation related to the spine bone.
	public float distToHand;                                      // Distance from neck to hand.
	public Vector3 castRelativeOrigin;                            // Position of neck to cast for blocked aim test.

    private void Awake() {
       
    }
    public void initAni(){
            Animator ani = gameObject.GetComponent<Animator>();
            Transform neck = ani.GetBoneTransform(HumanBodyBones.Neck);
            if (!neck)
            {
                neck = ani.GetBoneTransform(HumanBodyBones.Head).parent;
            }
            hips =ani.GetBoneTransform(HumanBodyBones.Hips);
            spine = ani.GetBoneTransform(HumanBodyBones.Spine);
            chest = ani.GetBoneTransform(HumanBodyBones.Chest);
            rightHand = ani.GetBoneTransform(HumanBodyBones.RightHand);
            leftArm = ani.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            initialRootRotation = (hips.parent == gameObject.transform) ? Vector3.zero : hips.parent.localEulerAngles;
            initialHipsRotation = hips.localEulerAngles;
            initialSpineRotation = spine.localEulerAngles;
            initialChestRotation = chest.localEulerAngles;
            castRelativeOrigin = neck.position - gameObject.transform.position;
            distToHand = (rightHand.position - neck.position).magnitude * 1.5f;
       //     blockedAimBool = Animator.StringToHash("BlockedAim");
    }
    // Check if aim is blocked by obstacles. 检查瞄准是否被障碍物阻挡。

    // Update is called once per frame
    private void OnDestroy()
    {
        if (hips != null)
        {
            hips = null;
        }
        if (spine != null)
        {
            spine = null;
        }
        if (chest != null)
        {
            chest = null;
        }
         if (rightHand != null)
        {
            rightHand = null;
        }
         if (leftArm != null)
        {
            leftArm = null;
        }
    }
}