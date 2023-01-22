using UnityEngine;

public class ResourceProducer : ResourceHolder
{
    [SerializeField] GameObject _resourcePrefab;
    [SerializeField] float _initialResourceZ = -0.2f;
    [SerializeField] ResourceType _resourceType;

    // Generally, does this ResourceHolder have the right to receive any resource
    public override bool IsAllowedToReceive() => false;

    // Generally, does this ResourceHolder have the right to deliver any resource
    public override bool IsAllowedToGive() => true;

    public override bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder) => false;

    public override void OnTick()
    {
        if (heldResource == null)
        {
            Vector3 position = this.transform.position;
            position.z = _initialResourceZ;

            GameObject resourceGameObject = Instantiate(_resourcePrefab, position, Quaternion.identity);
            ResourceObject resourceComponent = resourceGameObject.GetComponent<ResourceObject>();
            resourceComponent.SetType(_resourceType);
            heldResource = resourceComponent;
        }
    }
}
