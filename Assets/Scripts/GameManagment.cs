using UnityEngine.SceneManagement;
using UnityEngine;
using System;

//controlls everything in the game
public class GameManagment : MonoBehaviour
{

    public GameObject level;
    private GameControll gameControllAccess;
    public enum State { Alive, Dead, Transcending };
    public State currentState = State.Alive;
    [SerializeField] float levelLoadDelay = 3f;


    enum PlayerState { Default, Flaming, Stone};
    PlayerState currentPlayerState = PlayerState.Default;
    Material currentMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material flamingMaterial;
    [SerializeField] Material stoneMaterial;

    // Use this for initialization
    void Start ()
    {
        gameControllAccess = level.GetComponent<GameControll>();
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Finish":
                StartSuccessSequence();
                break;
            case "Enemy":
                StartDeathSequence();
                break;
            case "FireBuff":
                Destroy(collision.gameObject);
                FireBuffPickingUp();
                break;
            case "StoneBuff":
                Destroy(collision.gameObject);
                StoneBuffPickingUp();
                break;
            case "IceWall":
                collideWithObsticles("IceWall", collision);
                break;
            case "WoodWall":
                collideWithObsticles("WoodWall", collision);
                break;
        }
    }

    private void collideWithObsticles(string tag, Collision collision)
    {
        switch (tag)
        {
            case "IceWall":
                if (currentPlayerState != PlayerState.Flaming) { return; }
                Destroy(collision.gameObject);
                break;
            case "WoodWall":
                if (currentPlayerState != PlayerState.Stone) { return; }
                Destroy(collision.gameObject);
                break;
        }
    }

    private void StoneBuffPickingUp()
    {
        currentPlayerState = PlayerState.Stone;
        gameObject.GetComponent<Renderer>().material = stoneMaterial;
    }

    void FireBuffPickingUp()
    {
        currentPlayerState = PlayerState.Flaming;
        gameObject.GetComponent<Renderer>().material = flamingMaterial;


    }
    public void StartSuccessSequence()
    {
        currentState = State.Transcending;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    public void StartDeathSequence()
    {
        currentState = State.Dead;
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

}
