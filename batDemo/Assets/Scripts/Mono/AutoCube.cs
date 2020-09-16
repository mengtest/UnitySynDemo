using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCube : MonoBehaviour
{
    public Transform end;

    //float speed = 4f;

    bool liftActivated = false;
    bool backActivate =false;

    public Vector3 defultPos;

    void Start()
    {
      defultPos=this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(liftActivated)
         {
         //   float step = speed * Time.deltaTime;
            //To move up the lift
            transform.position = Vector3.MoveTowards(transform.position, end.position, 4 * Time.deltaTime);
            //To spin the lift
            transform.Rotate(Vector3.forward, 30 * Time.deltaTime);

            //To stop spining the lift when it reaches the end of the path
            if(transform.position.Equals(end.position))
                liftActivated = false;
         }
         if(backActivate){
         //    float step = speed * Time.deltaTime;
            //To move up the lift
            transform.position = Vector3.MoveTowards(transform.position, defultPos, 4 * Time.deltaTime);
            //To spin the lift
            transform.Rotate(Vector3.back, 30 * Time.deltaTime);

            //To stop spining the lift when it reaches the end of the path
            if(transform.position.Equals(defultPos))
                backActivate = false;
         }
    }
    
    void OnTriggerEnter(Collider other){
        liftActivated = true;
        other.gameObject.transform.parent = this.transform;
 //       other.gameObject.GetComponent<Rigidbody>().isKinematic=true;
    }
    void OnTriggerExit(Collider other) {
        liftActivated = false;
        backActivate=true;
        other.gameObject.transform.parent = null;
//        other.gameObject.GetComponent<Rigidbody>().isKinematic=false;
    }
}
