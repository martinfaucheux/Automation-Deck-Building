using DirectionEnum;
using System.Collections.Generic;
using UnityEngine;

public class HolderGeometry : MonoBehaviour
{
    [SerializeField] ResourceHolder _resourceHolder;
    [SerializeField] List<Direction> _inputDirections;

    public bool IsAllowedToReceiveFrom(ResourceHolder otherHolder)
    {
        Direction? neighborDirection = _resourceHolder.hexCollider.GetNeighborDirection(otherHolder.hexCollider);
        if (neighborDirection != null)
        {
            Direction convertedDirection = ToReferential((Direction)neighborDirection);
            return _inputDirections.Contains(convertedDirection);
        }
        return false;
    }

    private Direction ToReferential(Direction direction)
    {
        return direction.Rotate(-(int)_resourceHolder.direction);
    }

}
