using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct HexFace
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }

    public HexFace(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private List<HexFace> _faces;

    public Material material;
    public float innerSize;
    public float outerSize;
    public float height;
    // to be displayed in editor
    public Vector2Int coordinates;

    [SerializeField] TextMeshPro _labelText;

    // void Awake()
    // {
    //     SetMeshComponents();
    // }

    private void SetMeshComponents()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _mesh = new Mesh();
        _mesh.name = "Hex";
        _meshFilter.mesh = _mesh;
    }

    public void Initialize(
        float outerSize,
        float innerSize,
        float height,
        Vector2Int coordinates
    )
    {
        if (_meshRenderer != null || _meshFilter == null)
            SetMeshComponents();

        this.outerSize = outerSize;
        this.innerSize = innerSize;
        this.height = height;
        this.coordinates = coordinates;
    }

    public void DrawMesh()
    {
        _meshRenderer.material = material;
        DrawFaces();
        CombineFaces();

        _labelText.text = $"{coordinates.x},{coordinates.y}";
        _labelText.color = material.color;
    }

    private void DrawFaces()
    {
        _faces = new List<HexFace>();

        // Top faces
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }
    }

    private HexFace CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new List<Vector3> { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>(){
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1),
        };

        if (reverse)
            vertices.Reverse();

        return new HexFace(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float size, float height, int index)
    {
        float angleDegree = 60 * index;
        float angleRad = Mathf.PI / 180f * angleDegree;
        return new Vector3(
            size * Mathf.Cos(angleRad),
            size * Mathf.Sin(angleRad),
            height
        );
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < _faces.Count; i++)
        {
            // Add the vertices
            vertices.AddRange(_faces[i].vertices);
            uvs.AddRange(_faces[i].uvs);

            // Offset the triangles
            int offset = (4 * i);
            foreach (int triangle in _faces[i].triangles)
            {
                triangles.Add(triangle + offset);
            }
        }

        Debug.Assert((_mesh != null), "Mesh is not set");

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.RecalculateNormals();
    }

}
