﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ShootOnSee : MonoBehaviour {

    public Transform whoAmILookingFor;
    public GameObject bullet;

    private bool haveIFired;
    private float coolDownTimer;

    void Start()
    {
        whoAmILookingFor = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void Update () {

        if (Vector3.Dot(transform.GetChild(0).forward, Vector3.Normalize(whoAmILookingFor.position - transform.position)) >= .75f
            && Vector3.Distance(whoAmILookingFor.position, transform.position) < 7
            && !haveIFired)
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity, null);

            Vector3 shootDir = Quaternion.Euler(Random.insideUnitSphere * 7) *Vector3.Normalize(whoAmILookingFor.position - transform.position);
            newBullet.GetComponent<Rigidbody>().AddForce(shootDir * 7, ForceMode.Impulse);
            haveIFired = true;
        }

        if (haveIFired)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= 3)
            {
                haveIFired = false;
                coolDownTimer = 0;
            }
        }

    }
}
