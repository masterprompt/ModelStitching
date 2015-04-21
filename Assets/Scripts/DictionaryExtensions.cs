using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
    /// <summary>
    /// Add dictionary items to an existing dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> target)
    {
        foreach (var item in target)
            source[item.Key] = item.Value;
    }

    public static TValue Find<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
    {
        TValue value;
        source.TryGetValue(key, out value);
        return value;
    }
}