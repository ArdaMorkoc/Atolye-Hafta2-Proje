using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahneyi yeniden başlatmak için gerekli kütüphane

public class HealthManager : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int baslangicCani = 100;
    public int guncelCan;

    [Header("UI Bağlantısı")]
    public Image canBariGorseli;

    void Start()
    {
        guncelCan = baslangicCani; // Oyuna başlarken canı fulle
    }

    // Hasar alma fonksiyonu (Silah veya Zombi bunu çağıracak)
    public void HasarAl(int hasarMiktari)
    {
        guncelCan -= hasarMiktari;

        // --- CAN BARI GÜNCELLEME ---
        if (canBariGorseli != null)
        {
            // Matematik: Güncel Can / Toplam Can (Örn: 80/100 = 0.8)
            canBariGorseli.fillAmount = (float)guncelCan / baslangicCani;
        }

        Debug.Log(gameObject.name + " hasar aldı! Kalan: " + guncelCan);

        if (guncelCan <= 0)
        {
            Oldur();
        }
    }

    void Oldur()
    {
        // Ölen şey Player mı?
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("OYUN BİTTİ! PLAYER ÖLDÜ.");

            // Sahneyi (Leveli) baştan başlat
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        // Yoksa düşman mı?
        else
        {
            Debug.Log("Zombi Geberdi!");

            // Puan ekleme kodu buraya gelebilir

            // Zombiyi sahneden sil
            Destroy(gameObject);
        }
    }
}