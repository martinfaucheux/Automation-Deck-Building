using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestHexGridWorldSize : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(
            HexGrid.instance.worldSize.x,
            HexGrid.instance.worldSize.y,
            1f
        );
    }
}