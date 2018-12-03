using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAgentMovement : MonoBehaviour {

    public float timeTooLong;
    public float speed;
    public float fleeSpeed;
    public float fastSpeed;
    public float visionRange;

    Rigidbody rb;

    Vector3 desiredFinalPos;
    Vector3 alternativeFinalPos;

    Vector3 undesiredPos;

    Vector3 desiredVelocity;
    Vector3 flee;

    float timer;
    float timer2;

    bool escapedOnce = false;

    enum State
    {
        FLEE,
        SEEK
    }

    State state;


	void Start () {
        timer2 = 0f;
        state = State.SEEK;
        rb = gameObject.GetComponent<Rigidbody>();

        GameObject[] doors = GameObject.FindGameObjectsWithTag("ExitDoor");
        System.Random rnd = new System.Random();
        int chooseDoor = rnd.Next(0, 2);
        desiredFinalPos = doors[chooseDoor].transform.position;
        if (chooseDoor == 0) alternativeFinalPos = doors[1].transform.position;
        else alternativeFinalPos = doors[0].transform.position;

    }
	
	void FixedUpdate () {
        timer2 += Time.deltaTime;
        if (transform.position.x < -14){
            timer += Time.deltaTime;
            rb.velocity = Vector3.zero;
            if(timer > 2){
                timer = 0;
                Respawn();
            }
        }

        if(transform.position.x < 12.9){
            CheckObstacles();
        }

       

        switch (state){
            case State.SEEK:
                desiredVelocity = Vector3.Normalize(new Vector3(desiredFinalPos.x - transform.position.x, 0, desiredFinalPos.z - transform.position.z)) * speed;
                rb.AddForce(desiredVelocity * Time.deltaTime);
                break;
            case State.FLEE:
                timer += Time.deltaTime;
                if (timer > 0.6){
                    state = State.SEEK;
                    escapedOnce = false;
                    timer = 0f;
                } 
                flee = Vector3.Normalize(new Vector3(0.05f, 0, transform.position.z - undesiredPos.z)) * fleeSpeed;
                rb.AddForce((rb.velocity + flee) * Time.deltaTime);

                break;
        }

        if(timer2 > timeTooLong){
            //change strategy
            timer2 = 0f;
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
            Vector3 temp = desiredFinalPos;
            desiredFinalPos = alternativeFinalPos;
            alternativeFinalPos = temp;
            Debug.Log("too looooong, new desired pos = " +desiredFinalPos);
        }

        if(transform.position.x < -13.8 && (transform.position.z < -3.5 || transform.position.z > 3.5)){
            Respawn();
        }

    }

    void CheckObstacles()
    {
        GameObject[] redAgents = GameObject.FindGameObjectsWithTag("Agent");
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] greenAgents = GameObject.FindGameObjectsWithTag("GreenAgent");
        GameObject[] yellowAgents = GameObject.FindGameObjectsWithTag("YellowAgent");

        foreach (GameObject a in redAgents){
            if(a != gameObject && Vector3.Distance(a.transform.position, transform.position) < visionRange ){
                undesiredPos = a.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject g in greenAgents)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < visionRange)
            {
                undesiredPos = g.transform.position;
                state = State.FLEE;
            }
        }
        foreach (GameObject y in yellowAgents)
        {
            if (Vector3.Distance(y.transform.position, transform.position) < visionRange)
            {
                undesiredPos = y.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject c in cubes){
            if(!escapedOnce && Vector3.Distance(c.transform.position, transform.position) <= (visionRange + 0.7)){
                undesiredPos = c.transform.position;
                state = State.FLEE;
                escapedOnce = true;
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

    void Respawn()
    {
        transform.position = new Vector3(13.9f, transform.position.y, 0);
        Start();
    }
}
