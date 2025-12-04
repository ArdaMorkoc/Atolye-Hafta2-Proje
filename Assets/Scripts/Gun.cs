using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Gun : MonoBehaviour
{
    public float range = 1000f;
    public float damage = 100f;
    public Camera fpsCamera;

    public int currentAmmo = 0;
    public int magCapasity = 12;
    public int reservedAmmo = 60;
    public LayerMask enemyLayer;


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
        RaycastHit hit;

        //Eski if döngüsü: Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, enemyLayer)
        
        if (Physics.SphereCast(fpsCamera.transform.position, 4f, fpsCamera.transform.forward, out hit, range, enemyLayer))
        {
            Debug.Log("Hit object: " + hit.transform.name + " | Tag: " + hit.transform.tag);
           
            if (hit.transform.CompareTag("Enemy"))
            {
                
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    
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
