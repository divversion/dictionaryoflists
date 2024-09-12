namespace DictionaryOfLists;

/// <summary>
///     Extension class for <see cref="IDictionary{TKey,TValue}" /> that adds the <see cref="GetValueOrAdd{TKey,TValue}" />
///     method.
/// </summary>
public static class DictionaryGetValueOrAddExtension
{
    /// <summary>
    ///     Get an element from a dictionary of creates a new element if not found and adds this to the dictionary.
    /// </summary>
    /// <param name="dict">This dictionary.</param>
    /// <param name="key">The key to look up.</param>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>The found or created item.</returns>
    public static TValue GetValueOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        where TValue : new()
    {
        if (dict.TryGetValue(key, out var item))
            return item;

        item = new TValue();
        dict.Add(key, item);
        return item;
    }
}