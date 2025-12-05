using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator anim; // <--- Animasyoncuyu tanımladık

    [Header("Boss Ayarı")]
    [SerializeField] private bool isBossZombie;

    private float giveDamage;
    private float saldiriHizi = 1.5f;
    private float sonVurusZamani;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Zombinin üzerindeki Animator bileşenini buluyoruz
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // --- OTOMATİK AYARLAR (Can ve Hız) ---
        HealthManager myHealth = GetComponent<HealthManager>();

        if (isBossZombie)
        {
            giveDamage = 20;
            if (myHealth != null) myHealth.baslangicCani = 150;
            agent.speed = 1.5f; // Boss yavaş yürür
        }
        else
        {
            giveDamage = 10;
            if (myHealth != null) myHealth.baslangicCani = 100;
            agent.speed = 3.5f; // Normal zombi hızlı yürür
        }

        if (myHealth != null) myHealth.guncelCan = myHealth.baslangicCani;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        // --- ANİMASYON KODU BURASI ---
        // Zombinin hızı 0.1'den büyükse (yürüyorsa)
        if (agent.velocity.magnitude > 0.1f)
        {
            // Animator'daki "IsMoving" kutucuğunu işaretle
            anim.SetBool("IsMoving", true);
        }
        else
        {
            // Duruyorsa işaretini kaldır
            anim.SetBool("IsMoving", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (Time.time > sonVurusZamani + saldiriHizi)
            {
                // SALDIRI ANİMASYONU TETİKLE ("Attack" tetiğini çek)
                anim.SetTrigger("Attack");

                HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
                if (playerHealth != null)
                {
                    playerHealth.HasarAl((int)giveDamage);
                    sonVurusZamani = Time.time;
                }
            }
        }
    }
}
