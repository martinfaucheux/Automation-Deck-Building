using System.Collections.Generic;
using UnityEngine;

public class BeltSystem : MonoBehaviour
{
    [SerializeField] List<ResourceHolder> _resourceHolders = new List<ResourceHolder>();

    void Start()
    {
        BeltManager.instance.onTick += OnTick;
    }

    void OnDestroy()
    {
        BeltManager.instance.onTick -= OnTick;
    }


    public void AddHolder(ResourceHolder resourceHolder, bool insertAtTail = false)
    {
        int insertIndex = insertAtTail ? _resourceHolders.Count : 0;
        _resourceHolders.Insert(insertIndex, resourceHolder);
        resourceHolder.system = this;
        resourceHolder.isDirty = false;

        if (resourceHolder.IsAllowedToGive())
        {
            ResourceHolder targetHolder = resourceHolder.GetTargetHolder();
            if (
                targetHolder != null
                && targetHolder.IsAllowedToReceiveFrom(resourceHolder)
                && targetHolder.isDirty
            )
            {
                AddHolder(targetHolder, false);
            }
        }

        if (resourceHolder.IsAllowedToReceive())
        {
            // TODO: use feederOnly
            foreach (ResourceHolder neighborHolder in resourceHolder.GetNeighbors())
            {
                if (
                    (neighborHolder.GetTargetHolder() == resourceHolder)
                    // TODO: IsAllowedToReceiveFrom should be used only to check resource compatibility at runtime
                    // need to implement a new abstract method if we want to check building geometry compatibility
                    && resourceHolder.IsAllowedToReceiveFrom(neighborHolder)
                    && neighborHolder.isDirty
                )
                {
                    AddHolder(neighborHolder, true);
                }
            }
        }
    }

    private void OnTick()
    {
        foreach (ResourceHolder resourceHolder in _resourceHolders)
            resourceHolder.OnTick();

        UpdateWillFlush();
        Move();
    }

    public void UpdateWillFlush()
    {
        foreach (ResourceHolder resourceHolder in _resourceHolders)
            resourceHolder.ResetWillFlush();

        foreach (ResourceHolder resourceHolder in _resourceHolders)
            resourceHolder.UpdateWillFlush();
    }

    public void Move()
    {
        foreach (ResourceHolder resourceHolder in _resourceHolders)
            resourceHolder.Flush();
    }
}
