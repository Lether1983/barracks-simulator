using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LookUp<Tkey, TValue> : IEnumerable<KeyValuePair<Tkey,List<TValue>>>
{
    private Dictionary<Tkey, List<TValue>> lookUp = new Dictionary<Tkey, List<TValue>>();

    public List<TValue> Values(Tkey tkey)
    {
        if (lookUp.ContainsKey(tkey))
        {
            return lookUp[tkey];
        }
        return new List<TValue>();
    }

    public void Add(Tkey tkey, TValue tValue)
    {
        if (!lookUp.ContainsKey(tkey))
        {
            lookUp[tkey] = new List<TValue>();
        }
        lookUp[tkey].Add(tValue);
    }

    public void Remove(Tkey tkey, TValue tValue)
    {
        if (!lookUp.ContainsKey(tkey)) return;
        lookUp[tkey].Remove(tValue);
        if (lookUp[tkey].Count == 0)
        {
            Remove(tkey);
        }
    }

    public void Remove(Tkey tkey)
    {
        lookUp.Remove(tkey);
    }

    public IEnumerator<KeyValuePair<Tkey, List<TValue>>> GetEnumerator()
    {
        return lookUp.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return lookUp.GetEnumerator();
    }
}
