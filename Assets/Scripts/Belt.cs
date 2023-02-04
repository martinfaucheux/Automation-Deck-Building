using UnityEngine;

[RequireComponent(typeof(HexCollider))]
public class Belt : ResourceHolder
{
    public override bool IsAllowedToGive() => true;
}
