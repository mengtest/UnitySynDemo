
using UnityEngine;

[AutoRegistLua]
public class CameraManager  : MonoSingleton<CameraManager>
{
    public GameObject mainCamera;
    public ThirdPersonCameraCtrl cameraCtrl;

    public void Init()
    {
       
       mainCamera =  GameObject.FindGameObjectWithTag("MainCamera");
       cameraCtrl = mainCamera.GetComponent<ThirdPersonCameraCtrl>();
       if(cameraCtrl=null){
           cameraCtrl =  mainCamera.AddComponent<ThirdPersonCameraCtrl>();
       }

    }
    private void Update() {
     
    }

    
   
}
