using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirectionEnum;

public class Belt : MonoBehaviour
{
    [Tooltip("When building belt systems, indicate whether this belt has been treated")]
    public bool isDirty = true;
    [SerializeField] Direction _direction;
    [SerializeField] HexCollider _collider;
    private Resource _heldResource;
    public Vector2Int position { get => _collider.position; }

    void Start()
    {
        BeltManager.instance.AddBelt(this);
    }

    void OnDestroy()
    {
        BeltManager.instance.RemoveBelt(this);
    }

    public List<(Direction, Belt)> GetNeighbors()
    {
        List<(Direction, Belt)> neighbors = new List<(Direction, Belt)>();
        foreach (Direction direction in EnumUtil.GetValues<Direction>())
        {
            if (direction == Direction.NONE)
                continue;

            Vector2Int checkPosition = position + direction.ToHexPosition();
            Belt belt = BeltManager.instance.GetBeltAtPos(checkPosition);
            if (belt != null)
                neighbors.Add((direction, belt));
        }
        return neighbors;
    }

    public Vector2Int GetTargetPos() => position + _direction.ToHexPosition();

    public bool IsConnected(Belt belt)
    {
        return (
            this.GetTargetPos() == belt.position || belt.GetTargetPos() == this.position
        );
    }

    public bool CanFlush()
    {
        // TODO: this is invalid if some resources are blocked but some not
        return BeltManager.instance.GetBeltAtPos(GetTargetPos()) != null;
    }

    public void Flush() => StartCoroutine(FlushCoroutine());

    public IEnumerator FlushCoroutine()
    {
        yield break;
    }
}
