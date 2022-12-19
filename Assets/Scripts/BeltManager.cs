using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeltManager : SingletoneBase<BeltManager>
{
    public event Action onTick;
    public float tickPeriod = 0.5f;
    private float lastTick;
    private GenericGrid<Belt> _beltGrid = new GenericGrid<Belt>();
    private List<BeltSystem> _beltSystems = new List<BeltSystem>();

    public void AddBelt(Belt belt) => _beltGrid[belt.position] = belt;
    public void RemoveBelt(Belt belt) => _beltGrid.Remove(belt.position);
    public Belt GetBeltAtPos(Vector2Int position)
    {
        if (_beltGrid.ContainsKey(position))
            return _beltGrid[position];
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

        foreach (Belt belt in _beltGrid)
            belt.isDirty = true;

        foreach (Belt belt in _beltGrid)
        {
            if (!belt.isDirty)
                continue;

            BeltSystem system = gameObject.AddComponent<BeltSystem>();
            system.AddBelt(belt);
            _beltSystems.Add(system);
        }
    }
}
