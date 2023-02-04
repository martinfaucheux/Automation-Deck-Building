using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
            return DirectionUtil.positionMap[direction];
        }

        public static Direction Opposite(this Direction direction)
        {
            if (direction == Direction.NONE)
                return Direction.NONE;
            return (Direction)(MathUtil.Modulo((int)direction + 3, 6));
        }

        public static int GetAngleTo(this Direction direction, Direction otherDirection)
        {
            return MathUtil.Modulo((int)otherDirection - (int)direction, 6) * 60;
        }

        public static Direction Rotate(this Direction direction, int incr = 1)
        {
            return (Direction)(MathUtil.Modulo((int)direction + incr, 6));
        }

        public static Direction ToReferential(this Direction direction, Direction referential)
        {
            return direction.Rotate(-(int)referential);
        }

    }

    public enum Direction { NE = 0, N = 1, NO = 2, SO = 3, S = 4, SE = 5, NONE = 6 };

    public static class DirectionUtil
    {
        public static readonly Dictionary<Direction, Vector2Int> positionMap = new Dictionary<Direction, Vector2Int>{
            {Direction.NONE, new Vector2Int(0,0)},
            {Direction.NE, new Vector2Int(1, 1)},
            {Direction.N, new Vector2Int(0,2)},
            {Direction.NO, new Vector2Int(-1,1)},
            {Direction.SO, new Vector2Int(-1,-1)},
            {Direction.S, new Vector2Int(0,-2)},
            {Direction.SE, new Vector2Int(1,-1)},
        };

        public static Direction? GetDirectionFromPosition(Vector2Int positionDiff)
        {
            foreach (Direction direction in EnumUtil.GetValues<Direction>())
            {
                if (direction.ToHexPosition() == positionDiff)
                    return direction;
            }
            return null;
        }
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}