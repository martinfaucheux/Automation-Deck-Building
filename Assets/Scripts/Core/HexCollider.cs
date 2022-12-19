using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCollider : MonoBehaviour
{
    public Vector2Int position;

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

    public void SyncPosition() => position = grid.GetHexPos(this.transform.position);
    public Vector3 GetWorldPos => grid.GetWorldPos(position);
}
