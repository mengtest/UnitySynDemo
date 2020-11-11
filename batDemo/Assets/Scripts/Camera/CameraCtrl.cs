using System;
using UnityEngine;

// This class corresponds to the 3rd person camera features.
[AutoRegistLua]
public class CameraCtrl : MonoBehaviour 
{
    //开火点.
    public GameObject FirePoint=null;
	public Transform target;                                           // Player's reference.
    public float targetFocusHeight=1.8f;
	public Vector3 pivotOffset = new Vector3(0.0f, 1.0f,  0.0f);       // Offset to repoint the camera.
	public Vector3 camOffset   = new Vector3(0.4f, 0.6f, -2.2f);// 半身 new Vector3(0.24f, 0.8f, -1.8f);   // Offset to relocate the camera related to the player position.
	public float smooth = 10f;                                         // Speed of camera responsiveness.

    public float smoothVerAngle=10f;

	private float recoilAngleSub=20f;
	//水平 6f
    public float horizontalAimingSpeed = 6f;                           // Horizontal turn speed.
	//垂直 6f
    public float verticalAimingSpeed = 6f;                             // Vertical turn speed.
	public float maxVerticalAngle = 60f;                               // Camera max clamp angle. 
	public float minVerticalAngle = -60f;                              // Camera min clamp angle.
	public string XAxis = "Analog X";                                  // The default horizontal axis input name.
	public string YAxis = "Analog Y";                                  // The default vertical axis input name.
     //横向 水平
	private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
	//垂直
    private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.
	private Vector3 relCameraPos;                                      // Current camera position relative to the player.
	private float relCameraPosMag;                                     // Current camera distance to the player.
	public Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation.
	public Vector3 smoothCamOffset;                                   // Camera current offset on interpolation.
    //目标自己的偏移
	public Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate.
    //摄像机 看目标的偏移.
	public Vector3 targetCamOffset;                                   // Camera offset target to interpolate.
	public float defaultFOV=60;                                          // Default camera Field of View.
	private float targetFOV;                                           // Target camera Field of View.
	private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle.
	private Vector2 recoilAngle = new Vector2();                                    // The angle to vertically bounce the camera in a recoil movement.
   
   //旋转屏幕触发 加速度的 速度
    public int Horizontal_Acce_Dic =60;
    //滑屏加速度
    public int Horizontal_Acce_Speed=140;

    public bool isMouseMove=false;
     public bool isJoyMove=false;


	// Get the camera horizontal angle.
	public float GetH()
     {
        return angleH;
     }
    public float GetV()
     {
        return angleV;
     }
    private Camera _camera=null;


