using DirectionEnum;
using UnityEngine;
public class ResourceSwitch : ResourceHolder
{
    [SerializeField] bool _switch = false;
    [SerializeField] SwitchPointerRenderer _pointerUpdater;

    public Direction allowedDirection
    {
        get => geometry.inputDirections[_switch ? 0 : 1];
    }

    void Start()
    {
        if (geometry.inputDirections.Count != 2)
            Debug.LogError("Invalid switch geometry");
    }

    public override bool IsAllowedToReceiveFromDynamic(ResourceHolder resourceHolder)
    {
        Direction neighborDirection = (Direction)hexCollider.GetNeighborDirection(resourceHolder.hexCollider);
        return neighborDirection.ToReferential(this.direction) == allowedDirection;
    }

    public override bool IsAllowedToGive() => true;

    public override void OnResourceChanged()
    {
        _switch = !_switch;
        _pointerUpdater?.UpdatePointer();
    }
}
