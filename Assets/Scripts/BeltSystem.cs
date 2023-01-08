using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DirectionEnum;

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

    public void AddBelt(ResourceHolder resourceHolder, int index = 0)
    {
        _resourceHolders.Insert(index, resourceHolder);
        resourceHolder.isDirty = false;

        foreach ((Direction direction, ResourceHolder neighborHolder) in resourceHolder.GetNeighbors())
        {
            if (neighborHolder.isDirty)
            {
                // if neighbor is target of belt, it should be come before in the list
                if (resourceHolder.GetTargetPos() == neighborHolder.position)
                    AddBelt(neighborHolder, index);

                // if belt is target of neighbor, the belt should come before.
                else if (resourceHolder.position == neighborHolder.GetTargetPos())
                    AddBelt(neighborHolder, index + 1);
            }
        }
    }

    private void OnTick()
    {
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
            if (resourceHolder.willFlush)
                resourceHolder.Flush();
    }
}
