using UnityEngine;

[RequireComponent(typeof(HexCollider))]
public class Belt : ResourceHolder
{
    public override void OnTick() { }
    public override bool IsAllowedToReceive() => true;
    public override bool IsAllowedToGive() => true;
    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder) => true;
}
