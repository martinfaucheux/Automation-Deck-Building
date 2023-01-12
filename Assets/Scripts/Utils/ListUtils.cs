using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtil
{

    public static void MoveToHead<T>(List<T> list, List<int> indicesToMove)
    {
        foreach(int index in indicesToMove)
        {
            T element = list[index];
            list.RemoveAt(index);
            list.Insert(0, element);
        }
    }
}
