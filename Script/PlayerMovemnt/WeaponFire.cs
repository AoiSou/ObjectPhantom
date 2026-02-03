using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class WeaponFire : MonoBehaviour
{
    [Header("References")] 
    public Transform cam;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Settings")] 
    public int totalAmmos;
    public float maxAmmo;
    public float reloadTime;
    public float magazineBullets;
    public float fireRate;
    
    [Header("SE")]
    [SerializeField]private AudioSource FireSound;
    [SerializeField]private AudioSource Reload1Sound;
    [SerializeField]private AudioSource Reload2Sound;
    [SerializeField]private AudioClip FireSoundClip;
    [SerializeField]private AudioClip Reload1SoundClip;
    [SerializeField]private AudioClip Reload2SoundClip;
    
    [Header("Fire Settings")]
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode fireKey = KeyCode.Mouse0;
    public float fireForce;
    public float fireUpward;

    private bool peeking;
    
    private Animator anim;
    
    [Header("UI")]
    [SerializeField]private TextMeshProUGUI ammoText;
    [SerializeField]private TextMeshProUGUI totalAmmoText;
    [SerializeField]private Image ammoImage;
    
    bool readyToFire;
    bool reloading;

    void Start()
    {
        readyToFire = true;
        reloading = false;
        ammoText.text = magazineBullets.ToString();
        totalAmmoText.text = "/"+totalAmmos.ToString();
        
        anim = GetComponent<Animator>();

        Gage();
    }

    void Update()
    {
        if (Input.GetKey(fireKey)&&readyToFire&&magazineBullets>0)
        {
            Fire();
        }

        if (Input.GetKeyDown(reloadKey)&&totalAmmos>0&&magazineBullets!=maxAmmo&&!reloading)
        {
            anim.SetBool("Relod", true);
            readyToFire = false;
            reloading = true;
            Reload1Sound.PlayOneShot(Reload1SoundClip);
            Invoke(nameof(Reload), reloadTime);
        }
        
        Debug.DrawRay(firePoint.position,  cam.forward* 100f, Color.red);
    }

    void Fire()
    {
        readyToFire = false;
        
        FireSound.PlayOneShot(FireSoundClip);
        
        GameObject projectile =Instantiate(bulletPrefab,firePoint.position,cam.rotation);
        
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        //
        Rigidbody playerRb = GetComponentInParent<Rigidbody>();
        if (playerRb != null)
        {
            projectileRb.velocity = playerRb.velocity*2f;
        }
        
        //
        Vector3 forceDirection = cam.transform.forward;
        
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection=(hit.point -firePoint.position).normalized;
        }
        
        Vector3 forceToAdd=forceDirection*fireForce+transform.up*fireUpward;

        projectileRb.AddForce(forceToAdd,ForceMode.Impulse);
        
        //
        
        //
        
        magazineBullets--;
        
        if(!reloading)
            Invoke(nameof(ResetFire),fireRate);
        
        ammoText.text = magazineBullets.ToString();
        totalAmmoText.text = "/"+totalAmmos.ToString();
        
        Gage();
    }
    void ResetFire()
    {
        readyToFire = true;
    }

    void Gage()
    {
        ammoImage.fillAmount = magazineBullets/maxAmmo;
        //Debug.Log($"{magazineBullets}/{maxAmmo} = {magazineBullets/ maxAmmo}");
    }

    void Reload()
    {
        float addAmmo =maxAmmo - magazineBullets;

        anim.SetBool("Relod", false);
        
        if (totalAmmos < addAmmo)
        {
            magazineBullets+=totalAmmos;
            
            totalAmmos = 0;
        }
        else
        {
            totalAmmos-=Mathf.RoundToInt(addAmmo);
        
            magazineBullets+=addAmmo;   
        }
        
        Reload2Sound.PlayOneShot(Reload2SoundClip);
        
        ammoText.text = magazineBullets.ToString();
        totalAmmoText.text = "/"+totalAmmos.ToString();
        
        readyToFire = true;
        reloading = false;

        Gage();
    }
}