    /// <summary>
    /// Create new fire point for this weapon.
    /// </summary>
    public void InitFirePoint()
    {
        DebugLog.Log("InitFirePoint");
        if(FirePoint==null)
        {
            FirePoint = new GameObject("FirePoint");
            FirePoint.transform.SetParent(transform);
        }
        FirePoint.transform.localPosition = Vector3.zero;
        FirePoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
        FirePoint.transform.localScale = Vector3.one;
    }
    public void resetFirePoint(){
        FirePoint.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

	void Awake()
	{
        // Reference to the camera transform.
        _camera=transform.GetComponent<Camera>();
         if(this.target!=null){
             this.init();
         }
	}
    public void setMainCamera(Camera camera){
        _camera=camera;
        if(_camera!=null){
            _camera.transform.SetParent(transform);
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public bool isCamTarget(){
        if(_camera!=null){
            return true;
        }
        return false;
    }
    public void init(Player player){
        this.init(player.gameObject.transform);
    }
    public void init(Transform targetP=null){
         if(targetP != null){
             this.target= targetP;
         }

         InitFirePoint();

		// Set camera default position.
		transform.position = target.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
		transform.rotation = Quaternion.identity;

        // Get camera position relative to the player, used for collision test.
        relCameraPos = base.transform.position - target.position;
	//	DebugLog.Log("relCameraPos",relCameraPos);
	//	DebugLog.Log("relCameraPos.magnitude",relCameraPos.magnitude);
		relCameraPosMag = relCameraPos.magnitude - 0.2f;
	//	DebugLog.Log("relCameraPosMag",relCameraPosMag);

		// Set up references and default values.
		smoothPivotOffset = pivotOffset;
		smoothCamOffset = camOffset;
        if(_camera!=null){
		    defaultFOV = _camera.fieldOfView;
        }
		angleH = this.target.eulerAngles.y;
		ResetTargetOffsets ();
		ResetFOV ();
		ResetMaxVerticalAngle();
    }
    private void OnDestroy() {
        target=null;
        _camera=null;
    }
    public void  onTouchMove(Vector2 delta){
        //水平
        float dicH;
      //  DebugLog.Log("delta.x",delta.x); 
        if(delta.x>=Horizontal_Acce_Dic){
      //      DebugLog.Log("Add>>>>> delta.x",delta.x); 
          dicH=delta.x * horizontalAimingSpeed*0.031f *(1+ Horizontal_Acce_Speed/100);
        }else{
          dicH =delta.x * horizontalAimingSpeed*0.031f;
        }
	    angleH += dicH;
     	//垂直
        angleV += delta.y * verticalAimingSpeed*0.0155f;
      //  angleH += delta.x * horizontalAimingSpeed*0.01f;
     //	angleV += delta.y * verticalAimingSpeed*0.01f;
    }
    public void  onMouseMove(){
   //     DebugLog.Log(Input.GetAxis("Mouse X"));
      //加速度
        angleH += Input.GetAxis("Mouse X") * horizontalAimingSpeed/4;

        angleV += Input.GetAxis("Mouse Y") * verticalAimingSpeed/4;
      //无加速度
     //  angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;
	//	angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed;
      //    DebugLog.Log("angleH",angleH);
    }

	void LateUpdate()
	{
        if(target == null) return;
        
		// Get mouse movement to orbit the camera.
		// Mouse:
        // if(isMouseMove){
        //    this.onMouseMove();
        // }

		// Joystick:
        // if(this.isJoyMove){
        // 	angleH += Mathf.Clamp(Input.GetAxis(XAxis), -1, 1) * 60 * horizontalAimingSpeed * Time.deltaTime;
        // 	angleV += Mathf.Clamp(Input.GetAxis(YAxis), -1, 1) * 60 * verticalAimingSpeed * Time.deltaTime;
        // }

      //  DebugLog.Log("angleH",angleH);
     //   DebugLog.Log("angleV",angleV);
		// Set vertical movement limit.
		angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

		// Set vertical camera bounce.
		angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle.y, smoothVerAngle*Time.deltaTime);
	    angleH = Mathf.LerpAngle(angleH, angleH + recoilAngle.x, smoothVerAngle*Time.deltaTime);
          
        if(angleH>360){
            angleH=angleH%360f;
        }

		// Set camera orientation.
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		transform.rotation = aimRotation;

		// Set FOV.
   //     DebugLog.Log("targetFOV",targetFOV);
    //      DebugLog.Log("fieldOfView",cam.GetComponent<Camera>().fieldOfView);
       if(_camera!=null){
		   _camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, targetFOV, 5f* Time.deltaTime);
       }

		// Test for collision with the environment based on current camera position.
		Vector3 baseTempPosition = target.position + camYRotation * targetPivotOffset;
		Vector3 noCollisionOffset = targetCamOffset;
		float playerFocusHeight = targetFocusHeight * 0.9f;
		//最多打四次射线;
		float upValue=  (float)(Mathf.Abs(targetCamOffset.z/4f)*100/100);
        if(_camera!=null){
    //		DebugLog.Log("upValue",upValue);
            for(float zOffset = targetCamOffset.z; zOffset <= upValue; zOffset += upValue)
            {
                noCollisionOffset.z = zOffset;
    //			DebugLog.Log("check noCollisionOffset.z",noCollisionOffset.z);
                if(zOffset>=0){
                    noCollisionOffset.z=-0.1f;
        //			DebugLog.Log("check Over Zero");
                    break;
                }
                else if (DoubleViewingPosCheck (baseTempPosition + aimRotation * noCollisionOffset,playerFocusHeight)) 
                {
        //			DebugLog.Log("check Over");
                    break;
                } 
            }
        }else{
            //摄像机不在的时候 就最近点成为摄像机就好
            noCollisionOffset.z=-0.2f;
        }

		// Repostition the camera.
		smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
		smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);
     
     //   this.currentFollowTargetSmooth=Mathf.Lerp (this.currentFollowTargetSmooth,this.followTargetSmooth,7f* Time.deltaTime);

	 //   cam.position =   Vector3.Lerp(cam.position,target.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset,this.currentFollowTargetSmooth*Time.deltaTime);
          transform.position =  target.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset ;

		// Amortize Camera vertical bounce.
      //  DebugLog.Log(recoilAngle);
		if (recoilAngle.x > 0){
			recoilAngle.x -= recoilAngleSub * Time.deltaTime;
            if (recoilAngle.x <=0){
                recoilAngle.x=0;
            }
        }
		else if(recoilAngle.x < 0){
			recoilAngle.x += recoilAngleSub * Time.deltaTime;
              if (recoilAngle.x >=0){
                recoilAngle.x=0;
            }
        }

        if (recoilAngle.y > 0){
			recoilAngle.y -= recoilAngleSub * Time.deltaTime;
            if (recoilAngle.y <=0){
                recoilAngle.y=0;
            }
        }
		else if(recoilAngle.y < 0){
			recoilAngle.y += recoilAngleSub * Time.deltaTime;
            if (recoilAngle.y >=0){
                recoilAngle.y=0;
            }
        }

	}


	
	// Bounce the camera vertically.
	public void SetRecoilAngle(Vector2 recoil)
	{
		recoilAngle = recoilAngle+recoil;
	//	 DebugLog.Log(recoilAngle,recoil);
	}
   public void SetTargetPivotOffset(Vector3 newPivotOffset)
	{
		targetPivotOffset = newPivotOffset;
	}

