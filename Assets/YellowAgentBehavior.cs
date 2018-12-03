using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowAgentBehavior : MonoBehaviour {

    public float visionRangeYellow;
    public float visionRangeRed;
    public float visionRangeGreen;
    public float visionRangeCube;
    public float speedSlow;
    public float speed;
    public float fleeSpeed;

    bool escapingOnce = false;
    bool sawYellowAgent = false;
    public bool isSocial = false;
    public bool isInSocialCircle = false;
    bool actAsGreen;

    int left = -11;
    int bottom = -8;
    int right = 11;
    int top = 8;

    float fleeTimer;

    float wanderingTimer;

    float socialTimer;

    float actGreenTimer;

    float socialCircleTimer;

    Rigidbody rb;

    Vector3 desiredPos;
    Vector3 undesiredPos;
    Vector3 velocity;
    Vector3 flee;
    public Vector3 midpointMeetup;

    List<GameObject> socialCircle;
    List<Vector3> wayPoints;

    int lastPoint;

    enum State{
        SEEK,
        FLEE,
        DECIDE,
        CHOOSE_DEST,
        SOCIALIZE
    }

    State state;

	void Start () {
        lastPoint = 0;
        socialCircleTimer = 0f;
        rb = gameObject.GetComponent<Rigidbody>();
        state = State.CHOOSE_DEST;
	}

	void Update () {
        CheckObstacles();

        if(actAsGreen){
            state = State.SEEK;
            actGreenTimer += Time.deltaTime;
            if(actGreenTimer > 4){
                actAsGreen = false;
                actGreenTimer = 0f;
            }
        }

        switch(state){
            case State.CHOOSE_DEST:
                wanderingTimer = 0f;
                lastPoint = ChoosePoint(lastPoint);
                Debug.Log("desired pos = " + desiredPos);
                state = State.SEEK;
                break;
            case State.SEEK:
                wanderingTimer += Time.deltaTime;
                if(wanderingTimer > 9){
                    wanderingTimer = 0f;
                    state = State.CHOOSE_DEST;
                }else{
                    float x1 = transform.position.x;
                    float z1 = transform.position.z;
                    velocity = Vector3.Normalize(new Vector3(desiredPos.x - x1, 0, desiredPos.z - z1)) * speed;
                    rb.AddForce(velocity * Time.deltaTime);

                    if (Mathf.Abs(x1 - desiredPos.x) < 1.7 && Mathf.Abs(z1 - desiredPos.z) < 1.7)
                    {
                        state = State.CHOOSE_DEST;
                    }
                }
               
                break;
            case State.FLEE:
                fleeTimer += Time.deltaTime;
                if (fleeTimer > 0.6)
                {
                    state = State.SEEK;
                    escapingOnce = false;
                    sawYellowAgent = false;
                    fleeTimer = 0f;
                }
                flee = Vector3.Normalize(new Vector3(0.05f, 0, transform.position.z - undesiredPos.z)) * fleeSpeed;
                rb.AddForce((rb.velocity + flee) * Time.deltaTime);
                break;
            case State.DECIDE:
                if(isSocial){
                   
                }
                else{
                    state = State.FLEE;
                }
                break;
            case State.SOCIALIZE:
                socialTimer += Time.deltaTime;

                if(socialTimer > 2.5){
                    socialTimer = 0;
                    GameObject[] yellowAgents = GameObject.FindGameObjectsWithTag("YellowAgent");
                    foreach (GameObject y in yellowAgents)
                    {
                        if(y != gameObject && Vector3.Distance(transform.position, y.transform.position) < visionRangeYellow -1.5){
                            if (y.GetComponent<YellowAgentBehavior>().state == State.SOCIALIZE)
                            {
                                isSocial = true;
                            }
                            else
                            {
                                state = State.FLEE;
                            }
                        }

                    }
                }
                if(isSocial){
                    velocity = Vector3.zero;
                    float x = transform.position.x;
                    float z = transform.position.z;
                    Vector3 desiredVelocity = Vector3.Normalize(new Vector3(midpointMeetup.x - x, 0, midpointMeetup.z - z)) * speedSlow;
                    if (Mathf.Abs(x - midpointMeetup.x) < 0.5 && Mathf.Abs(z - midpointMeetup.z) < 0.5)
                    {
                        isInSocialCircle = true;
                        rb.velocity = Vector3.zero;
                    }
                    else
                    {
                        rb.AddForce(desiredVelocity * Time.deltaTime);
                    }
                }
                if(isInSocialCircle){
                    socialCircleTimer += Time.deltaTime;
                    if(socialCircleTimer > 15){
                        //stop socializing
                        ChoosePoint(lastPoint);
                        isSocial = false;
                        isInSocialCircle = false;
                        sawYellowAgent = false;
                        actAsGreen = true;
                    }
                }

                break;
        }
	}

    void CheckObstacles(){
        GameObject[] yellowAgents = GameObject.FindGameObjectsWithTag("YellowAgent");
        GameObject[] redAgents = GameObject.FindGameObjectsWithTag("Agent");
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] greenAgents = GameObject.FindGameObjectsWithTag("GreenAgent");


        socialCircle = new List<GameObject>();
        foreach (GameObject y in yellowAgents)
        {
            if (y != gameObject && Vector3.Distance(y.transform.position, transform.position) < visionRangeYellow && !sawYellowAgent)
            {
                if (y.GetComponent<YellowAgentBehavior>().isInSocialCircle){
                    Debug.Log("other agent is inside a social circle, midpoint = " + y.GetComponent<YellowAgentBehavior>().midpointMeetup);
                    midpointMeetup = y.GetComponent<YellowAgentBehavior>().midpointMeetup;
                }else{
                    float x = (y.transform.position.x + transform.position.x) / 2;
                    float z = (y.transform.position.z + transform.position.z) / 2;
                    sawYellowAgent = true;
                    midpointMeetup = new Vector3(x, transform.position.y, z);
                    undesiredPos = midpointMeetup;
                }
                    

                //1 chance in 8 to not socialize
                System.Random rnd = new System.Random();
                int choose = rnd.Next(0, 9);
                if (choose == 8 || actAsGreen)
                {
                    state = State.FLEE;
                    Debug.Log("yellow agent decided to wander");
                }
                else
                {
                    isSocial = true;
                    state = State.SOCIALIZE;
                    Debug.Log("yellow agent decided to socialize");
                }

            }
        }

        foreach (GameObject a in redAgents)
        {
            if (Vector3.Distance(a.transform.position, transform.position) < visionRangeRed)
            {
                undesiredPos = a.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject g in greenAgents)
        {
            if (g != gameObject && Vector3.Distance(g.transform.position, transform.position) < visionRangeGreen)
            {
                undesiredPos = g.transform.position;
                state = State.FLEE;
            }
        }

        foreach (GameObject c in cubes)
        {
            if (!escapingOnce && Vector3.Distance(c.transform.position, transform.position) <= visionRangeCube)
            {
                undesiredPos = c.transform.position;
                escapingOnce = true;
                state = State.FLEE;
            }
        }
    }

    int ChoosePoint(int pastPoint){
        System.Random rnd = new System.Random();
        int point = rnd.Next(1, 7);
        while(point == pastPoint){
            point = rnd.Next(1, 7);
        }
        switch(point){
            case 1:
                desiredPos = new Vector3(-10f, 0f, 0f);
                break;
            case 2:
                desiredPos = new Vector3(-5f, 0f, -7f);
                break;
            case 3:
                desiredPos = new Vector3(-4f, 0f, 5f);
                break;
            case 4:
                desiredPos = new Vector3(4f, 0f, 5f);
                break;
            case 5:
                desiredPos = new Vector3(5f, 0f, -6f);
                break;
            case 6:
                desiredPos = new Vector3(10f, 0f, 0f);
                break;

        }
        return point;
    }

    void WhosIsInCircle(){
        GameObject[] yellowAgents = GameObject.FindGameObjectsWithTag("YellowAgent");
        foreach (GameObject y in yellowAgents)
        {
            if (y != gameObject && Vector3.Distance(transform.position, y.transform.position) < visionRangeYellow - 1.5)
            {
                if (y.GetComponent<YellowAgentBehavior>().state == State.SOCIALIZE)
                {
                    socialCircle.Add(y);
                }
            }

        }
    }

   
}
