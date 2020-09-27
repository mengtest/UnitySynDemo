using UnityEngine;

// This class corresponds to the 3rd person camera features.
[AutoRegistLua]
public class ThirdPersonCameraCtrl : MonoBehaviour 
{
	public Transform target;                                           // Player's reference.
    public float targetFocusHeight=1.8f;
	public Vector3 pivotOffset = new Vector3(0.0f, 1.0f,  0.0f);       // Offset to repoint the camera.
	public Vector3 camOffset   = new Vector3(0.24f, 0.8f, -1.8f);       // Offset to relocate the camera related to the player position.
	public float smooth = 10f;                                         // Speed of camera responsiveness.

    public float smoothVerAngle=10f;
	//水平 6f
    public float horizontalAimingSpeed = 6f;                           // Horizontal turn speed.
	//垂直 6f
    public float verticalAimingSpeed = 6f;                             // Vertical turn speed.
	public float maxVerticalAngle = 60f;                               // Camera max clamp angle. 
	public float minVerticalAngle = -60f;                              // Camera min clamp angle.
	public string XAxis = "Analog X";                                  // The default horizontal axis input name.
	public string YAxis = "Analog Y";                                  // The default vertical axis input name.
     //横向
	private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
	private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.
	private Transform cam;                                             // This transform.
	private Vector3 relCameraPos;                                      // Current camera position relative to the player.
	private float relCameraPosMag;                                     // Current camera distance to the player.
	public Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation.
	public Vector3 smoothCamOffset;                                   // Camera current offset on interpolation.
    //目标自己的偏移
	public Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate.
    //摄像机 看目标的偏移.
	public Vector3 targetCamOffset;                                   // Camera offset target to interpolate.
	private float defaultFOV;                                          // Default camera Field of View.
	private float targetFOV;                                           // Target camera Field of View.
	private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle.
	private float deltaH = 0;                                          // Delta to horizontaly rotate camera when locking its orientation.      
	private Vector3 firstDirection;                                    // The direction to lock camera for the first time.
	private Vector3 directionToLock;                                   // The current direction to lock the camera.
	private float recoilAngle = 0f;                                    // The angle to vertically bounce the camera in a recoil movement.
	private Vector3 forwardHorizontalRef;                              // The forward reference on horizontal plane when clamping camera rotation.
	private float leftRelHorizontalAngle, rightRelHorizontalAngle;     // The left and right angles to limit rotation relative to the forward reference.
   
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
    private Camera _camera;

	void Awake()
	{
		// Reference to the camera transform.
		cam = transform;
        _camera=cam.GetComponent<Camera>();
         if(this.target!=null){
             this.init();
         }
	}
    public void init(Transform targetP=null){
         if(targetP != null){
             this.target= targetP;
         }

		// Set camera default position.
		cam.position = target.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
		cam.rotation = Quaternion.identity;

		// Get camera position relative to the player, used for collision test.
		relCameraPos = transform.position - target.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;

		// Set up references and default values.
		smoothPivotOffset = pivotOffset;
		smoothCamOffset = camOffset;
		defaultFOV = _camera.fieldOfView;
		angleH = this.target.eulerAngles.y;
		ResetTargetOffsets ();
		ResetFOV ();
		ResetMaxVerticalAngle();
    }
    private void OnDestroy() {
        target=null;
        _camera=null;
        cam = null;
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
        angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;
        angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed;
    }


	void Update()
	{
        if(target == null) return;
        
		// Get mouse movement to orbit the camera.
		// Mouse:
        if(isMouseMove){
           this.onMouseMove();
        }

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
		angleV = Mathf.LerpAngle(angleV, angleV + recoilAngle, smoothVerAngle*Time.deltaTime);

		// Handle camera orientation lock.
		if (firstDirection != Vector3.zero)
		{
			angleH -= deltaH;
			UpdateLockAngle();
			angleH += deltaH;
		}

		// Handle camera horizontal rotation limits if set.
		// if(forwardHorizontalRef != default(Vector3))
		// {
		// 	ClampHorizontal();
		// }
        // if(angleH>1000){
        //     angleH=angleH%360f;
        // }

		// Set camera orientation.
		Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
		Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		cam.rotation = aimRotation;

		// Set FOV.
   //     DebugLog.Log("targetFOV",targetFOV);
    //      DebugLog.Log("fieldOfView",cam.GetComponent<Camera>().fieldOfView);
		_camera.fieldOfView = Mathf.Lerp (_camera.fieldOfView, targetFOV, 5f* Time.deltaTime);

		// Test for collision with the environment based on current camera position.
		Vector3 baseTempPosition = target.position + camYRotation * targetPivotOffset;
		Vector3 noCollisionOffset = targetCamOffset;
		float playerFocusHeight = targetFocusHeight * 0.75f;
		//最多打四次射线;
		float upValue=  (float)(Mathf.Abs(targetCamOffset.z/4f)*100/100);
	//	DebugLog.Log("upValue",upValue);
		for(float zOffset = targetCamOffset.z; zOffset <= upValue; zOffset += upValue)
		{
			noCollisionOffset.z = zOffset;
	//		DebugLog.Log("check noCollisionOffset.z",noCollisionOffset.z);
			if(zOffset>=0){
				noCollisionOffset.z=0;
	//			DebugLog.Log("check Over Zero");
				break;
			}
			else if (DoubleViewingPosCheck (baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset),playerFocusHeight)) 
			{
	//			DebugLog.Log("check Over");
				break;
			} 
		}
		if(noCollisionOffset.z>=0){
			//如果 还是有碰撞 就需要往人物身边 X轴靠拢.
			  	for(float xOffset = targetCamOffset.x; xOffset > 0; xOffset -= 0.1f)
	         	{
                    noCollisionOffset.x = xOffset;
					if(xOffset<=0){
						noCollisionOffset.x=0;
						break;
					}else if(DoubleViewingPosCheck (baseTempPosition + aimRotation * noCollisionOffset,0, playerFocusHeight)|| xOffset == 0) 
					{
			//			DebugLog.Log("checkX Over");
						break;
					} 
				}
		}

