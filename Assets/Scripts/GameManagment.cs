using UnityEngine.SceneManagement;
using UnityEngine;
using System;


//controlls everything in the game
public class GameManagment : MonoBehaviour
{

    public GameObject level;
    //private GameControll gameControllAccess;
    public enum State { Alive, Dead, Transcending };
    //private Light playerLight; //decide to use or not
    public State currentState = State.Alive;
    [SerializeField] float levelLoadDelay = 3f;


    public enum PlayerState { Default, Flaming, Stone};
    PlayerState currentPlayerState = PlayerState.Default;
    Material currentMaterial;
    [Space]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material flamingMaterial;
    [SerializeField] Material stoneMaterial;
    [Space]
    [SerializeField] ParticleSystem deathParticle;

    [Space]
    public GameObject meltedIce;
    private GameObject player;
    // Use this for initialization
    void Start ()
    {
        //gameControllAccess = level.GetComponent<GameControll>();
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
        //playerLight = gameObject.GetComponent<Light>();
        player = GameObject.Find("Player");
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
                if (currentPlayerState != PlayerState.Flaming)
                {
                    StartDeathSequence();
                }
                else
                {
                    Destroy(collision.gameObject);
                    Instantiate(meltedIce, collision.transform.position, collision.transform.rotation, level.transform);
                }
                break;
            case "WoodWall":
                if (currentPlayerState != PlayerState.Stone) { return; }
                Destroy(collision.gameObject);
                break;
        }
    }

    public void StoneBuffPickingUp()
    {
        currentPlayerState = PlayerState.Stone;
        gameObject.GetComponent<Renderer>().material = stoneMaterial;
    }

    public void FireBuffPickingUp()
    {
        currentPlayerState = PlayerState.Flaming;
        gameObject.GetComponent<Renderer>().material = flamingMaterial;
        player.GetComponent<Light>().color = new Color(0.9f, 0.6f, 0.20f, 0f); 
    }
    public void StartSuccessSequence()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        currentState = State.Transcending;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    public void StartDeathSequence()
    {
        currentState = State.Dead;
        player.GetComponent<Rigidbody>().isKinematic = true;
        deathParticle.Play();
        player.GetComponent<MeshRenderer>().enabled = !player.GetComponent<MeshRenderer>().enabled;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; //todo show end titles =)
        }
        if (currentState == State.Transcending)
        {
            LoadLevel(nextSceneIndex);
        }
        else if (currentState == State.Dead)
        {
            LoadLevel(currentSceneIndex);
        }
    }
    private void LoadLevel(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

}
