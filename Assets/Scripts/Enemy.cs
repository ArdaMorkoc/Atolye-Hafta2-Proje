using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Boss Ayarı")]
    [SerializeField] private bool isBossZombie; // Bunu işaretlersen Boss olur

    private float giveDamage; // Player'a ne kadar vuracak?
    private float saldiriHizi = 1.0f; // Saniyede kaç kere vursun?
    private float sonVurusZamani;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Player'ı güvenli bir şekilde bul
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // --- OTOMATİK AYAR KISMI ---
        // Kendi üzerindeki HealthManager'ı bulup canı ayarlayalım
        HealthManager myHealth = GetComponent<HealthManager>();

        if (isBossZombie)
        {
            giveDamage = 20; // Boss hasarı
            if (myHealth != null) myHealth.baslangicCani = 150; // Boss canı
        }
        else
        {
            giveDamage = 10; // Normal hasar
            if (myHealth != null) myHealth.baslangicCani = 100; // Normal can
        }

        // Eğer oyun başladığında HealthManager start almışsa, güncel canı da eşitlemek lazım
        if (myHealth != null) myHealth.guncelCan = myHealth.baslangicCani;
    }

    void Update()
    {
        // Player yaşıyorsa kovala
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // Player ile çarpışınca çalışır
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            // Sürekli can gitmesin diye süre kontrolü (Cooldown)
            if (Time.time > sonVurusZamani + saldiriHizi)
            {
                // Player'ın üzerindeki HealthManager'ı bul
                HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();

                if (playerHealth != null)
                {
                    // Hasarı ver (Int istiyordu, float'ı int'e çevirdik)
                    playerHealth.HasarAl((int)giveDamage);
                    sonVurusZamani = Time.time;
                    Debug.Log("Zombi vurdu! Hasar: " + giveDamage);
                }
            }
        }
    }
}
