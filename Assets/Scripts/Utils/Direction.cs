using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


// Define an extension method in a non-nested static class.
namespace DirectionEnum
{
    public static class Extensions
    {
        public static float ToAngleDegree(this Direction direction)
        {
            return 60 * (int)direction + 30;
        }

        public static Vector2Int ToHexPosition(this Direction direction)
        {
            int x = 0;
            int y = 0;
            switch (direction)
            {
                case Direction.NONE:
                    break;
                case Direction.NE:
                    x = 1;
                    y = 1;
                    break;
                case Direction.N:
                    y = 2;
                    break;
                case Direction.NO:
                    x = -1;
                    y = 1;
                    break;
                case Direction.SO:
                    x = -1;
                    y = -1;
                    break;
                case Direction.S:
                    y = -2;
                    break;
                case Direction.SE:
                    x = 1;
                    y = -1;
                    break;
            }
            return new Vector2Int(x, y);
        }

        public static Direction Opposite(this Direction direction)
        {
            if (direction == Direction.NONE)
                return Direction.NONE;
            return (Direction)(((int)direction + 3) % 6);
        }
    }

    public enum Direction { NE = 0, N = 1, NO = 2, SO = 3, S = 4, SE = 5, NONE = 6 };

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}