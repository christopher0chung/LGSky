using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Ship_Pilot : MonoBehaviour {

    public Vector3 leftBoundVector;
    public Vector3 rightBoundVector;
    public Vector3 leftInVector;
    public Vector3 rightInVector;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    #region Pilot
    Vector3 inputDirRaw;
    Vector3 inputDirNorm;
    Vector3 moveDir;
    float xBoundCalc;
    Vector3 limitPos;

    //private void _Pilot(float inputX, float inputY, bool jump)
    //{
    //    xBoundCalc = Mathf.Lerp(xBoundClose, xBoundFar, (transform.position.z - zBoundClose) / (zBoundFar - zBoundClose));

    //    inputDirRaw.x = inputX;
    //    inputDirRaw.z = inputY;

    //    inputDirNorm = Vector3.Normalize(inputDirRaw);

    //    if (transform.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
    //        moveDir = inputDirRaw;
    //    else if (transform.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
    //        moveDir = inputDirRaw;

    //    else if (transform.position.z <= zBoundClose && Vector3.Dot(inputDirNorm, Vector3.forward) > 0)
    //        moveDir = inputDirRaw;
    //    else if (transform.position.z >= zBoundFar && Vector3.Dot(inputDirNorm, Vector3.back) > 0)
    //        moveDir = inputDirRaw;

    //    else moveDir = inputDirRaw;

    //    transform.position += moveDir * Time.deltaTime * moveVel;

    //    if (jump)
    //        transform.position += moveDir * 3;


    //    limitPos = transform.position;
    //    if (transform.position.x < -xBoundCalc)
    //        limitPos.x = -xBoundCalc;
    //    if (transform.position.x > xBoundCalc)
    //        limitPos.x = xBoundCalc;
    //    if (transform.position.z < zBoundClose)
    //        limitPos.z = zBoundClose;
    //    if (transform.position.z > zBoundFar)
    //        limitPos.z = zBoundFar;

    //    transform.position = limitPos;
    //}
    #endregion
}
