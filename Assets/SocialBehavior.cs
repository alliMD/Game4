using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialBehavior : MonoBehaviour {
    public float speed;
    public float speedSlow;
    public float visionRangeYellow;
    public float visionRange;
    public float fleeSpeed;

    Vector3 velocity;
    Rigidbody rb;
    Vector3 midpoint;
    Vector3 flee;

    Vector3 undesiredPos;

    bool seingOnce = false;

    bool escapingOnce = false;

    float timer;

    float timerSocial;

    enum State{
        SEEK,
        FLEE,
        DECIDE,
        SOCIALIZE
    }

    State state;

    // Use this for initialization
    void Start () {
        timer = 0f;
        timerSocial = 0f;
        rb = gameObject.GetComponent<Rigidbody>();

        if(transform.position.x < 0){
            velocity = new Vector3(speed, 0, 0);
        }else{
            velocity = new Vector3(-speed, 0, 0);
        }

        state = State.SEEK;
	}
	
	// Update is called once per frame
	void Update () {
        CheckNearby();

        if(transform.position.x > 11){
           
            velocity = new Vector3(-speed, 0 , 0);
        }else if(transform.position.x < -11 ){
           
            velocity = new Vector3(speed, 0, 0);
        }

        switch(state){
            case State.SEEK:
                rb.AddForce(velocity * Time.deltaTime);
                break;
            case State.FLEE:
                timer += Time.deltaTime;
                if (timer > 0.5)
                {
                    state = State.SEEK;
                    escapingOnce = false;
                    timer = 0f;
                }
                flee = Vector3.Normalize(new Vector3(0.05f, 0, transform.position.z - undesiredPos.z)) * fleeSpeed;
                rb.AddForce((rb.velocity + flee) * Time.deltaTime);
                break;
            case State.DECIDE:
                System.Random rnd = new System.Random();
                int choose = rnd.Next(0, 9);
                if(choose == 8){
                    state = State.FLEE;
                    Debug.Log("yellow agent decided to wander");
                }else{
                    state = State.SOCIALIZE;
                }
                break;
            case State.SOCIALIZE:
                timerSocial += Time.deltaTime;
                if(timerSocial > 3){
                    //decide to stay in the social group or leave
                    state = State.DECIDE;
                    timerSocial = 0f;
                }
                velocity = Vector3.zero;
                float x = transform.position.x;
                float z = transform.position.z;
                Vector3 desiredVelocity = Vector3.Normalize(new Vector3(midpoint.x - x, 0, midpoint.z - z)) * speedSlow;
                if (Mathf.Abs(x - midpoint.x) < 0.5 && Mathf.Abs(z - midpoint.z) < 0.5)
                {
                    rb.velocity = Vector3.zero;
                }else{
                    rb.AddForce(desiredVelocity * Time.deltaTime);
                }

                break;
        }
       

    }

    void CheckNearby(){
        GameObject[] yellowAgents = GameObject.FindGameObjectsWithTag("YellowAgent");
        GameObject[] redAgents = GameObject.FindGameObjectsWithTag("Agent");
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] greenAgents = GameObject.FindGameObjectsWithTag("GreenAgent");

        foreach (GameObject y in yellowAgents){
            if(y != gameObject && Vector3.Distance(y.transform.position, transform.position) < visionRangeYellow && !seingOnce){
                float x = (y.transform.position.x + transform.position.x) / 2;
                float z = (y.transform.position.z + transform.position.z) / 2;
                midpoint = new Vector3(x, transform.position.y, z);
                undesiredPos = midpoint;
                state = State.DECIDE;
                seingOnce = true;
            }
        }

        foreach (GameObject a in redAgents)
        {
            if (Vector3.Distance(a.transform.position, transform.position) < visionRange)
            {
                undesiredPos = a.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject g in greenAgents)
        {
            if (g != gameObject && Vector3.Distance(g.transform.position, transform.position) < visionRange)
            {
                undesiredPos = g.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject c in cubes)
        {
            if ( !escapingOnce &&Vector3.Distance(c.transform.position, transform.position) <= visionRange + 1)
            {
                undesiredPos = c.transform.position;
                escapingOnce = true;
                state = State.FLEE;
            }
        }

    }
}
