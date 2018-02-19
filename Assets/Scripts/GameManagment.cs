using UnityEngine.SceneManagement;
using UnityEngine;
using System;


//controlls (almost) everything in the game
public class GameManagment : MonoBehaviour
{
    public AudioManager audioManager;
    public GameObject level;
    public GameObject meltedIce;
    [HideInInspector]
    public enum State { Alive, Dead, Transcending };
    [HideInInspector]
    public State currentState = State.Alive;
    [SerializeField] float levelLoadDelay = 3f;


    public enum PlayerState { Default, Flaming, Stone};
    public PlayerState currentPlayerState = PlayerState.Default;
    Material currentMaterial;
    [Space]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material flamingMaterial;
    //[SerializeField] Material stoneMaterial;
    [Space]
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem successParticle;

    

    [Space]
    private GameObject player;
    [Space]
    [Header("Impact sound setting")]
    public float minImpactVelocity = 2.4f;
    public float maxImpactVelocity = 10f;
    public float minImpactPitch = .3f;
    public float maxImpactPitch = 1f;
    public float minImpactVolume = 0f;
    public float maxImpactVolume = 0.3f;
    public float velocityThreshold;


    [Header("Rolling sound settings")]
    public float minRollingVelocity = .2f;
    public float maxRollingVelocity = 10f;
    public float minRollingPitch = .3f;
    public float maxRollingPitch = 1f;
    public float minRollingVolume = .8f;
    public float maxRollingVolume = 1f;


    private Rigidbody rb;
    Vector3 lastPos;

    [HideInInspector]
    public int deaths = 0;
    private void Awake()
    {
        Cursor.visible = false;
        audioManager = FindObjectOfType<AudioManager>();
    }
    void Start ()
    {
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
        player = gameObject;
        rb = player.GetComponent<Rigidbody>();
        var lastPos = player.transform.position;
	}

	void Update ()
    {
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentState = State.Transcending;
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            FireBuffPickingUp();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        var rollingVelocity = rb.velocity.sqrMagnitude;
        if (audioManager)
        {
            PlayRollingSound(collision, rollingVelocity);
        }
    }

    private void PlayRollingSound(Collision collision, float rollingVelocity)
    {
        if (rollingVelocity > minRollingVelocity && !audioManager.IsPlaying("Rolling") && collision.gameObject.CompareTag("Untagged") && currentState == State.Alive)
        {
            var audioSource = audioManager.FindClipByName("Rolling");
            audioSource.volume = Mathf.Lerp(minRollingVolume, maxRollingVolume, (rollingVelocity - minRollingVelocity) / maxRollingVelocity);
            audioSource.pitch = Mathf.Lerp(minRollingPitch, maxRollingPitch, (rollingVelocity - minRollingVelocity) / maxImpactVelocity);
            audioManager.Play("Rolling");

        }
        if (rollingVelocity < minRollingVelocity || currentState != State.Alive)
        {
            audioManager.Stop("Rolling");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = collision.relativeVelocity.sqrMagnitude;
        switch (collision.gameObject.tag)
        {
            case "Untagged":
                if (audioManager)
                {
                    PlayImpactSound(impactVelocity);
                }
                break;
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

    private void PlayImpactSound(float impactVelocity)
    {
        if (impactVelocity > minImpactVelocity && !audioManager.IsPlaying("Drop"))
        {
            var audioSource = audioManager.FindClipByName("Drop");
            audioSource.volume = Mathf.Lerp(minImpactVolume, maxImpactVolume, (impactVelocity - minImpactVelocity) / maxImpactVelocity);
            audioSource.pitch = Mathf.Lerp(minImpactPitch, maxImpactPitch, (impactVelocity - minImpactVelocity) / maxImpactVelocity);
            audioManager.Play("Drop");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (audioManager)
            audioManager.Stop("Rolling");
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
                    if (audioManager)
                        audioManager.Play("Melting");
                    Instantiate(meltedIce, collision.transform.position, collision.transform.rotation, level.transform);
                }
                break;
            case "WoodWall":
                if (currentPlayerState != PlayerState.Stone) { return; }
                Destroy(collision.gameObject);
                break;
        }
    }

    /* Possible additional gameplay mechanic
     * public void StoneBuffPickingUp()
    {
        currentPlayerState = PlayerState.Stone;
        gameObject.GetComponent<Renderer>().material = stoneMaterial;
    }*/

    public void FireBuffPickingUp()
    {
        currentPlayerState = PlayerState.Flaming;
        gameObject.GetComponent<Renderer>().material = flamingMaterial;
        player.GetComponent<Light>().color = new Color(0.9f, 0.6f, 0.20f, 0f);
        if (audioManager)
            audioManager.Play("Flaming");
    }
    public void StartSuccessSequence()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        successParticle.Play();
        if (audioManager)
            audioManager.Play("Success");
        player.GetComponent<MeshRenderer>().enabled = !player.GetComponent<MeshRenderer>().enabled;
        currentState = State.Transcending;
        Invoke("LoadNextScene", levelLoadDelay);
    }

    public void StartDeathSequence()
    {
        currentState = State.Dead;
        player.GetComponent<Rigidbody>().isKinematic = true;
        if (audioManager)
            audioManager.Play("Death");
        deathParticle.Play();
        player.GetComponent<MeshRenderer>().enabled = !player.GetComponent<MeshRenderer>().enabled;
        Invoke("LoadNextScene", levelLoadDelay);
        deaths += 1;
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {

            nextSceneIndex = 1; 
        }
        if (currentState == State.Transcending)
        {
            LoadLevel(nextSceneIndex);
            ThemeChangeForTitles(nextSceneIndex);
        }
        else if (currentState == State.Dead)
        {
            LoadLevel(currentSceneIndex);
        }
    }

    private void ThemeChangeForTitles(int nextSceneIndex)
    {
        if (!audioManager)
            return;
        if (nextSceneIndex == 15)
        {
            audioManager.Stop("Theme");
            audioManager.Play("Titles");
        }
        else
        {
            if (!audioManager.IsPlaying("Theme"))
            {
                audioManager.Stop("Titles");
                audioManager.Play("Theme");
            }
        }
    }

    private void LoadLevel(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

}
