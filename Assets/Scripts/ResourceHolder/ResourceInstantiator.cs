using UnityEngine;

public class ResourceInstantiator : MonoBehaviour
{
    [SerializeField] GameObject _resourcePrefab;
    private ResourceHolder _resourceHolder;
    private HexLayer _hexLayer;

    private void Start()
    {
        _resourceHolder = GetComponent<ResourceHolder>();
        _hexLayer = _resourcePrefab.GetComponent<HexCollider>().layer;
    }

    public GameObject InstantiateResource(ResourceType resourceType)
    {
        Vector3 position = this.transform.position;
        position.z = HexLayerUtil.GetHeight(_hexLayer);

        GameObject resourceGameObject = Instantiate(_resourcePrefab, position, Quaternion.identity);
        ResourceObject resourceComponent = resourceGameObject.GetComponent<ResourceObject>();
        resourceComponent.SetType(resourceType);
        _resourceHolder.SetHeldResource(resourceComponent);
        return resourceGameObject;
    }

}
