using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject firePickingUpParticle;
    private GameObject level;
    private void Start()
    {
        level = GameObject.Find("Level");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {
        GameManagment buff = player.GetComponent<GameManagment>();
        if (gameObject.CompareTag("FireBuff"))
        {
            Instantiate(firePickingUpParticle, player.transform.position, level.transform.rotation, level.transform);
            buff.FireBuffPickingUp();

        }
        else if (gameObject.CompareTag("StoneBuff"))
        {
            buff.StoneBuffPickingUp();
        }
        Destroy(gameObject);
        
    }
}

