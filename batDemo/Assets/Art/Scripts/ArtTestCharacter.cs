using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtTestCharacter : MonoBehaviour
{
    [Tooltip("移动速度")]
    public float moveSpeed = 5;
    private Animator animator;
    private CharacterController controller;
    private Camera cam;
    private Transform camTrans;

    private const float STAND_CONTROLLER_HEIGHT = 1.8F;
    private const float SQUAT_CONTROLLER_HEIGHT = 1.6F;
    private const float PRONE_CONTROLLER_HEIGHT = 1F;
    private const float CONTROLLER_STEP_OFFSET = 0.5F;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = gameObject.AddComponent<CharacterController>();
        cam = Camera.current;

        if (cam == null)
        {
            var camGo = new GameObject("Camera");
            cam = camGo.AddComponent<Camera>();
        }

        var camCtrl = cam.gameObject.AddComponent<ArtTestCamera>();
        camCtrl.SetFollowTarget(transform);

        camTrans = cam.transform;

        controller.center = new Vector3(0, STAND_CONTROLLER_HEIGHT / 2, 0);
        controller.height = STAND_CONTROLLER_HEIGHT;
        controller.slopeLimit = 45f;
        controller.stepOffset = CONTROLLER_STEP_OFFSET;
    }

    private Vector3 moveMotion = new Vector3();
    private float gravity = -19.6f;
    private float velocityY = 0;
    private Vector3 lastPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isJump = Input.GetButton("Jump");

        moveMotion = (v * transform.forward + h * transform.right).normalized;
        moveMotion *= Time.deltaTime * moveSpeed;

        if (isJump)
        {
            velocityY = 7;
        }

        if (velocityY > 0 || controller.isGrounded == false)
        {
            velocityY += Time.deltaTime * gravity;
            moveMotion.y = velocityY * Time.deltaTime;
        }
        else
        {
            velocityY = 0;
        }

        controller.Move(moveMotion);

        animator.SetBool("IsGround", velocityY < 0 || controller.isGrounded);
        animator.SetBool("IsJump", isJump);
        animator.SetFloat("Speed", (transform.position - lastPos).magnitude / Time.deltaTime);
        animator.SetFloat("moveX", h);
        animator.SetFloat("moveZ", v);

        lastPos = transform.position;
    }
}
