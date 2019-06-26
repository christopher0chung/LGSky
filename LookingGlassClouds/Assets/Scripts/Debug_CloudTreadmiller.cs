using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_CloudTreadmiller : MonoBehaviour {

    public Model_Game gameModel;

    public View_StaticProceduralMesh startAft;
    public View_StaticProceduralMesh startFwd;

    private Vector3 initialOffset = Vector3.zero;
    private Vector3 offsetInterval = Vector3.forward * 50;
    private Vector3 currentMostFwd;

    void Start()
    {
        currentMostFwd = initialOffset + offsetInterval;
        Debug.Log(currentMostFwd);
        BlendNormals(startFwd, startAft);
    }

    void Update()
    {
        Vector3 toMove = -Vector3.forward * gameModel.worldSpeed_fwd * Time.deltaTime;

        startAft.transform.position += toMove;
        startFwd.transform.position += toMove;

        if (startAft.transform.position.z <= -50)
        {
            startAft.transform.position += Vector3.forward * 100;
            currentMostFwd += offsetInterval;
            startAft.FlashVerts(currentMostFwd);
            BlendNormals(startAft, startFwd);
        }

        if (startFwd.transform.position.z <= -50)
        {
            startFwd.transform.position += Vector3.forward * 100;
            currentMostFwd += offsetInterval;
            startFwd.FlashVerts(currentMostFwd);
            BlendNormals(startFwd, startAft);
        }
    }

    Vector3 fwdNormal;
    Vector3 aftNormal;
    Vector3 avgNormal;

    public void BlendNormals(View_StaticProceduralMesh forwardOne, View_StaticProceduralMesh aftOne)
    {
        //Debug.Log("Debug_NormalStitcher's BlendNormals called");
        for (int i = 0; i <= forwardOne.chunkVertexDimensions; i++)
        {
            int forwardEdgeOffset = forwardOne.chunkVertexDimensions * (forwardOne.chunkVertexDimensions + 1);
            fwdNormal = forwardOne._myNormals[i + forwardEdgeOffset];
            aftNormal = aftOne._myNormals[i];
            avgNormal = (fwdNormal + aftNormal) / 2;

            forwardOne._myNormals[i + forwardEdgeOffset] = avgNormal;
            aftOne._myNormals[i] = avgNormal;

            forwardOne.ExternalStitchApply();
            aftOne.ExternalStitchApply();
        }
    }
}
