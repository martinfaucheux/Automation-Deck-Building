using System;
using System.Collections.Generic;
using UnityEngine;

public class BeltManager : SingletonBase<BeltManager>
{
    public event Action onTick;
    public float tickPeriod = 0.5f;
    public float moveDuration = 0.1f;
    private float lastTick;
    private GenericGrid<ResourceHolder> _holderGrid = new GenericGrid<ResourceHolder>();
    private List<BeltSystem> _beltSystems = new List<BeltSystem>();

    public void AddHolder(ResourceHolder holder, bool recalculate_system = true)
    {
        _holderGrid[holder.position] = holder;
        if (recalculate_system)
            // TODO: recalculation should be done once at the end of the frame if needed
            BuildSystems();
    }
    public void RemoveHolder(ResourceHolder holder, bool recalculate_system = true)
    {
        bool hasBeenRemoved = _holderGrid.Remove(holder.position);
        if (hasBeenRemoved && recalculate_system)
            // TODO: recalculation should be done once at the end of the frame if needed
            BuildSystems();
    }
    public ResourceHolder GetHolderAtPos(Vector2Int position)
    {
        if (_holderGrid.ContainsKey(position))
            return _holderGrid[position];
        return null;
    }

    private void Start()
    {
        // initialize all preexisting holders
        foreach (ResourceHolder resourceHolder in FindObjectsOfType<ResourceHolder>())
            resourceHolder.Initialize(recalculate_system: false);

        // Building systems is done only once after initialization of all holders
        BuildSystems();
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
