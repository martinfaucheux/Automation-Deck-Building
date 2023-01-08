using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirectionEnum;

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
        position = grid.GetHexPos(this.transform.position);
    }

    public Vector3 GetWorldPos => grid.GetWorldPos(position);

    public void AlignOnGrid()
    {
        SyncPosition();
        Vector3 newPosition = HexGrid.instance.GetWorldPos(position);
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    public Direction? GetNeighborDirection(HexCollider otherCollider)
    {
        return DirectionUtil.GetDirectionFromPosition(otherCollider.position - this.position);
    }
}
