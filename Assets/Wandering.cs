using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour {

    public int left = -11;
    public int bottom = -8;
    public int right = 11;
    public int top = 8;
    public float speed;
    public float fleeSpeed;
    public float visionRange;
    public float waitingTime;

    Vector3 undesiredPos;
    Vector3 desiredPos;
    Vector3 desiredVelocity;
    Vector3 flee;
    Vector3 doorPos;

    bool redAgentNearby = false;

    bool escapingOnce = false;


    enum State{
        CHOOSE_DEST,
        SEEK,
        FLEE,
        DEFEND
    }

    State state;

    float timer;
    float timer2;

    Rigidbody rb;

	void Start () {
        state = State.CHOOSE_DEST;

        timer = 0f;
        timer2 = 0f;

        rb = gameObject.GetComponent<Rigidbody>();
	}

	void Update () {
        System.Random rnd = new System.Random();
        timer2 += Time.deltaTime;

        CheckObstacles();
        switch (state){
            case State.CHOOSE_DEST:
                float newX = rnd.Next(left, right + 1);
                float newZ = rnd.Next(bottom, top + 1);
                desiredPos = new Vector3(newX, 0, newZ);
                Debug.Log("desired pos = " + desiredPos);
                state = State.SEEK;
                break;
            case State.SEEK:
                float x = transform.position.x;
                float z = transform.position.z;
                desiredVelocity = Vector3.Normalize(new Vector3(desiredPos.x - x, 0, desiredPos.z - z)) * speed;
                rb.AddForce(desiredVelocity * Time.deltaTime);

                if(Mathf.Abs(x - desiredPos.x) < 1.7 && Mathf.Abs(z - desiredPos.z) < 1.7){
                    state = State.CHOOSE_DEST;
                }
                break;
            case State.FLEE:
                timer += Time.deltaTime;
                if (timer > 0.7)
                {
                    state = State.SEEK;
                    escapingOnce = false;
                    timer = 0f;
                }
                flee = Vector3.Normalize(new Vector3(0.05f, 0, transform.position.z - undesiredPos.z)) * fleeSpeed;
                rb.AddForce((rb.velocity + flee) * Time.deltaTime);
                break;
            case State.DEFEND:
                timer += Time.deltaTime;
                if (timer > 2)
                {
                    state = State.CHOOSE_DEST;
                    timer = 0f;
                }
                x = transform.position.x;
                z = transform.position.z;

                if (Mathf.Abs(x - (doorPos.x + 1)) < 0.6 && Mathf.Abs(z - doorPos.z) < 0.3)
                {
                    rb.velocity = Vector3.zero;
                }else{
                    desiredVelocity = desiredVelocity = Vector3.Normalize(new Vector3((doorPos.x+1) - x, 0, doorPos.z - z)) * speed;
                    rb.AddForce(desiredVelocity * Time.deltaTime);
                }

                break;
        }
        if(timer2 > waitingTime){
            timer2 = 0f;
            state = State.CHOOSE_DEST;
        }

        //Check if close to any of the two left doors
        GameObject[] doors = GameObject.FindGameObjectsWithTag("ExitDoor");
        foreach(GameObject d in doors){
            if(Vector3.Distance(d.transform.position, transform.position) < 5){
                if(redAgentNearby){
                    doorPos = d.transform.position;
                    state = State.DEFEND;
                }
            }
        }
    }


    void CheckObstacles()
    {
        GameObject[] redAgents = GameObject.FindGameObjectsWithTag("Agent");
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] greenAgents = GameObject.FindGameObjectsWithTag("GreenAgent");
        GameObject[] yellowAgents = GameObject.FindGameObjectsWithTag("YellowAgent");

        foreach (GameObject a in redAgents)
        {
            if (Vector3.Distance(a.transform.position, transform.position) < visionRange)
            {
                undesiredPos = a.transform.position;
                state = State.FLEE;
            }

            if(Vector3.Distance(a.transform.position, transform.position) < 3){
                redAgentNearby = true;
            }else{
                redAgentNearby = false;
            }
        }
        foreach (GameObject g in greenAgents)
        {
            if (g != gameObject && Vector3.Distance(g.transform.position, transform.position) < visionRange )
            {
                undesiredPos = g.transform.position;
                state = State.FLEE;
            }
        }
        foreach (GameObject y in yellowAgents)
        {
            if (Vector3.Distance(y.transform.position, transform.position) < visionRange + 0.4)
            {
                undesiredPos = y.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject c in cubes)
        {
            if (!escapingOnce && Vector3.Distance(c.transform.position, transform.position) < visionRange + 0.9)
            {
                undesiredPos = c.transform.position;
                escapingOnce = true;
                state = State.FLEE;
            }
        }
        Vector3 ray = visionRange * rb.velocity.normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, ray, out hit, ray.magnitude))
        {
            Debug.DrawRay(transform.position, ray.normalized * hit.distance, Color.yellow);
           
        }
        else
        {
            Debug.DrawRay(transform.position, ray, Color.magenta);
        }
    }
}
