using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DirectionEnum;

public class BeltSystem : MonoBehaviour
{
    [SerializeField] List<Belt> _belts = new List<Belt>();

    void Start()
    {
        BeltManager.instance.onTick += OnTick;
    }

    void OnDestroy()
    {
        BeltManager.instance.onTick -= OnTick;
    }

    public void AddBelt(Belt belt, int index = 0)
    {
        _belts.Insert(index, belt);
        belt.isDirty = false;

        foreach ((Direction direction, Belt neighborBelt) in belt.GetNeighbors())
        {
            if (neighborBelt.isDirty)
            {
                // if neighbor is target of belt, it should be come before in the list
                if (belt.GetTargetPos() == neighborBelt.position)
                    AddBelt(neighborBelt, index);

                // if belt is target of neighbor, the belt should come before.
                else if (belt.position == neighborBelt.GetTargetPos())
                    AddBelt(neighborBelt, index + 1);
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
        foreach (Belt belt in _belts)
            belt.ResetWillFlush();

        foreach (Belt belt in _belts)
            belt.UpdateWillFlush();
    }

    public void Move()
    {
        foreach (Belt belt in _belts)
            if (belt.willFlush)
                belt.Flush();
    }
}
