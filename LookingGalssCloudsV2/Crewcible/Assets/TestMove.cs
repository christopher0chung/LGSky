using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{

    public float closeXRange;
    public float farXRange;
    public float nearZ;
    public float farZ;

    private float zBetween;
    private float xForThisZ;

    private Vector3 scratch;

    void Update()
    {

        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            transform.position -= Vector3.forward;
        if (Input.GetKey(KeyCode.A))
            transform.position -= Vector3.right;
        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right;


        if (transform.position.z >= farZ)
        {
            scratch = transform.position;
            scratch.z = farZ;
            transform.position = scratch;
        }
        if (transform.position.z <= nearZ)
        {
            scratch = transform.position;
            scratch.z = nearZ;
            transform.position = scratch;
        }

        zBetween = (transform.position.z - nearZ) / (farZ - nearZ);
        xForThisZ = Mathf.Lerp(closeXRange, farXRange, zBetween);

        if (transform.position.x >= xForThisZ)
        {
            scratch = transform.position;
            scratch.x = xForThisZ;
            transform.position = scratch;
        }
        if (transform.position.x <= -xForThisZ)
        {
            scratch = transform.position;
            scratch.x = -xForThisZ;
            transform.position = scratch;
        }
    }
}
