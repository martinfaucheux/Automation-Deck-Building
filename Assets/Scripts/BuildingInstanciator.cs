using DirectionEnum;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInstanciator : SingletonBase<BuildingInstanciator>
{

    [SerializeField] GameObject _ghostGameObject;
    private GameObject _buildingPrefab;

    private ResourceHolder _resourceHolder;
    private Vector2 _lastPosition;
    private float snapAnimDuration = 0.03f;
    private int _ltSnapAnimId;
    private bool _isDeleteMode = false;

    public void SetBuildingPrefab(GameObject buildingPrefab)
    {
        _isDeleteMode = false;
        _buildingPrefab = buildingPrefab;
        CreateGhostObject();
    }

    public void SetDeleteMode()
    {
        _buildingPrefab = null;
        _ghostGameObject = null;
        _isDeleteMode = true;
    }

    private static readonly int PLACE_BUTTON = 0; // left button
    private static readonly int CANCEL_BUTTON = 1; // right button

    public void CreateGhostObject()
    {
        _ghostGameObject = Instantiate(_buildingPrefab);

        _resourceHolder = _ghostGameObject.GetComponent<ResourceHolder>();
        if (_resourceHolder == null)
            Debug.LogError("Ghost GameObject should have a ResourceHolder component attached", _buildingPrefab);

        SnapGhostToGrid(instant: true);
    }

    public void PlaceGameObject()
    {
        Vector2Int mouseHexPosition = GetMouseHexPos();
        if (_ghostGameObject != null && CanPlace(mouseHexPosition))
        {
            _resourceHolder.hexCollider.SetPosition(mouseHexPosition);
            _resourceHolder.Place();

            _ghostGameObject = null;
            _resourceHolder = null;

            CreateGhostObject();
        }
    }

    public void DeleteGameObject()
    {
        foreach (HexLayer layer in EnumUtil.GetValues<HexLayer>())
        {
            HexCollider collider = HexGrid.instance.GetColliderAtPosition(GetMouseHexPos(), layer);
            if (collider != null)
                Destroy(collider.gameObject);
        }
    }
    private bool CanPlace(Vector2Int position)
    {
        return HexGrid.instance.GetColliderAtPosition(
           position, _resourceHolder.hexCollider.layer
        ) == null;
    }

    public void CancelSelection()
    {
        if (_ghostGameObject != null)
        {
            Destroy(_ghostGameObject);
            _ghostGameObject = null;
            _buildingPrefab = null;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(PLACE_BUTTON) && IsNotClickingUI())
        {
            if (_isDeleteMode)
                DeleteGameObject();
            else
                PlaceGameObject();
        }

        else if (Input.GetMouseButtonDown(CANCEL_BUTTON) && IsNotClickingUI())
            CancelSelection();

        else if (Input.GetKeyDown(KeyCode.R))
            Rotate();

        if (_ghostGameObject != null)
        {
            SnapGhostToGrid();
            Rerender();
        }
    }

    private void SnapGhostToGrid(bool instant = false)
    {
        Vector2Int mouseHexPos = GetMouseHexPos();
        Vector3 snappedWorldPosition = HexGrid.instance.GetWorldPos(
            mouseHexPos, _resourceHolder.hexCollider.layer
        );

        if (instant)
        {
            _ghostGameObject.transform.position = snappedWorldPosition;
        }
        else
        {
            LeanTween.cancel(_ltSnapAnimId);
            LTDescr ltDescr = LeanTween.move(_ghostGameObject, snappedWorldPosition, snapAnimDuration);
            _ltSnapAnimId = ltDescr.id;
        }
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

    private bool IsNotClickingUI()
    {
        return (

            EventSystem.current != null &&
            !EventSystem.current.IsPointerOverGameObject()
        );
    }

    private void Rotate()
    {
        if (_ghostGameObject != null)
        {
            _resourceHolder.transform.Rotate(new Vector3(0, 0, 60));
            _resourceHolder.InferDirection();
        }
    }

    private Vector2Int GetMouseHexPos()
    {
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = 0;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return HexGrid.instance.GetHexPos(worldPosition);
    }
}
