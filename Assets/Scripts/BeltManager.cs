using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeltManager : SingletoneBase<BeltManager>
{
    public event Action onTick;
    public float tickPeriod = 0.5f;
    public float moveDuration = 0.1f;
    private float lastTick;
    private GenericGrid<ResourceHolder> _holderGrid = new GenericGrid<ResourceHolder>();
    private List<BeltSystem> _beltSystems = new List<BeltSystem>();

    public void AddHolder(ResourceHolder holder) => _holderGrid[holder.position] = holder;
    public void RemoveHolder(ResourceHolder holder) => _holderGrid.Remove(holder.position);
    public ResourceHolder GetHolderAtPos(Vector2Int position)
    {
        if (_holderGrid.ContainsKey(position))
            return _holderGrid[position];
        return null;
    }

    void Update()
    {
        if (Time.time > lastTick + tickPeriod)
        {
            lastTick = Time.time;
            Tick();
        }
    }

    public void Tick()
    {
        if (onTick != null)
            onTick();
    }

    private void CleanSystems()
    {
        foreach (BeltSystem system in _beltSystems)
            Destroy(system);
        _beltSystems = new List<BeltSystem>();
    }

    public void BuildSystems()
    {
        CleanSystems();

        foreach (ResourceHolder resourceHolder in _holderGrid)
            resourceHolder.isDirty = true;

        foreach (ResourceHolder resourceHolder in _holderGrid)
        {
            if (!resourceHolder.isDirty)
                continue;

            BeltSystem system = gameObject.AddComponent<BeltSystem>();
            system.AddHolder(resourceHolder);
            _beltSystems.Add(system);
        }
    }
}
