using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class CrawlingProceduralMesh : MonoBehaviour
{
    Mesh _myMesh;
    MeshFilter _myMeshFilter;
    MeshRenderer _myMeshRenderer;

    public Vector3[] _myVerts = new Vector3[0];
    public int[] _myTris = new int[0];
    public Vector3[] _myNormals = new Vector3[0];
    public Vector2[] _myUVs = new Vector2[0];

    public Vector2 offset;

    [Header("Tuning Values")]
    public int chunkVertexDimensions;
    public float chunkUnitDimension;
    public float noiseMagnitudeScalarOctave1;
    public float noiseMagnitudeScalarOctave2;
    public float secondOctaveNoiseScalar;
    public Vector2 startingPositionalOffset;
    public float moveSpeed;
    public float noiseSpeed;

    [Header("Public References")]
    public Material matToApplyToGeneratedMesh;

    void Start()
    {
        GetRefs();
        InitializeData();
        CalculateMesh();
        ApplyMesh();
    }

    bool updateEveryOtherFrame;

    private void Update()
    {
        offset.y = Time.time * moveSpeed;

        updateEveryOtherFrame = !updateEveryOtherFrame;

        if (updateEveryOtherFrame)
        {
            VertCalc();

            ApplyMesh();
        }
    }

    #region Internal Functions
    private void GetRefs()
    {
        _myMeshFilter = GetComponent<MeshFilter>();
        _myMeshRenderer = GetComponent<MeshRenderer>();
    }

    private void InitializeData()
    {
        int vertDim = chunkVertexDimensions + 1;
        _myVerts = new Vector3[vertDim * vertDim];
        _myNormals = new Vector3[_myVerts.Length];
        _myUVs = new Vector2[_myVerts.Length];

        _myMesh = new Mesh();

        _myMeshRenderer.material = matToApplyToGeneratedMesh;
    }

    private void CalculateMesh()
    {
        VertCalc();

        List<int> tris = new List<int>();

        for (int z = 0; z < chunkVertexDimensions; z++)
        {
            for (int x = 0; x < chunkVertexDimensions; x++)
            {
                tris.Add(x + z * (chunkVertexDimensions + 1));
                tris.Add((x + 1) + (z + 1) * (chunkVertexDimensions + 1));
                tris.Add((x + 1) + z * (chunkVertexDimensions + 1));

                tris.Add(x + z * (chunkVertexDimensions + 1));
                tris.Add(x + (z + 1) * (chunkVertexDimensions + 1));
                tris.Add((x + 1) + (z + 1) * (chunkVertexDimensions + 1));
            }
        }

        _myTris = new int[tris.Count];

        _myTris = tris.ToArray();
    }

    private void VertCalc()
    {
        for (int i = 0; i <= chunkVertexDimensions; i++)
        {
            for (int j = 0; j <= chunkVertexDimensions; j++)
            {
                _myVerts[j + i * (chunkVertexDimensions + 1)] =
                    new Vector3(chunkUnitDimension * j / (float)chunkVertexDimensions,
                    noiseMagnitudeScalarOctave1 *
                        Perlin.Noise(startingPositionalOffset.x + offset.x + transform.position.x + chunkUnitDimension * j / (float)chunkVertexDimensions,
                        startingPositionalOffset.y + offset.y + transform.position.z + chunkUnitDimension * i / (float)chunkVertexDimensions,
                        Time.time * noiseSpeed)
                        +
                        noiseMagnitudeScalarOctave2 *
                        Perlin.Noise(startingPositionalOffset.x + offset.x + transform.position.x + chunkUnitDimension * j / ((float)chunkVertexDimensions * secondOctaveNoiseScalar),
                        startingPositionalOffset.y + offset.y + transform.position.z + chunkUnitDimension * i / ((float)chunkVertexDimensions * secondOctaveNoiseScalar),
                        Time.time * noiseSpeed),
                    chunkUnitDimension * i / (float)chunkVertexDimensions);
                _myUVs[j + i * (chunkVertexDimensions + 1)] =
                    new Vector2(j / (float)chunkVertexDimensions,
                    i / (float)chunkVertexDimensions);
            }
        }
    }

    private void ApplyMesh()
    {
        _myMesh.vertices = _myVerts;
        _myMesh.triangles = _myTris;
        _myMesh.uv = _myUVs;
        _myMesh.RecalculateNormals();

        _myMeshFilter.mesh = _myMesh;
    }
    #endregion

    Vector3 consideredNormal;
    Vector3 readNormal;

    #region Blend Functions
    public void BlendFwdNormals(Vector3[] normalsFromMeshFromRelFwd)
    {
        for (int i = 0; i <= chunkVertexDimensions; i++)
        {
            consideredNormal = _myNormals[i + chunkVertexDimensions * (chunkVertexDimensions + 1)];
            readNormal = normalsFromMeshFromRelFwd[i];

            _myNormals[i + chunkVertexDimensions * (chunkVertexDimensions + 1)] = (consideredNormal + readNormal) / 2;
        }
    }

    public void BlendBackNormals(Vector3[] normalsFromMeshFromRelBack)
    {
        for (int i = 0; i <= chunkVertexDimensions; i++)
        {
            consideredNormal = _myNormals[i];
            readNormal = normalsFromMeshFromRelBack[i + chunkVertexDimensions * (chunkVertexDimensions + 1)];

            _myNormals[i + chunkVertexDimensions * (chunkVertexDimensions + 1)] = (consideredNormal + readNormal) / 2;
        }
    }

    public void BlendRightNormals(Vector3[] normalsFromMeshFromRelRight)
    {
        for (int i = 0; i <= chunkVertexDimensions; i++)
        {
            consideredNormal = _myNormals[i * (chunkVertexDimensions + 1) + chunkVertexDimensions * (chunkVertexDimensions + 1)];
            readNormal = normalsFromMeshFromRelRight[i * (chunkVertexDimensions + 1)];

            _myNormals[i + chunkVertexDimensions * (chunkVertexDimensions + 1)] = (consideredNormal + readNormal) / 2;
        }
    }

    public void BlendLeftNormals(Vector3[] normalsFromMeshFromRelLeft)
    {
        for (int i = 0; i <= chunkVertexDimensions; i++)
        {
            consideredNormal = _myNormals[i * (chunkVertexDimensions + 1)];
            readNormal = normalsFromMeshFromRelLeft[i * (chunkVertexDimensions + 1) + chunkVertexDimensions * (chunkVertexDimensions + 1)];

            _myNormals[i + chunkVertexDimensions * (chunkVertexDimensions + 1)] = (consideredNormal + readNormal) / 2;
        }
    }
    #endregion
}