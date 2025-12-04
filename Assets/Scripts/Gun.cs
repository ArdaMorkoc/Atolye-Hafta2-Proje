using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 1000f;
    public float damage = 100f;
    public Camera fpsCamera;

    public int currentAmmo = 0;
    public int magCapasity = 12;
    public int reservedAmmo = 60;

    [Header("UI Kýsmý")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI reservedAmmoText;

    private void Start()
    {
        ammoText.text = currentAmmo.ToString() + "/" + magCapasity.ToString();
        reservedAmmoText.text = reservedAmmo.ToString();
    }
    private void Update()
    {
        //Kurþunun gittiði çizgiyi görmek için
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range, Color.green);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                if (currentAmmo > 0)
                {
                    Shoot();
                    
                    if (currentAmmo <= magCapasity/2)
                        ammoText.color = Color.yellow;
                }
                    
                else
                    ammoText.color = Color.red;
        }
    }
    
    void Shoot()
    {
        //debuglar döngü kontrolleri için eklendi
        RaycastHit hit;
        Debug.Log("Shoot çalýþtý");

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            
            Debug.Log("1. if'e girdi");
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("compare tag çalýþtý");
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    Debug.Log("rb null deðil");
                    rb.AddForce(-hit.normal * damage, ForceMode.Impulse);
                    hit.collider.gameObject.GetComponent<Enemy>().TakeDamage();
                }

                
            }
        }

        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        ammoText.text = currentAmmo.ToString() + "/" + magCapasity.ToString();
        reservedAmmoText.text = reservedAmmo.ToString();
    }

    public void Reload()
    {
        int ammoToLoad = Mathf.Min(magCapasity - currentAmmo, reservedAmmo);
        currentAmmo += ammoToLoad;
        reservedAmmo -= ammoToLoad;
        ammoText.text = currentAmmo.ToString() + "/" + magCapasity.ToString();
        reservedAmmoText.text = reservedAmmo.ToString();
        ammoText.color = Color.white;
    }
}
