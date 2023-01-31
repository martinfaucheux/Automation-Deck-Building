using DirectionEnum;
using System.Collections.Generic;
using UnityEngine;
public abstract class ResourceHolder : MonoBehaviour
{
    [Tooltip("When building belt systems, indicate whether this belt has been treated")]
    public bool isDirty = true;

    // Marked as true if the resource should move at the end of processing.
    public bool willFlush { get; private set; } = false;

    public BeltSystem system;
    public Direction direction;
    private ResourceObject _futureResource;
    [field: SerializeField] public ResourceObject heldResource { get; protected set; }
    [SerializeField] protected ResourceHolderRenderer _renderer;
    public HexCollider hexCollider;
    public Vector2Int position { get => hexCollider.position; }

    public void Initialize(bool recalculate_system = true)
    {
        BeltManager.instance.AddHolder(this, recalculate_system);
    }

    public void Place()
    {
        InferDirection();
        Initialize(true);
        Render();
        foreach (ResourceHolder neighborHolder in GetNeighbors())
            neighborHolder.Render();
    }

    void OnDestroy()
    {
        if (heldResource != null)
            Destroy(heldResource.gameObject);
        BeltManager.instance.RemoveHolder(this);
    }

    // Method run on BeltManager tick before distributing the resource
    public abstract void OnTick();
    public abstract bool IsAllowedToReceive();
    public abstract bool IsAllowedToGive();
    public abstract bool IsAllowedToReceiveFrom(ResourceHolder resourceHolder);

    public void SetHeldResource(ResourceObject resource) => heldResource = resource;

    public void ResetWillFlush()
    {
        _futureResource = null;
        willFlush = false;
    }

    public List<ResourceHolder> GetNeighbors(bool feederOnly = false)
    {
        List<ResourceHolder> neighbors = new List<ResourceHolder>();
        foreach (Direction direction in EnumUtil.GetValues<Direction>())
        {
            if (direction == Direction.NONE)
                continue;

            Vector2Int checkPosition = position + direction.ToHexPosition();
            ResourceHolder resourceHolder = BeltManager.instance.GetHolderAtPos(checkPosition);
            if (
                resourceHolder != null
                && (!feederOnly || resourceHolder.GetTargetHolder() == this)
            )
                neighbors.Add(resourceHolder);
        }
        return neighbors;
    }

    public Vector2Int GetTargetPos() => position + direction.ToHexPosition();
    public ResourceHolder GetTargetHolder() => BeltManager.instance.GetHolderAtPos(GetTargetPos());
    public bool IsFeeder(ResourceHolder resourceHolder) => resourceHolder.GetTargetHolder() == this;

    // <summary>
    // Update willFlush i.e. whether this holder will be able to pass its resource
    // </summary>
    public void UpdateWillFlush()
    {
        willFlush = false;
        ResourceHolder targetHolder = GetTargetHolder();

        if (heldResource != null)
        {
            // check if target position has a valid ResourceHolder
            if (
                targetHolder != null
                && targetHolder.system == this.system
                && targetHolder.IsAllowedToReceiveFrom(this)
                && targetHolder._futureResource == null
            )
            {
                targetHolder._futureResource = heldResource;
                willFlush = true;
            }
            else
            {
                _futureResource = heldResource;
            }
        }
        // if no resource is held, no modification is made
    }

    public void Flush()
    {
        ResourceObject previousResource = heldResource;
        heldResource = _futureResource;

        if (willFlush)
        {
            // pass resource
            ResourceHolder targetHolder = GetTargetHolder();
            if (targetHolder == null)
                Debug.LogError("targetHolder should never be null", gameObject);
            else
            {
                if (previousResource != null)
                {
                    previousResource.Move(targetHolder.position);
                }
                else
                {
                    Debug.LogError("flushed resource should never be null", gameObject);
                }
            }
        }
    }

    private void CleanRotation()
    {
        int zRotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z);
        int rotationPace = 30;
        int finalZrotation = (zRotation + rotationPace / 2) / rotationPace * rotationPace;
        transform.rotation = Quaternion.Euler(0, 0, finalZrotation);
    }

    public void InferDirection()
    {
        CleanRotation();
        int angle = Mathf.RoundToInt(transform.rotation.eulerAngles.z);

        if (angle % 60 == 0)
        {
            int angleId = (angle / 60) % 6;
            direction = (Direction)angleId;
        }
        else
        {
            Debug.LogError("Invalid angle, must be a mulitple of 60");
        }
    }

    public virtual void Render() => _renderer?.Render();
}