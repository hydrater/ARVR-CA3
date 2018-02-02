using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject sandHitEffect;
    public GameObject[] fleshHitEffects;
    public GameObject woodHitEffect;

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    public Camera Cam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public VRTK.VRTK_ControllerEvents controllerEvents;
	
	// Update is called once per frame
	void Update () {

        OVRInput.Update(); // Call before checking the input

        if (controllerEvents.triggerPressed && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
	}

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyUnit target = hit.transform.root.GetComponent<EnemyUnit>();
            if (target != null)
            {
                target.DealDamage(10);
            }

            if (hit.collider.sharedMaterial != null)
            {
                string materialName = hit.collider.sharedMaterial.name;

                switch (materialName)
                {
                    case "Sand":
                        SpawnDecal(hit, sandHitEffect);
                        break;         
                    case "Wood":
                        SpawnDecal(hit, woodHitEffect);
                        break;
                    case "Meat":
                        SpawnDecal(hit, fleshHitEffects[Random.Range(0, fleshHitEffects.Length)]);
                        break;
                    case "Character":
                        SpawnDecal(hit, fleshHitEffects[Random.Range(0, fleshHitEffects.Length)]);
                        break;
                }
            }
        }
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
        spawnedDecal.transform.SetParent(hit.collider.transform);
        Destroy(spawnedDecal, 2f);
    }
}
