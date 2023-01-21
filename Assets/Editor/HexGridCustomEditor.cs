
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGrid))]
public class HexGridCustomEditor : Editor
{
    private HexGrid t;

    private void OnEnable()
    {
        t = target as HexGrid;
    }

    public override void OnInspectorGUI()
    {
        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Render Grid"))
        {
            ClearHex();
            LayoutGrid();
            RecalculateOrigin();
            EditorUtility.SetDirty(t.gameObject);
        }

        if (GUILayout.Button("Align all colliders"))
            AlignCollidersOnGrid();
    }

    private void ClearHex()
    {
        for (int i = t.hexGridMeshContainer.childCount; i > 0; --i)
            DestroyImmediate(t.hexGridMeshContainer.GetChild(0).gameObject);
    }

    public void LayoutGrid()
    {
        for (int y = 0; y < t.gridSize.y; y++)
        {
            for (int x = 0; x < t.gridSize.x; x++)
            {
                Vector2Int coordinates;
                if (x % 2 == 0)
                    coordinates = new Vector2Int(x, 2 * y);
                else
                    coordinates = new Vector2Int(x, 2 * y + 1);

                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(t.hexPrefab, t.hexGridMeshContainer);
                tile.name = $"Hex {coordinates.x},{coordinates.y}";

                tile.transform.position = HexGrid.instance.GetWorldPos(coordinates);

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.Initialize(
                    t.unitSize,
                    t.unitSize - t.lineThickness,
                    t.transform.position.z,
                    coordinates
                );
                hexRenderer.DrawMesh();
            }
        }
    }

    private void RecalculateOrigin()
    {
        t.origin = -0.5f * HexGrid.instance.worldSize + 0.5f * HexGrid.instance.hexSize;
    }

    private void AlignCollidersOnGrid()
    {
        foreach (HexCollider collider in FindObjectsOfType<HexCollider>())
            collider.AlignOnGrid();
    }
}