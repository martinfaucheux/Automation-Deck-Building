using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DirectionEnum;
using System.Linq;
public abstract class ResourceHolder : MonoBehaviour
{
    [Tooltip("When building belt systems, indicate whether this belt has been treated")]
    public bool isDirty = true;

    // Marked as true if the resource should move at the end of processing.
    public bool willFlush { get; private set; } = false;

    [SerializeField] protected Direction _direction;
    [SerializeField] protected Resource _heldResource;
    private Resource _cachedHeldResource;
    public HexCollider hexCollider;
    public Vector2Int position { get => hexCollider.position; }

    void Start()
    {
        BeltManager.instance.AddHolder(this);
    }

    void OnDestroy()
    {
        BeltManager.instance.RemoveHolder(this);
    }

    // Method run on BeltManager tick before distributing the resource
    public abstract void OnTick();

    public abstract bool IsAllowedToReceive();
    public abstract bool IsAllowedToGive();
    public abstract bool IsAllowedToReceive(ResourceHolder resourceHolder);

    public void SetHeldResource(Resource resource) => _heldResource = resource;


    public void ResetWillFlush()
    {
        _cachedHeldResource = _heldResource;
        _heldResource = null;
        willFlush = false;
    }

    public List<ResourceHolder> GetNeighbors()
    {
        List<ResourceHolder> neighbors = new List<ResourceHolder>();
        foreach (Direction direction in EnumUtil.GetValues<Direction>())
        {
            if (direction == Direction.NONE)
                continue;

            Vector2Int checkPosition = position + direction.ToHexPosition();
            ResourceHolder resourceHolder = BeltManager.instance.GetHolderAtPos(checkPosition);
            if (resourceHolder != null)
                neighbors.Add(resourceHolder);
        }
        return neighbors;
    }

    public Vector2Int GetTargetPos() => position + _direction.ToHexPosition();
    public ResourceHolder GetTargetHolder() => BeltManager.instance.GetHolderAtPos(GetTargetPos());

    // <summary>
    // Update willFlush i.e. whether this holder will be able to pass its resource
    // </summary>
    public void UpdateWillFlush()
    {
        if (_cachedHeldResource == null)
        {
            willFlush = false;
            return;
        }

        // check if target position has a valid ResourceHolder
        ResourceHolder targetHolder = GetTargetHolder();
        if (targetHolder != null)
        {

            // if target already has a resource and is stuck, this can't flush
            if (targetHolder._cachedHeldResource != null && !targetHolder.willFlush)
            {
                willFlush = false;
                return;
            }

            // check if another holder already chose to flush to target
            willFlush = !(
                targetHolder.GetNeighbors()
                .Where(neighborOfTargetHolder => (
                    // check only neighbor of targets that have the right to flush to target
                    targetHolder.IsAllowedToReceive(neighborOfTargetHolder)
                    // filter only the one that will flush a resource
                    && neighborOfTargetHolder.willFlush
                    && neighborOfTargetHolder._heldResource != null
                ))
                .Any()
            );
        }
        else
        {
            willFlush = false;
        }
    }

    public void Flush()
    {
        if (!willFlush)
        {
            // if _cachedResource is not null, then this might have receive a new resource
            // in the meantime
            if (_cachedHeldResource != null)
                SetHeldResource(_cachedHeldResource);
            return;
        }
        // pass resource
        ResourceHolder targetHolder = GetTargetHolder();
        if (targetHolder == null)
            Debug.LogError("targetHolder should never be null", gameObject);

        if (_cachedHeldResource != null)
        {
            _cachedHeldResource.SetHolder(targetHolder);
            targetHolder.SetHeldResource(_cachedHeldResource);
            _cachedHeldResource.Move(targetHolder.position);
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
            _direction = (Direction)angleId;
        }
        else
        {
            Debug.LogError("Invalid angle, must be a mulitple of 60");
        }
    }
}