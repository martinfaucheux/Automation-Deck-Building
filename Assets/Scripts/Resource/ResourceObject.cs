using UnityEngine;

[RequireComponent(typeof(HexCollider))]
public class ResourceObject : MonoBehaviour
{
    [SerializeField] HexCollider _hexCollider;
    [SerializeField] ResourceHolder _resourceHolder;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [field: SerializeField] public ResourceType resourceType { get; private set; }

    void Start()
    {
        ResourceHolder resourceHolder = BeltManager.instance.GetHolderAtPos(_hexCollider.position);
        if (resourceHolder != null)
        {
            resourceHolder.SetHeldResource(this);
        }
        else
        {
            Debug.LogError("No holder found", gameObject);
        }

    }

    public void SetHolder(ResourceHolder resourceHolder)
    {
        if (resourceHolder == null)
            Debug.LogError("resourceHolder should never be null", gameObject);

        _resourceHolder = resourceHolder;
    }

    public void Move(Vector2Int position)
    {
        Vector3 targetPosition = HexGrid.instance.GetWorldPos(position);
        targetPosition.z = transform.position.z;

        _hexCollider.SetPosition(position);
        LeanTween.move(
            gameObject,
            targetPosition,
            BeltManager.instance.moveDuration
        );
    }

    public void SetType(ResourceType resourceType)
    {
        this.resourceType = resourceType;
        _spriteRenderer.sprite = resourceType.sprite;
    }
}
