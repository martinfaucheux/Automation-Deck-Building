using UnityEngine;
using UnityEngine.SceneManagement;
public class ResourceInstantiator : MonoBehaviour
{
    [SerializeField] GameObject _resourcePrefab;
    private ResourceHolder _resourceHolder;
    private HexLayer _hexLayer;

    private static string CONTAINER_NAME = "ResourceContainer";
    private static Transform _resourceContainer;

    private void Start()
    {
        _resourceHolder = GetComponent<ResourceHolder>();
        _hexLayer = _resourcePrefab.GetComponent<HexCollider>().layer;

        if (_resourceContainer == null)
            SetResourceContainer();

    }

    public GameObject InstantiateResource(ResourceType resourceType)
    {
        Vector3 position = this.transform.position;
        position.z = HexLayerUtil.GetHeight(_hexLayer);

        GameObject resourceGameObject = Instantiate(_resourcePrefab, position, Quaternion.identity, _resourceContainer);
        ResourceObject resourceComponent = resourceGameObject.GetComponent<ResourceObject>();
        resourceComponent.SetType(resourceType);
        _resourceHolder.SetHeldResource(resourceComponent);
        return resourceGameObject;
    }

    private void SetResourceContainer()
    {
        foreach (GameObject _gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (_gameObject.name == CONTAINER_NAME)
            {
                _resourceContainer = _gameObject.transform;
                return;
            }
        }
        Debug.LogError("Cannot find resource container", gameObject);
    }

}
