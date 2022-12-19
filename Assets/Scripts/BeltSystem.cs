using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DirectionEnum;

public class BeltSystem : MonoBehaviour
{
    private List<Belt> _belts = new List<Belt>();

    public void AddBelt(Belt belt, int index = 0)
    {
        _belts.Insert(index, belt);
        _belts.Add(belt);
        belt.isDirty = false;

        foreach ((Direction, Belt) neighbor in belt.GetNeighbors())
        {
            Direction direction = neighbor.Item1;
            Belt neighborBelt = neighbor.Item2;

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

    public bool IsBlocked() => _belts.Where(belt => !belt.CanFlush()).Any();

    public void Move()
    {
        foreach (Belt belt in _belts)
            belt.Flush();
    }
}
