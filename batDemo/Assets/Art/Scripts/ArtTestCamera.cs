using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestCamera : MonoBehaviour
{
    public float verticalSpeed = 10;
    public float horizSpeed = 30;
    public float followDist = 3;
    public float followHeight = 4;
    public Vector3 followOffset = new Vector3(-0.18f, 1.74f,0);
    private Transform selfTrans;
    private Transform followTarget;

    public void SetFollowTarget(Transform followTrans)
    {
        followTarget = followTrans;
    }

    // Start is called before the first frame update
    void Start()
    {
        selfTrans = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (followTarget == null)
            return;

        var wantedPos = followTarget.position + followTarget.forward * -followDist + Vector3.up * followHeight;

        selfTrans.position = wantedPos;//Vector3.Lerp(selfTrans.position, wantedPos,Time.deltaTime * 5);

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            followTarget.Rotate(Vector3.up, mouseX * Time.deltaTime * horizSpeed);

            followHeight -= mouseY * Time.deltaTime * verticalSpeed;
        }

        selfTrans.LookAt(followTarget.position + followOffset, Vector3.up);
    }
}
