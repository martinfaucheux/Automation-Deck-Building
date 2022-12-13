using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;

    [Header("TileSettings")]
    public float outerSize = 1f;
    public float innerSize = 0f;
    public float height = 1f;
    public Material material;
    public Color color;
    public bool isFlatTopped;

    void Awake()
    {
        material.color = color;
    }

    private void ClearHex()
    {
        for (int i = transform.childCount; i > 0; --i)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }

    public void LayoutGrid()
    {
        ClearHex();

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex {x},{y}", typeof(HexRenderer));
                tile.transform.position = GetHexPosition(new Vector2Int(x, y));

                HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                hexRenderer.Initialize(outerSize, innerSize, height, material, color);
                hexRenderer.DrawMesh();
                tile.transform.SetParent(transform, true);
            }
        }
    }

    private Vector3 GetHexPosition(Vector2Int coordinates)
    {
        int column = coordinates.x;
        int row = coordinates.y;
        float width;
        float height;
        float xPosition;
        float yPosition;
        bool shouldOffset;
        float horizontalDistance;
        float verticalDistance;
        float offset;
        float size = outerSize;

        if (!isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = shouldOffset ? width / 2 : 0;
            xPosition = column * horizontalDistance + offset;
            yPosition = row * verticalDistance;
        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = shouldOffset ? height / 2 : 0;
            xPosition = column * horizontalDistance;
            yPosition = row * verticalDistance - offset;
        }


        return new Vector3(xPosition, -yPosition, 0);
    }
}
