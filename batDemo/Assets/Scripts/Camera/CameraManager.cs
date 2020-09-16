
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[AutoRegistLua]
public class CameraManager  : MonoSingleton<CameraManager>
{
    public GameObject mainCamera;
    public ThirdPersonCameraCtrl cameraCtrl;
    public PostProcessLayer postLayer;
    public void Init()
    {
       
       mainCamera =  GameObject.FindGameObjectWithTag("MainCamera");
       cameraCtrl = mainCamera.GetComponent<ThirdPersonCameraCtrl>();
       if(cameraCtrl==null){
           cameraCtrl =  mainCamera.AddComponent<ThirdPersonCameraCtrl>();
        //   cameraCtrl.maxVerticalAngle
       }
       postLayer = mainCamera.GetComponent<PostProcessLayer>();
       postLayer.enabled=true;

    }
    private void Update() {
     
    }

    
   
}
