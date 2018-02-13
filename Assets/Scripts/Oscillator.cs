using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;
    // todo remove from inspector later
    float movementFactor; //0 for not moved 1 for moved fully

    private Vector3 startingPos;
    private Vector3 movement = new Vector3(10f, 10f, 10f);

    [SerializeField] float period = 5f;


    const float tau = Mathf.PI * 2;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (period <= Mathf.Epsilon /*smallest float in unity */ ) { return; }
        else
        {
            float cycles = Time.time / period; //grows continually from 10
            float rawSinWave = Mathf.Sin(cycles * tau); //goes from -1 to 1
            movementFactor = rawSinWave / 2 + 0.5f; //goes from 0 to 1

            movement = movementVector * movementFactor;
            transform.position = startingPos + movement;
        }
    }
}
