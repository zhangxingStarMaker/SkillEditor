using System;
using Module.Utility;
using UnityEngine;

namespace Module.Collider2D
{
    public static class Vector2IntExt
    {
        //注意超出范围问题
        public static long Dot(Vector2Int vec1,Vector2Int vec2)
        {
            return (long)vec1.x * vec2.x + (long)vec1.y * vec2.y;
        }
        
        public static long DistanceSquare(Vector2Int a, Vector2Int b)
        {
            long num1 = a.x - b.x;
            long num2 = a.y - b.y;
            return num1 * num1 + num2 * num2;
        }
        
        public static long DistanceSquare(this Vector2Int a)
        {
            long xSquare = (long)a.x * a.x;
            long ySquare = (long)a.y * a.y;
            return xSquare + ySquare;
        }
        
        public static int SafeMagnitude(this Vector2Int a)
        {
            long xSquare = (long)a.x * a.x;
            long ySquare = (long)a.y * a.y;
            Mathf.Sqrt(1);
            int value = (int)Math.Sqrt(xSquare + ySquare);
            return value;
        }
        
        public static Vector2Int ToVector2Int(this Vector2 a)
        {
            return new Vector2Int((int)(a.x),(int)(a.y));
        }
    }
}