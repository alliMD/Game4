using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float visionRange;
    public float timeTooLong;

    Rigidbody rb;
    public float speedFast = 500f;
    public float speedMedium = 350f;
    public float speedSlow = 400f;
    Vector3 desiredFinalPos;
    Vector3 otherFinalPos;
    Vector3 forwardVel;

    Vector3 desiredVelocity;
    Vector3 steering;

    float forceSeek_x;
    float forceSeek_z;
    float forceFlee_x;
    float forceFlee_z;
    Vector3 direction;

    float timer = 0f;
    float timer2 = 0f;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        //choose door
        GameObject[] doors = GameObject.FindGameObjectsWithTag("ExitDoor");
        System.Random rnd = new System.Random();
        int chooseDoor = rnd.Next(0, 2);
        desiredFinalPos = doors[chooseDoor].transform.position;

        if(chooseDoor == 0) otherFinalPos = doors[1].transform.position;
        else otherFinalPos = doors[0].transform.position;

        direction = Vector3.Normalize(desiredFinalPos - transform.position);
       
        Debug.Log("start: forceSeek x = " + forceSeek_x + "   forceSeek z = " + forceSeek_z);


        forceFlee_x = 0;
        forceFlee_z = 0;

	}


    void FixedUpdate () {

        if (rb.velocity.magnitude < 0.3)
        {
            timer += Time.deltaTime;
            if (timer > timeTooLong)
            {
                Debug.Log("Too long");
                Vector3 temp = desiredFinalPos;
                desiredFinalPos = otherFinalPos;
                otherFinalPos = temp;
                timer2 = 0.3f;
            }
        }
        if(timer2 > 0){
            timer = 0f;
            timer2 -= Time.deltaTime;
            transform.position = new Vector3(transform.position.x + 7 * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else{
            timer2 = 0f;
            direction = Vector3.Normalize(desiredFinalPos - transform.position);
            forceSeek_x = speedMedium * direction.x;
            forceSeek_z = speedMedium * direction.z;
        }

        if (transform.position.x > -14){

            CheckForObstacle();

            if(forceFlee_z > 0 || forceFlee_z < 0)
            {
                forceSeek_x = 0;
                forceSeek_z = 0;
                rb.AddForce(forceFlee_x * Time.deltaTime, 0, forceFlee_z * Time.deltaTime);

            }else{
                direction = Vector3.Normalize(desiredFinalPos - transform.position);

                forceSeek_x = speedMedium * direction.x;
                forceSeek_z = speedMedium * direction.z;

                rb.AddForce(forceSeek_x * Time.deltaTime, 0, forceSeek_z * Time.deltaTime);
            }
        }else{
            Respawn();
        }

    }

    void CheckForObstacle(){
        RaycastHit hit;
        List<Vector3> rays = new List<Vector3>();

        Vector3 ray = visionRange * direction;
        Vector3 ray2 = new Vector3(ray.x + 0.4f, ray.y, ray.z + 0.4f);
        Vector3 ray3 = new Vector3(ray.x + 0.4f, ray.y, ray.z - 0.4f);

        rays.Add(ray);
        rays.Add(ray2);
        rays.Add(ray3);

        //foreach(Vector3 r in rays){
            if (Physics.Raycast(transform.position, ray, out hit, ray.magnitude))
            {
                Debug.DrawRay(transform.position, ray.normalized * hit.distance, Color.yellow);

                if (hit.collider.gameObject.name == "Cube")
                {
                    forceFlee_x = speedSlow * (-ray.normalized.x);
                    forceFlee_z = speedFast * (-ray.normalized.z);
                    forceSeek_x = 0f;
                    forceSeek_z = 0f;
                }

            }
            else
            {
                Debug.DrawRay(transform.position, ray, Color.magenta);
                forceFlee_x = 0;
                forceFlee_z = 0;
                forceSeek_x = speedMedium * direction.x;
                forceSeek_z = speedMedium * direction.z;

            }
        //}

      
    }

    void Respawn(){
        transform.position = new Vector3(12, transform.position.y, 0);
        Start();
    }
}
