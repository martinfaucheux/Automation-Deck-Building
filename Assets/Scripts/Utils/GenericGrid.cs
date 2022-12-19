using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericGrid<T> : IEnumerable<T>
{

    private Dictionary<Vector2Int, T> _dict = new Dictionary<Vector2Int, T>();

    public T this[Vector2Int vect]
    {
        get => _dict[vect];
        set => _dict[vect] = value;
    }

    public T this[int x, int y]
    {
        get => this[new Vector2Int(x, y)];
        set => _dict[new Vector2Int(x, y)] = value;
    }

    public bool ContainsKey(Vector2Int vect) => _dict.ContainsKey(vect);

    public bool Remove(Vector2Int vect)
    {
        if (_dict.ContainsKey(vect))
        {
            _dict.Remove(vect);
            return true;
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T element in _dict.Values)
        {
            yield return element;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
