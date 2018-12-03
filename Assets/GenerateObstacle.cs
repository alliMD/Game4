using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacle : MonoBehaviour {

    List<int> rejected = new List<int>();
    Queue chosen = new Queue();
    bool chooseSecondTile = true;

    //    1  |  2  | 3
    //    -------------
    //    4  |  5  | 6
    //    -------------
    //    7  |  8  | 9

    //    1  |  2  | 3  | 4  | 5
    //    ------------------------
    //    6  |  7  | 8  | 9  | 10
    //    ------------------------
    //    11 |  12 | 13 | 14 | 15
    //    ------------------------
    //    16 | 17  | 18 | 19 | 20
    //    -----------------------


    //    21 | 22  | 23 | 24 | 25

    public int width = 0;
    public int length = 0;



    void Start () {
        System.Random rnd = new System.Random();
        int firstChoice = rnd.Next(1, 10);
        chosen.Enqueue(firstChoice);

        while(chosen.Count > 0){
            int current = (int)chosen.Dequeue();

            Vector3 pos = GetPosition(current);
            //Instantiate(cube, pos, Quaternion.identity);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = pos;
            cube.tag = "Obstacle";
            cube.transform.parent = transform;

            List<int> neig = FindNeighbours(current);

            foreach(int tile in neig){
                if(!rejected.Contains(tile)){
                    if(chooseSecondTile){
                        //make sure the obstacle is at least made of 2 cubes 
                        chosen.Enqueue(tile);
                        chooseSecondTile = false;
                    }else{
                        int chooseToPut = rnd.Next(0, 2);
                        if (chooseToPut == 1) chosen.Enqueue(tile);
                        else rejected.Add(tile);
                    }
                   
                }
            }

        }//end of while loop

        //change of rotation in y for more variety
        int rotate = rnd.Next(0, 2);
        int rotation = rnd.Next(0, 100);
        if (rotate == 1) transform.Rotate(0, rotation, 0);

    }


    List<int> FindNeighbours(int current){
        List<int> neighbourhood = new List<int>();
        switch(current){
            case 1:
                neighbourhood.Add(2);
                neighbourhood.Add(6);
                break;
            case 2:
                neighbourhood.Add(1);
                neighbourhood.Add(3);
                neighbourhood.Add(7);
                break;
            case 3:
                neighbourhood.Add(2);
                neighbourhood.Add(4);
                neighbourhood.Add(8);
                break;
            case 4:
                neighbourhood.Add(3);
                neighbourhood.Add(5);
                neighbourhood.Add(9);
                break;
            case 5:
                neighbourhood.Add(4);
                neighbourhood.Add(10);
                break;
            case 6:
                neighbourhood.Add(1);
                neighbourhood.Add(7);
                neighbourhood.Add(11);
                break;
            case 7:
                neighbourhood.Add(2);
                neighbourhood.Add(6);
                neighbourhood.Add(8);
                neighbourhood.Add(12);
                break;
            case 8:
                neighbourhood.Add(3);
                neighbourhood.Add(7);
                neighbourhood.Add(9);
                neighbourhood.Add(13);
                break;
            case 9:
                neighbourhood.Add(4);
                neighbourhood.Add(8);
                neighbourhood.Add(10);
                neighbourhood.Add(14);
                break;
            case 10:
                neighbourhood.Add(5);
                neighbourhood.Add(9);
                neighbourhood.Add(15);
                break;
            case 11:
                neighbourhood.Add(6);
                neighbourhood.Add(12);
                neighbourhood.Add(16);
                break;
            case 12:
                neighbourhood.Add(7);
                neighbourhood.Add(11);
                neighbourhood.Add(13);
                neighbourhood.Add(17);
                break;
            case 13:
                neighbourhood.Add(8);
                neighbourhood.Add(12);
                neighbourhood.Add(14);
                neighbourhood.Add(18);
                break;
            case 14:
                neighbourhood.Add(9);
                neighbourhood.Add(13);
                neighbourhood.Add(15);
                neighbourhood.Add(19);
                break;
            case 15:
                neighbourhood.Add(10);
                neighbourhood.Add(14);
                neighbourhood.Add(20);
                break;
            case 16:
                neighbourhood.Add(11);
                neighbourhood.Add(17);
                break;
            case 17:
                neighbourhood.Add(12);
                neighbourhood.Add(16);
                neighbourhood.Add(18);
                break;
            case 18:
                neighbourhood.Add(13);
                neighbourhood.Add(17);
                neighbourhood.Add(19);
                break;
            case 19:
                neighbourhood.Add(14);
                neighbourhood.Add(18);
                neighbourhood.Add(20);
                break;
            case 20:
                neighbourhood.Add(15);
                neighbourhood.Add(19);
                break;
        }
        return neighbourhood;
    }

    Vector3 GetPosition(int current){
        Vector3 pos = new Vector3();
        switch (current)
        {
            case 1:
                pos = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 1.5f);
                break;
            case 2:
                pos = new Vector3(transform.position.x -1, transform.position.y, transform.position.z + 1.5f);
                break;
            case 3:
                pos = new Vector3(transform.position.x , transform.position.y, transform.position.z + 1.5f);
                break;
            case 4:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1.5f);
                break;
            case 5:
                pos = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 1.5f);
                break;
            case 6:
                pos = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z + 0.5f);
                break;
            case 7:
                pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z + 0.5f);
                break;
            case 8:
                pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
                break;
            case 9:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 0.5f);
                break;
            case 10:
                pos = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 0.5f);
                break;
            case 11:
                pos = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 0.5f);
                break;
            case 12:
                pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 0.5f);
                break;
            case 13:
                pos = new Vector3(transform.position.x , transform.position.y, transform.position.z - 0.5f);
                break;
            case 14:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 0.5f);
                break;
            case 15:
                pos = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 0.5f);
                break;
            case 16:
                pos = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z - 1.5f);
                break;
            case 17:
                pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 1.5f);
                break;
            case 18:
                pos = new Vector3(transform.position.x , transform.position.y, transform.position.z - 1.5f);
                break;
            case 19:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 1.5f);
                break;
            case 20:
                pos = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 1.5f);
                break;
        }
        return pos;
    }

    List<int> FindNeighbours2(int current)
    {
        List<int> neighbourhood = new List<int>();
        switch (current)
        {
            case 1:
                neighbourhood.Add(2);
                neighbourhood.Add(4);
                break;
            case 2:
                neighbourhood.Add(1);
                neighbourhood.Add(3);
                neighbourhood.Add(5);
                break;
            case 3:
                neighbourhood.Add(2);
                neighbourhood.Add(6);
                break;
            case 4:
                neighbourhood.Add(1);
                neighbourhood.Add(5);
                neighbourhood.Add(7);
                break;
            case 5:
                neighbourhood.Add(2);
                neighbourhood.Add(4);
                neighbourhood.Add(6);
                neighbourhood.Add(8);
                break;
            case 6:
                neighbourhood.Add(3);
                neighbourhood.Add(5);
                neighbourhood.Add(9);
                break;
            case 7:
                neighbourhood.Add(4);
                neighbourhood.Add(8);
                break;
            case 8:
                neighbourhood.Add(5);
                neighbourhood.Add(7);
                neighbourhood.Add(9);
                break;
            case 9:
                neighbourhood.Add(6);
                neighbourhood.Add(8);
                break;
        }
        return neighbourhood;
    }

    Vector3 GetPosition2(int current)
    {
        Vector3 pos = new Vector3();
        switch (current)
        {
            case 1:
                pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z + 1);
                break;
            case 2:
                pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                break;
            case 3:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1);
                break;
            case 4:
                pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                break;
            case 5:
                pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                break;
            case 6:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                break;
            case 7:
                pos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z - 1);
                break;
            case 8:
                pos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                break;
            case 9:
                pos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z - 1);
                break;
        }
        return pos;
    }

}
