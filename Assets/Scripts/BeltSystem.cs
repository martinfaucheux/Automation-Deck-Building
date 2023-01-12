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

        ResourceHolder targetHolder = resourceHolder.GetTargetHolder();
        if (targetHolder != null && targetHolder.isDirty)
        {
            AddHolder(targetHolder, false);
        }

        foreach (ResourceHolder neighborHolder in resourceHolder.GetNeighbors())
        {
            if ((neighborHolder.GetTargetHolder() == resourceHolder) && neighborHolder.isDirty)
            {
                AddHolder(neighborHolder, true);
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
