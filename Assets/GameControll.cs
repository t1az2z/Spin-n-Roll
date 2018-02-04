using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControll : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;

    enum State { Alive, Dead, Transcending};
    State currentState = State.Alive;


    Rigidbody rigidBody;

    [SerializeField] float levelLoadDelay = 3f;
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (currentState == State.Alive)
        {
            RotateLevel();
            if (transform.position.y <= -15f) { StartDeathSequence(); } //preventing from falling out of labirinth
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Finish":
                StartSuccessSequence();
                break;
            case "Enemy":
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        currentState = State.Dead;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        currentState = State.Transcending;
        Invoke("LoadNextScene", levelLoadDelay);
    }
    void LoadNextScene()
    {
        if (currentState == State.Transcending)
        {
            print("Level compllete!");
            LoadLevel(0);
        }
        else if (currentState == State.Dead)
        {
            print("You died!");
            LoadLevel(0);
        }
    }
    private void LoadLevel(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    private void Update()
    {


    }
    private void RotateLevel()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
