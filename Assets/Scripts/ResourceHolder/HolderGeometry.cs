using DirectionEnum;
using System.Collections.Generic;
using UnityEngine;

public class HolderGeometry : MonoBehaviour
{
    [SerializeField] ResourceHolder _resourceHolder;
    [field: SerializeField]
    public List<Direction> inputDirections { get; private set; }

    public bool IsAllowedToReceiveFrom(Direction direction)
    {
        Direction convertedDirection = direction.ToReferential(_resourceHolder.direction);
        return inputDirections.Contains(convertedDirection);
    }

    public bool IsAllowedToReceiveFrom(ResourceHolder otherHolder)
    {
        Direction? neighborDirection = _resourceHolder.hexCollider.GetNeighborDirection(otherHolder.hexCollider);
        if (neighborDirection != null)
        {
            return IsAllowedToReceiveFrom((Direction)neighborDirection);
        }
        return false;
    }
}
