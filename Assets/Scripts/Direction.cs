using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Direction : Enumeration
{
    private int xOffset;
    private int yOffset;

    public static Direction NONE = new Direction(0, nameof(NONE), 0, 0);
    public static Direction NE = new Direction(1, nameof(NE), 1, 1);
    public static Direction N = new Direction(2, nameof(N), 0, 2);
    public static Direction NO = new Direction(3, nameof(NO), -1, 1);
    public static Direction SO = new Direction(4, nameof(SO), -1, -1);
    public static Direction S = new Direction(5, nameof(S), 0, -2);
    public static Direction SE = new Direction(6, nameof(SO), 1, -1);

    public Direction(int id, string name, int xOffset, int yOffset) : base(id, name)
    {
        this.xOffset = xOffset;
        this.yOffset = yOffset;
    }

    public Vector2Int ToHexPosition() => new Vector2Int(xOffset, yOffset);
    public float ToAngleDegree() => 60 * this.Id - 30;
}