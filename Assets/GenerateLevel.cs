using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour {

    public int numObstacles = 2;
    public int numRedAgents;
    public int numGreenAgents;
    public GameObject obstacle;
    public GameObject redAgent;
    public GameObject greenAgent;
    public GameObject yellowAgent;

    float left = -8.5f;
    float down = -5.5f;
    float offset = 4f;

    float timer1;
    float timer2;
    float oldTime;
    float stepTime;

    int index;

    Vector3 point1 = new Vector3(-13.6f, 0.2f, 0);
    Vector3 point2 = new Vector3(0, 0.2f, 7.9f);
    Vector3 point3 = new Vector3(0, 0.2f, -7.9f);

    List<int> points = new List<int>();

	void Start () {

        index = 1;

        for (int p = 1; p <= 15; p++){
            points.Add(p);
        }
        //spawn obstacles
        while(numObstacles > 0 ){
            numObstacles--;
            Vector3 pos = ChoosePointObstacle();

            Instantiate(obstacle, pos, Quaternion.identity);
        }


        oldTime = 0;
        stepTime = 4f;
    }

    private void Update()
    {
        timer1 += Time.deltaTime;
        if(timer1 >= oldTime && timer1 < stepTime ){
            if(numRedAgents > 0){
                Instantiate(redAgent, new Vector3(13.9f, 0.3f, 0.57f), Quaternion.identity);
                numRedAgents--;
            }
            if(numGreenAgents > 0){
                switch (index){
                    case 1:
                        Instantiate(greenAgent, point1, Quaternion.identity);
                        numGreenAgents--;
                        break;
                    case 2:
                        Instantiate(greenAgent, point2, Quaternion.identity);
                        numGreenAgents--;
                        break;
                    case 3:
                        Instantiate(greenAgent, point3, Quaternion.identity);
                        numGreenAgents--;
                        break;
                }
                index++;
                if (index == 4) index = 1;
            }
            oldTime = stepTime;
            stepTime += stepTime;
        }

       

    }


    Vector3 ChoosePointObstacle()
    {
        Vector3 pos = new Vector3();

        System.Random rnd = new System.Random();
        int point = rnd.Next(1, 11);
        while (!points.Contains(point))
        {
            point = rnd.Next(1, 11);
        }
        points.Remove(point);

        switch (point)
        {
            case 1:
                pos = new Vector3(-8f, 0.51f, 4f);
                break;
            case 2:
                pos = new Vector3(-3f, 0.51f, 5.5f);
                break;
            case 3:
                pos = new Vector3(2.5f, 0.51f, 5f);
                break;
            case 4:
                pos = new Vector3(7.8f, 0.51f, 4f);
                break;
            case 5:
                pos = new Vector3(-4.5f, 0.51f, -0.5f);
                break;
            case 6:
                pos = new Vector3(4.5f, 0.51f, +0.5f);
                break;
            case 7:
                pos = new Vector3(-8f, 0.51f, -4f);
                break;
            case 8:
                pos = new Vector3(-2.5f, 0.51f, -5f);
                break;
            case 9:
                pos = new Vector3(2.4f, 0.51f, -5.5f);
                break;
            case 10:
                pos = new Vector3(7.8f, 0.51f, -5f);
                break;
            
           
        }
        return pos;
    }

}
