using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class Debug_TrailTest : MonoBehaviour {

    public Model_Game gameModel;

    private LineRenderer line;
    private Transform tr;
    private Vector3[] positions;
    private Vector3[] directions;
    private int i;

    private int currentNumberOfPoints;
    private bool allPointsAdded;

    private Vector3 tempVec;

    public int numberOfPoints = 10;
    public float spread = .2f;

    public float spreadOverFramesFactor;

    void Start()
    {
        tr = transform;
        line = GetComponent<LineRenderer>();

        positions = new Vector3[numberOfPoints];
        directions = new Vector3[numberOfPoints];

        line.positionCount = currentNumberOfPoints;

        for (i = 0; i < currentNumberOfPoints; i++)
        {
            tempVec = diffusionAndWorldTrail();
            directions[i] = tempVec;
            positions[i] = tr.position;
            line.SetPosition(i, positions[i]);
        }
    }

    void Update()
    {
        // Add points until the target number is reached.
        if (!allPointsAdded)
        {
            currentNumberOfPoints++;
            line.positionCount = currentNumberOfPoints;
            tempVec = diffusionAndWorldTrail();
            directions[0] = tempVec;
            positions[0] = tr.position;
            line.SetPosition(0, positions[0]);
        }

        if (!allPointsAdded && (currentNumberOfPoints == numberOfPoints))
        {
            allPointsAdded = true;
        }

        // Make each point in the line take the position and direction of the one before it (effectively removing the last point from the line and adding a new one at transform position).
        for (i = currentNumberOfPoints - 1; i > 0; i--)
        {
            tempVec = positions[i - 1];
            positions[i] = tempVec;
            tempVec = directions[i - 1];
            directions[i] = tempVec;
        }
        tempVec = diffusionAndWorldTrail();
        directions[0] = tempVec; // Remember and give 0th point a direction for when it gets pulled up the chain in the next line update.


        // Update the line...
        for (i = 1; i < currentNumberOfPoints; i++)
        {
            tempVec = positions[i];
            directions[i].x *= spreadOverFramesFactor;
            directions[i].y *= spreadOverFramesFactor;
            tempVec += directions[i] * Time.deltaTime;
            positions[i] = tempVec;

            line.SetPosition(i, positions[i]);
        }
        positions[0] = tr.position; // 0th point is a special case, always follows the transform directly.
        line.SetPosition(0, tr.position);

    }

    Vector3 diffusionAndWorldTrail()
    {
        Vector3 smokeVec = Random.insideUnitSphere;
        smokeVec.z = 0;
		smokeVec *= spread;
        smokeVec.z = -gameModel.worldSpeed_fwd;
		return smokeVec;
    }
}
