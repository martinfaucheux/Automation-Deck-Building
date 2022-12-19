using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirectionEnum;
using System.Linq;

public class Belt : MonoBehaviour
{
    [Tooltip("When building belt systems, indicate whether this belt has been treated")]
    public bool isDirty = true;

    // Marked as true if the resource should move at the end of processing.
    public bool willFlush { get; private set; } = false;

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
    public Belt GetTargetBelt() => BeltManager.instance.GetBeltAtPos(GetTargetPos());

    public void ResetWillFlush() => willFlush = false;

    public void UpdateWillFlush()
    {
        // check if target position has a belt
        Belt targetBelt = GetTargetBelt();
        if (targetBelt != null)
        {
            willFlush = !(
                GetNeighbors()
                // get only belts pointing toward target
                .Where(tuple => tuple.Item1.Opposite() == tuple.Item2._direction)
                // remove direction
                .Select(tuple => tuple.Item2)
                // filter only the one that will flush a resource
                .Where(belt => belt.willFlush && belt._heldResource != null)
                .Any()
            );
        }
        else
        {
            willFlush = false;
        }
    }

    public void Flush() => StartCoroutine(FlushCoroutine());

    private IEnumerator FlushCoroutine()
    {
        Resource initialHeldItem = _heldResource;

        // wait end of frame to make sure each initial resource has been cached
        yield return new WaitForEndOfFrame();

        // pass resource
        GetTargetBelt()._heldResource = initialHeldItem;

        // TODO: lean tween move
    }
}
