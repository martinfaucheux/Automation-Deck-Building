using UnityEngine;

public class ResourceProducer : ResourceHolder
{
    [SerializeField] ResourceType _resourceType;
    [SerializeField] ResourceInstantiator _instantiator;


    // Generally, does this ResourceHolder have the right to deliver any resource
    public override bool IsAllowedToGive() => true;

    public override bool IsAllowedResource(ResourceObject resource) => false;

    public override void OnTick()
    {
        if (heldResource == null)
            _instantiator.InstantiateResource(_resourceType);
    }
}
