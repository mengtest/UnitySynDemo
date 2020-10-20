
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[AutoRegistLua]
public class CameraManager  : MonoSingleton<CameraManager>
{
    public GameObject mainCamera;
    public Camera cam;
    public PostProcessLayer postLayer;

    private Player target;
    public void Init()
    {
       
       mainCamera =  GameObject.FindGameObjectWithTag("MainCamera");
       cam=mainCamera.GetComponent<Camera>();
    //   cameraCtrl = mainCamera.GetComponent<CameraCtrl>();
    //    if(cameraCtrl==null){
    //        cameraCtrl =  mainCamera.AddComponent<CameraCtrl>();
    //     //   cameraCtrl.maxVerticalAngle
    //    }
       postLayer = mainCamera.GetComponent<PostProcessLayer>();
       postLayer.enabled=true;
    }
    public void FocusPlayer(Player player){
        if(target!=null){
            target.CameraFocus(null);
        }
        target=player;
        player.CameraFocus(cam);
    }
    private void Update() {
     
    }

    
   
}
