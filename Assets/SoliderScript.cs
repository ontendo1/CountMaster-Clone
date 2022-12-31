using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderScript : MonoBehaviour
{
    PlayerScript playerScript;
    void Awake()
    {
        playerScript = transform.parent.GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            playerScript.soliders.Remove(gameObject);
            Destroy(other.gameObject);
            playerScript.ReshapeSoliders();
            Destroy(gameObject);
        }
    }
}
