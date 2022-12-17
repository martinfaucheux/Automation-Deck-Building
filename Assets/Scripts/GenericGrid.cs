using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGrid<T> : IEnumerable<T>
{

    private Dictionary<Vector2Int, List<T>> _dict = new Dictionary<Vector2Int, List<T>>();

    public List<T> this[Vector2Int vect]
    {
        get => _dict[vect];
        set => _dict[vect] = value;
    }

    public List<T> this[int x, int y]
    {
        get => this[new Vector2Int(x, y)];
        set => _dict[new Vector2Int(x, y)] = value;
    }

    public bool ContainsKey(Vector2Int vect) => _dict.ContainsKey(vect);

    public IEnumerator<T> GetEnumerator()
    {
        foreach (List<T> eltList in _dict.Values)
        {
            foreach (T element in eltList)
                yield return element;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
