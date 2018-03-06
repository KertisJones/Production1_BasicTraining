using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {
    //Jesse was here

    public Vector3 speed;
    public Vector3 destination;

	
	// Update is called once per frame
	void Update () {
        this.transform.position += speed;
        if (this.transform.position == destination)
            gameObject.SetActive(false);
	}
}
