using UnityEngine;

public class BuildingInstanciator : SingletonBase<BuildingInstanciator>
{

    [SerializeField] GameObject _ghostGameObject;
    private GameObject _buildingPrefab;

    private ResourceHolder _resourceHolder;
    private Vector2 _lastPosition;
    private float snapAnimDuration = 0.03f;
    private int _ltAnimId;

    public void SetBuildingPrefab(GameObject buildingPrefab)
    {
        _buildingPrefab = buildingPrefab;
        CreateGhostObject();
    }

    private static readonly int BUTTON_INDEX = 1; // right button

    public void CreateGhostObject()
    {
        _ghostGameObject = Instantiate(_buildingPrefab);

        _resourceHolder = _ghostGameObject.GetComponent<ResourceHolder>();
        if (_resourceHolder == null)
            Debug.LogError("Ghost GameObject should have a ResourceHolder component attached", _buildingPrefab);
    }

    public void PlaceGameObject()
    {
        if (CanPlace())
        {
            _resourceHolder.Initialize();
            _ghostGameObject = null;
            _resourceHolder = null;

            CreateGhostObject();
        }
    }

    private bool CanPlace()
    {
        return HexGrid.instance.GetColliderAtPosition(
            _resourceHolder.hexCollider.position, _resourceHolder.hexCollider.layer
        ) == null;
    }

    public void CancelSelection()
    {
        Destroy(_ghostGameObject);
        _ghostGameObject = null;
        _buildingPrefab = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(BUTTON_INDEX))
            CancelSelection();

        if (_ghostGameObject != null)
        {
            SnapGhostToGrid();
            Rerender();
        }
    }

    private void SnapGhostToGrid()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 0;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2Int hexPosition = HexGrid.instance.GetHexPos(worldPosition);
        _resourceHolder.hexCollider.position = hexPosition;

        Vector3 snappedWorldPosition = _resourceHolder.hexCollider.GetSnappedPosition();

        LeanTween.cancel(_ltAnimId);
        LTDescr ltDescr = LeanTween.move(_ghostGameObject, snappedWorldPosition, snapAnimDuration);
        _ltAnimId = ltDescr.id;
    }

    private void Rerender()
    {
        // rerender only if position has changed
        if (_resourceHolder.hexCollider.position != _lastPosition)
        {
            _lastPosition = _resourceHolder.hexCollider.position;
            _resourceHolder.Render();
        }
    }
}
