using UnityEngine;

//Controlls level rotation
public class GameControll : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 100f;

    public GameObject player;

    private GameManagment gameManagmentAccess;

    

    void Start ()
    {
        gameManagmentAccess = player.GetComponent<GameManagment>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (gameManagmentAccess.currentState == GameManagment.State.Alive)
        {
            Controls();
            if (player.transform.position.y <= -10f) { gameManagmentAccess.StartDeathSequence(); } //preventing from falling out of labirinth
        }
    }
    
    private void Controls()
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
