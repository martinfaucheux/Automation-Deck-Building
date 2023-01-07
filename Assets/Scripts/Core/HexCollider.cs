using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCollider : MonoBehaviour
{
    public Vector2Int position;
    public HexLayer layer;

    public HexGrid grid { get => HexGrid.instance; }

    void Awake()
    {
        SyncPosition();
        grid.AddCollider(this);
    }

    void OnDestroy()
    {
        grid?.RemoveCollider(this);
    }

    public void SyncPosition()
    {
        // TODO: this is invalid, most likely because of GetHexPos
        position = grid.GetHexPos(this.transform.position);
        Debug.Log("position: " + position.ToString(), gameObject);
    }
    public Vector3 GetWorldPos => grid.GetWorldPos(position);

    public void AlignOnGrid()
    {
        SyncPosition();
        transform.position = HexGrid.instance.GetWorldPos(position);
    }
}
