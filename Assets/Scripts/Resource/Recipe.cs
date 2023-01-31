using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ResourceCount
{
    public ResourceType type;
    public int count;
}
[CreateAssetMenu(menuName = "ScriptableObject / Recipe")]
public class Recipe : ScriptableObject
{
    public List<ResourceCount> ingredients;
    public ResourceCount product;
    public Process process;

}
