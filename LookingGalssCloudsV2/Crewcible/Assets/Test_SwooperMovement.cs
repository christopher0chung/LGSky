﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SwooperMovement : MonoBehaviour
{
    Transform target;
    public float turnSpeed;
    public float moveSpeed;

    private Vector3 offset;
    private bool close;
    private bool closeTrigger;

    private float turnSpeedMin;
    private float turnSpeedMax;
    private float moveSpeedMin;
    private float moveSpeedMax;
    private Vector3 newUp;

    public GameObject newBullet;

    private bool fired;
    private bool readyToFire;

    public float fireDist;

    void Start()
    {
        target = ServiceLocator.instance.Player;
        offset = Random.insideUnitSphere * 10;

        turnSpeedMin = turnSpeed * .8f;
        turnSpeedMax = turnSpeed * 1.2f;

        moveSpeedMin = moveSpeed * .8f;
        moveSpeedMax = moveSpeed * 1.2f;

        newUp = Vector3.Normalize(Random.insideUnitSphere);
        fired = true;

        transform.position = transform.position + Random.insideUnitSphere * 20;
        transform.rotation = Quaternion.LookRotation(Random.insideUnitSphere);
    }

    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position + offset + Vector3.up * 7, newUp), turnSpeed * Time.deltaTime);
        transform.position += transform.forward * moveSpeed * Time.deltaTime + Vector3.back * 30 * Time.deltaTime;

        if (Vector3.Distance(target.position, transform.position) < 15)
        {
            close = true;
        }
        else
        {
            close = false;
            closeTrigger = false;

        }

        if (close && !closeTrigger)
        {
            offset = Random.insideUnitSphere * 10;
            closeTrigger = true;

            newUp = Vector3.Normalize(Random.insideUnitSphere);

            turnSpeed *= Random.Range(.9f, 1.1f);
            turnSpeed = Mathf.Clamp(turnSpeed, turnSpeedMin, turnSpeedMax);

            moveSpeed *= Random.Range(.9f, 1.1f);
            moveSpeed = Mathf.Clamp(moveSpeed, moveSpeedMin, moveSpeedMax);
            fired = false;
        }

        if (Vector3.Distance(transform.position, target.position) > fireDist - 2 && Vector3.Distance(transform.position, target.position) < fireDist + 2)
            readyToFire = true;
        else
            readyToFire = false;

        if (!fired &&
            readyToFire &&
            Vector3.Dot(transform.forward, Vector3.Normalize(target.position - transform.position)) < -.5 &&
            Vector3.Dot(transform.forward, Vector3.forward) > 0 &&
            transform.position.y > target.transform.position.y + 1)
        {
            readyToFire = false;
            fired = true;
            GameObject bullet = Instantiate(newBullet, transform.position, Quaternion.LookRotation(target.position - transform.position));
        }
    }
}