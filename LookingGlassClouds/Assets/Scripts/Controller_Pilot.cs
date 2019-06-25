using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Pilot : MonoBehaviour {

    public Model_Game gameModel;
    public Model_Energy energyModel;
    public Model_Input inputModel;

    public Transform player;

    public float xBoundClose;
    public float xBoundFar;
    public float zBoundClose;
    public float zBoundFar;

    private Vector3 leftBoundVector;
    private Vector3 rightBoundVector;
    private Vector3 leftInVector;
    private Vector3 rightInVector;

    void Start () {
        leftBoundVector = Vector3.Normalize(new Vector3(-xBoundFar - -xBoundClose, 0, zBoundFar - zBoundClose));
        leftInVector = Vector3.Cross(leftBoundVector, Vector3.up);

        rightBoundVector = Vector3.Normalize(new Vector3(xBoundFar - xBoundClose, 0, zBoundFar - zBoundClose));
        rightInVector = Vector3.Cross(rightBoundVector, Vector3.down);
    }
	
	void Update () {
        if (gameModel.leftStation == Stations.Pilot)
            _Pilot(inputModel.L_X, inputModel.L_Y, inputModel.L_Action_OnDown);
        else if (gameModel.rightStation == Stations.Pilot)
            _Pilot(inputModel.R_X, inputModel.R_Y, inputModel.R_Action_OnDown);
    }

    #region Pilot
    Vector3 inputDirRaw;
    Vector3 inputDirNorm;
    Vector3 moveDir;
    float xBoundCalc;
    Vector3 limitPos;

    private void _Pilot(float inputX, float inputY, bool jump)
    {
        xBoundCalc = Mathf.Lerp(xBoundClose, xBoundFar, (player.position.z - zBoundClose) / (zBoundFar - zBoundClose));

        inputDirRaw.x = inputX;
        inputDirRaw.z = inputY;

        inputDirNorm = Vector3.Normalize(inputDirRaw);

        if (Vector3.Magnitude(inputDirNorm) >= .05f)
            energyModel.pilotOn = true;
        else
            energyModel.pilotOn = false;

        if (player.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;
        else if (player.position.x <= -xBoundCalc && Vector3.Dot(inputDirNorm, leftInVector) > 0)
            moveDir = inputDirRaw;

        else if (player.position.z <= zBoundClose && Vector3.Dot(inputDirNorm, Vector3.forward) > 0)
            moveDir = inputDirRaw;
        else if (player.position.z >= zBoundFar && Vector3.Dot(inputDirNorm, Vector3.back) > 0)
            moveDir = inputDirRaw;

        else moveDir = inputDirRaw;

        player.position += moveDir * Time.deltaTime * gameModel.flySpeed;

        if (jump)
            player.position += moveDir * gameModel.boostDist;


        limitPos = player.position;
        if (player.position.x < -xBoundCalc)
            limitPos.x = -xBoundCalc;
        if (player.position.x > xBoundCalc)
            limitPos.x = xBoundCalc;
        if (player.position.z < zBoundClose)
            limitPos.z = zBoundClose;
        if (player.position.z > zBoundFar)
            limitPos.z = zBoundFar;

        player.position = limitPos;
    }
    #endregion
}
