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

    public int AddHolder(ResourceHolder resourceHolder, int index = 0)
    {
        _resourceHolders.Insert(index, resourceHolder);
        resourceHolder.isDirty = false;

        int shiftCount = 0;
        foreach (ResourceHolder neighborHolder in resourceHolder.GetNeighbors())
        {
            if (neighborHolder.isDirty)
            {
                // if neighbor is target of the holder, it should be come before in the list
                if (
                    resourceHolder.GetTargetPos() == neighborHolder.position
                    && resourceHolder.IsAllowedToGive()
                    && neighborHolder.IsAllowedToReceive()
                )
                {
                    int subShiftCount = AddHolder(neighborHolder, index);
                    index += subShiftCount + 1;
                    shiftCount += subShiftCount + 1;
                }

                // if belt is target of neighbor, the belt should come before.
                else if (
                    resourceHolder.position == neighborHolder.GetTargetPos()
                    && neighborHolder.IsAllowedToGive()
                    && resourceHolder.IsAllowedToReceive()
                )
                {
                    int subShiftCount = AddHolder(neighborHolder, index + 1);
                    shiftCount += subShiftCount + 1;
                }
            }
        }
        return shiftCount;
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
