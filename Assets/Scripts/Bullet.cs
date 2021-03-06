﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject sandHitEffect;
    public GameObject fleshHitEffect;
    public GameObject woodHitEffect;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyUnit>().DealDamage(1);//Insert Damage here

            //Insert fleshHitEffects here
            Instantiate(fleshHitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (col.gameObject.tag == "Terrain")
        {
            Instantiate(woodHitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
