using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator1 : MonoBehaviour {

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(-45, -15, -90) * Time.deltaTime);
	}
}