		// Repostition the camera.
		smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
		smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);
     
     //   this.currentFollowTargetSmooth=Mathf.Lerp (this.currentFollowTargetSmooth,this.followTargetSmooth,7f* Time.deltaTime);

	 //   cam.position =   Vector3.Lerp(cam.position,target.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset,this.currentFollowTargetSmooth*Time.deltaTime);
          cam.position =  target.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset ;

		// Amortize Camera vertical bounce.
		if (recoilAngle > 0)
			recoilAngle -= 5 * Time.deltaTime;
		else if(recoilAngle < 0)
			recoilAngle += 5 * Time.deltaTime;
	}

	// Set/Unset horizontal rotation limit angles relative to custom direction.
	public void ToggleClampHorizontal(float LeftAngle = 0, float RightAngle = 0, Vector3 fwd = default(Vector3))
	{
		forwardHorizontalRef = fwd;
		leftRelHorizontalAngle = LeftAngle;
		rightRelHorizontalAngle = RightAngle;
	}

	// Limit camera horizontal rotation.
	private void ClampHorizontal()
	{
		// Get angle between reference and current forward direction.
		Vector3 cam2dFwd = this.transform.forward;
		cam2dFwd.y = 0;
		float angleBetween = Vector3.Angle(cam2dFwd, forwardHorizontalRef);
		float sign = Mathf.Sign(Vector3.Cross(cam2dFwd, forwardHorizontalRef).y);
		angleBetween = angleBetween * sign;

		// Get current input movement to compensate after limit angle is reached.
		float acc = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;
		acc += Mathf.Clamp(Input.GetAxis("Analog X"), -1, 1) * 60 * horizontalAimingSpeed * Time.deltaTime;

		// Limit left angle.
		if (sign < 0 && angleBetween < leftRelHorizontalAngle)
		{
			if (acc > 0)
				angleH -= acc;
		}
		// Limit right angle.
		else if (angleBetween > rightRelHorizontalAngle)
		{
			if (acc < 0)
				angleH -= acc;
		}
	}

	// Bounce the camera vertically.
	public void BounceVertical(float degrees)
	{
		recoilAngle = degrees;
	}

	// Handle current camera facing when locking on a specific dynamic orientation.
	private void UpdateLockAngle()
	{
		directionToLock.y = 0f;
		float centerLockAngle = Vector3.Angle(firstDirection, directionToLock);
		Vector3 cross = Vector3.Cross(firstDirection, directionToLock);
		if (cross.y < 0) centerLockAngle = -centerLockAngle;
		deltaH = centerLockAngle;
	}

	// Lock camera orientation to follow a specific direction. Usually used in short movements.
	// Example uses: (player turning cover corner, skirting convex wall, vehicle turning)
	public void LockOnDirection(Vector3 direction)
	{
		if (firstDirection == Vector3.zero)
		{
			firstDirection = direction;
			firstDirection.y = 0f;
		}
		directionToLock = Vector3.Lerp(directionToLock, direction, 0.15f * smooth * Time.deltaTime);
	}

	// Unlock camera orientation to free mode.
	public void UnlockOnDirection()
	{
		deltaH = 0;
		firstDirection = directionToLock = Vector3.zero;
	}

	// Set camera offsets to custom values.
	public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCamOffset = newCamOffset;
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
	bool DoubleViewingPosCheck(Vector3 checkPos, float offset,float playerFocusHeight)
	{
       //  target.GetComponent<CharacterController> ().height * 0.75f;
		return ViewingPosCheck (checkPos, playerFocusHeight) && ReverseViewingPosCheck (checkPos, playerFocusHeight, offset);
	}

	// Check for collision from camera to player.
	bool ViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight)
	{
		// Cast target.
		Vector3 targetPos = this.target.position + (Vector3.up * deltaPlayerHeight);
		// If a raycast from the check position to the player hits something...
		if (Physics.SphereCast(checkPos, 0.2f, targetPos - checkPos, out RaycastHit hit, relCameraPosMag,LayerHelper.GetGroundLayerMask()))
		{
			// ... if it is not the player...
			if (hit.transform != this.target && !hit.transform.GetComponent<Collider>().isTrigger)
			{
         //       DebugLog.Log(" hit ",hit.transform.name);
				// This position isn't appropriate.
				return false;
			}
		}
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		return true;
	}

	// Check for collision from player to camera.
	bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
	{
		// Cast origin.
		Vector3 origin = target.position + (Vector3.up * deltaPlayerHeight);
		if (Physics.SphereCast(origin, 0.2f, checkPos - origin, out RaycastHit hit, maxDistance,LayerHelper.GetGroundLayerMask()))
		{
			if (hit.transform != target && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
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
