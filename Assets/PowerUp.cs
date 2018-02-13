using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    


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
            buff.FireBuffPickingUp();

        }
        else if (gameObject.CompareTag("StoneBuff"))
        {
            buff.StoneBuffPickingUp();
        }
        Destroy(gameObject);
        
    }
}

