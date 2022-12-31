using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    GameObject player;
    [SerializeField] float attackSpeed = 1;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * attackSpeed * Time.deltaTime);
    }
}