	// Set camera offsets to custom values.
	public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCamOffset = newCamOffset;
	}
// Reset camera offsets to default values.
	public void ResetTargetPivotOffset()
	{
		targetPivotOffset = pivotOffset;
	}
	// Reset camera offsets to default values.
	public void ResetTargetOffsets()
	{
		targetPivotOffset = pivotOffset;
		targetCamOffset = camOffset;
	}

	// Reset the camera vertical offset.
	public void ResetYCamOffset()
	{
		targetCamOffset.y = camOffset.y;
	}

	// Set camera vertical offset.
	public void SetYCamOffset(float y)
	{
		targetCamOffset.y = y;
	}

	// Set camera horizontal offset.
	public void SetXCamOffset(float x)
	{
		targetCamOffset.x = x;
	}
	// Set camera horizontal offset.
	public void SetZCamOffset(float z)
	{
		targetCamOffset.z = z;
	}
	// Set custom Field of View.
	public void SetFOV(float customFOV)
	{
		this.targetFOV = customFOV;
	}

	// Reset Field of View to default value.
	public void ResetFOV()
	{
		this.targetFOV = defaultFOV;
	}

	// Set max vertical camera rotation angle.
	public void SetMaxVerticalAngle(float angle)
	{
		this.targetMaxVerticalAngle = angle;
	}

	// Reset max vertical camera rotation angle to default value.
	public void ResetMaxVerticalAngle()
	{
		this.targetMaxVerticalAngle = maxVerticalAngle;
	}

	// Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
	bool DoubleViewingPosCheck(Vector3 checkPos,float playerFocusHeight)
	{
       //  target.GetComponent<CharacterController> ().height * 0.75f;
		return ViewingPosCheck (checkPos, playerFocusHeight) && ReverseViewingPosCheck (checkPos, playerFocusHeight);
	}

	// Check for collision from camera to player.
	bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
	{
		// Cast target.
		Vector3 targetPos = this.target.position + (Vector3.up * deltaPlayerHeight);
		// If a raycast from the check position to the player hits something...
	//	Debug.DrawRay(checkPos,  targetPos - checkPos, Color.cyan);
	    Vector3 dic= targetPos - checkPos;
		//Physics.Raycast(checkPos,dic,out RaycastHit hit,dic.magnitude,LayerHelper.GetGroundLayerMask())
		//(Physics.SphereCast(checkPos, 0.2f, targetPos - checkPos, out RaycastHit hit, relCameraPosMag,LayerHelper.GetGroundLayerMask())
		if (Physics.SphereCast(checkPos, 0.1f, dic, out RaycastHit hit, dic.magnitude,LayerHelper.GetGroundLayerMask()))
		{
			// ... if it is not the player...
			if (hit.transform != this.target && !hit.transform.GetComponent<Collider>().isTrigger)
			{
           //     DebugLog.Log(" hit ",hit.transform.name);
				// This position isn't appropriate.
				return false;
			}
		}
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		return true;
	}

	// Check for collision from player to camera.
	bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
	{
		// Cast origin.
		Vector3 origin = target.position + (Vector3.up * deltaPlayerHeight);
		    Vector3 dic= checkPos - origin;
	//		Debug.DrawRay(origin,  checkPos - origin, Color.red);
		//	Physics.Raycast(checkPos,dic,out RaycastHit hit,dic.magnitude,LayerHelper.GetGroundLayerMask())
			//Physics.SphereCast(origin, 0.2f, checkPos - origin, out RaycastHit hit, maxDistance,LayerHelper.GetGroundLayerMask())
		if (Physics.SphereCast(origin, 0.1f, dic, out RaycastHit hit, dic.magnitude,LayerHelper.GetGroundLayerMask()))
		{
			if (hit.transform != target && hit.transform != base.transform && !hit.transform.GetComponent<Collider>().isTrigger)
			{
           //         DebugLog.Log(" hit2 ",hit.transform.name);
				return false;
			}
		}
		return true;
	}

	// Get camera magnitude.
	public float GetCurrentPivotMagnitude(Vector3 finalPivotOffset)
	{
		return Mathf.Abs ((finalPivotOffset - smoothPivotOffset).magnitude);
	}
}
