using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private float health = 100;
    [SerializeField] private bool isBossZombie;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void TakeDamage()
    {
        if (isBossZombie)
        {
            health -= 10;
        }
        else
        {
            health -= 20;
        }

        if(health <= 0)
        {
            Destroy(gameObject, 3.0f);
        }
    }

}
