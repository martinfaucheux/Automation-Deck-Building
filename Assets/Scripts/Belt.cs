using UnityEngine;
using DirectionEnum;

[RequireComponent(typeof(HexCollider))]
public class Belt : ResourceHolder
{
    public override void OnTick() { }
    public override bool IsAllowedToReceive() => true;
    public override bool IsAllowedToGive() => true;
    public override bool IsAllowedToReceive(ResourceHolder resourceHolder)
    {
        Direction? otherDirection = hexCollider.GetNeighborDirection(resourceHolder.hexCollider);
        if (otherDirection == null)
            return false;

        return (this._direction.Opposite() == otherDirection);
    }
}
