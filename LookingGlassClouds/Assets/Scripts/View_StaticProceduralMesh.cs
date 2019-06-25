using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class View_StaticProceduralMesh : MonoBehaviour
{
    Mesh _myMesh;
    MeshFilter _myMeshFilter;
    MeshRenderer _myMeshRenderer;

    public Vector3[] _myVerts = new Vector3[0];
    public int[] _myTris = new int[0];
    public Vector3[] _myNormals = new Vector3[0];
    public Vector2[] _myUVs = new Vector2[0];

    [Header("Tuning Values")]
    public int chunkVertexDimensions;
    public float chunkUnitDimension;
    public float noiseMagnitudeScalarOctave1;
    public float noiseSampleScalarOctave1;
    public Vector2 offsetPos;

    public bool crawl;

    [Header("Public References")]
    public Material matToApplyToGeneratedMesh;

    void Awake()
    {
        GetRefs();
        InitializeData();
        CalculateMesh();
        LoadMesh();
    }

    void Start()
    {
        ApplyMesh();
    }

    bool updateEveryOtherFrame;

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
                                    Perlin.Noise(
                                        noiseSampleScalarOctave1 * (offsetPos.x + chunkUnitDimension * j / (float)chunkVertexDimensions),
                                        noiseSampleScalarOctave1 * (offsetPos.y + chunkUnitDimension * i / (float)chunkVertexDimensions)),
                                chunkUnitDimension * i / (float)chunkVertexDimensions);

                _myUVs[j + i * (chunkVertexDimensions + 1)] =
                    new Vector2(j / (float)chunkVertexDimensions,
                    i / (float)chunkVertexDimensions);
            }
        }
    }

    private void LoadMesh()
    {
        _myMesh.vertices = _myVerts;
        _myMesh.triangles = _myTris;
        _myMesh.uv = _myUVs;
        _myMesh.RecalculateNormals();
        _myNormals = _myMesh.normals;
    }

    private void ApplyMesh()
    {
        _myMeshFilter.mesh = _myMesh;
    }
    #endregion

    public void FlashVerts(Vector3 newOffset)
    {
        offsetPos.x = newOffset.x;
        offsetPos.y = newOffset.z;
        VertCalc();
    }

    public void ExternalStitchApply()
    {
        _myMesh.vertices = _myVerts;
        _myMesh.triangles = _myTris;
        _myMesh.uv = _myUVs;
        _myMesh.normals = _myNormals;

        _myMeshFilter.mesh = _myMesh;
    }

    Vector3 internalNormal;
    Vector3 externalNormal;
}