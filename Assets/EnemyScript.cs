using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    GameObject player;
    [SerializeField] float triggerDistance;
    List<Transform> childs = new List<Transform>();
    List<GameObject> enemies = new List<GameObject>();
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        childs = GetComponentsInChildren<Transform>().ToList();
        foreach (Transform t in childs)
        {
            if (t.gameObject.transform.parent != null)
                enemies.Add(t.gameObject);
        }
    }
    void Update()
    {
        if (player.transform.position.x < transform.position.x + triggerDistance)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {        // foreach/)

    }
}
