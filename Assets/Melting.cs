using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melting : MonoBehaviour {

    public ParticleSystem vape;

    private void Start()
    { 

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<GameManagment>().currentPlayerState == GameManagment.PlayerState.Flaming)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
            gameObject.GetComponent<BoxCollider>().enabled = !gameObject.GetComponent<BoxCollider>().enabled;
            vape.Play();

        }
    }
}
