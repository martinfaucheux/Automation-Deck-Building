using UnityEngine;
using DirectionEnum;

[RequireComponent(typeof(HexCollider))]
public class Belt : ResourceHolder
{
    public override void OnTick() { }
    public override bool IsAllowedToReceive() => true;
    public override bool IsAllowedToGive() => true;
    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder)
    {
        Direction? directionToOther = hexCollider.GetNeighborDirection(resourceHolder.hexCollider);
        if (directionToOther == null)
            return false;

        return (resourceHolder.direction.Opposite() == directionToOther);
    }
}
