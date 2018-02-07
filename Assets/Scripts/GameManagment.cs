using UnityEngine.SceneManagement;
using UnityEngine;

//controlls everything in the game
public class GameManagment : MonoBehaviour
{

    public GameObject level;
    private GameControll gameControllAccess;
    public enum State { Alive, Dead, Transcending };
    public State currentState = State.Alive;
    [SerializeField] float levelLoadDelay = 3f;

    
    // Use this for initialization
    void Start ()
    {
        gameControllAccess = level.GetComponent<GameControll>();
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
                case "Fire":
                     //todo create fire effect
                    break;
            }
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
