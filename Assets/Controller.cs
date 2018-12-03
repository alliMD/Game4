using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public float speed = 2;

	
	// Update is called once per frame
	void Update () {
        float currentPos = transform.position.x;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;


        transform.Translate(x, 0, 0);
        transform.Translate(0, 0, z);
    }

}
