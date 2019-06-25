using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_NormalStitcher : MonoBehaviour {

    public View_StaticProceduralMesh aft;
    public View_StaticProceduralMesh fwd;

	void Update () {
        if (Input.GetKeyDown(KeyCode.H))
            BlendNormals(aft, fwd);
	}

    Vector3 fwdNormal;
    Vector3 aftNormal;
    Vector3 avgNormal;

    public void BlendNormals(View_StaticProceduralMesh forwardOne, View_StaticProceduralMesh aftOne)
    {
        Debug.Log("Debug_NormalStitcher's BlendNormals called");
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
